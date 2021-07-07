using ElsaV6.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Websocket.Client;
using ElsaV6.Clients;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using ElsaV6.Contexts;
using System.IO;
using DataAccess.Repository.User;
using DataAccess.Repository;

namespace ElsaV6
{
    public class Bot : IDisposable
    {
        private const int MAX_MESSAGE_LENGTH = 100000;
        private static readonly TimeSpan SAME_MESSAGE_COOLDOWN = new TimeSpan(0, 0, 2);

        private IClient _client;
        private DateTime? _lastMessageTime;
        private string _lastMessage;
        private string _currentRoom;
        private bool _disposedValue;
        private string[] _formats;
        private IDictionary<string, Command> _commands;
        private IDictionary<string, Room> _rooms;
        private IDictionary<string, Reader> _readers;
        private IDictionary<string, User> _pmSenders;

        public Config Config { get; }
        public IBotRepository BotRepository { get; }

        public Bot(IClient client, Config config, IBotRepository botRepository)
        {
            Config = config;
            BotRepository = botRepository;
            _client = client;
            _lastMessage = "";
            _lastMessageTime = null;
            _formats = new string[] { };
            _commands = new Dictionary<string, Command>();
            _rooms = new Dictionary<string, Room>();
            _readers = new Dictionary<string, Reader>();
            _pmSenders = new Dictionary<string, User>();

            _client.ReconnectTimeout = new TimeSpan(0, 0, 20);

            Logger.Enabled = Config.Log;
            LoadCommandsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public void LoadCommandsFromAssembly(Assembly assembly)
        {
            var commandsTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Command)));
            var readersTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Reader)));
            
            foreach(var commandType in commandsTypes)
            {
                var commandInstance = (Command)Activator.CreateInstance(commandType);
                _commands[commandInstance.Name] = commandInstance;
                foreach(var alias in commandInstance.Aliases)
                {
                    _commands[alias] = commandInstance;
                }
            }

            foreach(var readerType in readersTypes)
            {
                var readerInstance = (Reader)Activator.CreateInstance(readerType);
                _readers[readerType.FullName] = readerInstance;
            }
        }

        public Bot(Config config) 
            : this(new Client(new Uri($"ws://{config.Host}:{config.Port}/showdown/websocket")), config, new BotRepository())
        {

        }

        public async Task Say(string room, string message)
        {
            await Send($"{room}|{message}");
        }

        public async Task Send(string message)
        {
            var now = DateTime.Now;
            if (_lastMessage != null
                && _lastMessageTime != null
                && _lastMessage.Equals(message)
                && now - _lastMessageTime < SAME_MESSAGE_COOLDOWN)
                return;

            if (message.Length > MAX_MESSAGE_LENGTH)
                return;

            await _client.Send(message);
            Logger.Message($"[S] {message}");

            _lastMessage = message;
            _lastMessageTime = now;
        }

        public async Task Start()
        {
            _client.MessageReceived?.Subscribe(async msg =>
            {
                await MessageReceived(msg);
            });

            await _client.Start();
        }

        private async Task MessageReceived(string message)
        {
            var parts = message.Split('\n');
            string room = null;
            if (parts[0].Length > 0 && parts[0][0] == '>')
            {
                room = parts[0].Substring(1);
                _currentRoom = room;
            }
            if (parts.Length >= 2 && parts[1].StartsWith("|init|chat"))
            {
                LoadRoom(message, room);
            }
            else
            {
                foreach(var line in parts)
                {
                    await ParseMessage(line, room);
                }
            }
        }

        private void LoadRoom(string message, string roomID)
        {
            Logger.Debug($"Loading room {roomID}...");
            var parts = message.Split('\n');
            var roomTitle = parts[2].Split('|')[2];
            var users = parts[3].Split('|')[2].Split(',').Skip(1).ToArray();

            var room = new Room(roomTitle, roomID);
            room.InitializeUsers(users);
            _rooms[roomID] = room;
            Logger.Debug($"{roomID} : Done");
        }

        private async Task ParseMessage(string line, string room)
        {
            var parts = line.Split('|');
            if (parts.Length < 2) return;
            if (string.IsNullOrEmpty(room))
            {
                Logger.Debug("Room set to current :" + _currentRoom);
                room = _currentRoom;
            }

            foreach(var reader in _readers.Values)
            {
                try
                {
                    reader.Read(this, parts, _rooms[room]);
                }
                catch(Exception e)
                {
                    Logger.Error($"Parser crashed: {e.Message}");
                }
            }

            Logger.Message($"({room}) {line}");

            switch(parts[1])
            {
                case "challstr":
                    var challstr = string.Join('|', parts.Skip(2).ToArray());
                    await Login(challstr);
                    break;
                case "updateuser":
                    await CheckConnection(parts);
                    break;
                case "c:":
                    await ChatMessage(parts[4], parts[3], room, parts[2]);
                    break;
                case "J":
                    _rooms[room].JoinUser(parts[2]);
                    break;
                case "L":
                    _rooms[room].LeaveUser(parts[2]);
                    break;
                case "N":
                    _rooms[room].RenameUser(parts[3], parts[2]);
                    break;
                case "pm":
                    await PrivateMessage(parts[4], parts[2]);
                    break;
                case "formats":
                    GetFormats(line);
                    break;
                default:
                    Logger.Debug($"Unsupported message type: {parts[1]}");
                    break;
            }
        }

        private async Task PrivateMessage(string message, string senderName)
        {
            var sender = new User(senderName);
            if (Config.Blacklist.Contains(sender.UserID))
                return;

            _pmSenders[sender.UserID] = sender;

            // TODO: virer la dupli de code avec ChatMessage
            int triggerLength = Config.Trigger.Length;
            if (!message.Substring(0, triggerLength).Equals(Config.Trigger))
                return;

            var text = message.Substring(triggerLength);
            int spaceIndex = text.IndexOf(' ');
            var command = spaceIndex > 0
                ? text.Substring(0, spaceIndex).ToLower()
                : text.Trim().ToLower();

            if (string.IsNullOrEmpty(Text.ToLowerAlphaNum(command)))
                return;

            var target = spaceIndex > 0
                ? text.Substring(spaceIndex + 1)
                : "";

            var context = new PMContext(this, target, sender, command);

            if (_commands.ContainsKey(command))
            {
                try
                {
                    await _commands[command].Call(context);
                }
                catch (Exception e)
                {
                    Logger.Error(e.Message);
                }
            }
        }

        private void GetFormats(string line)
        {
            var formats = line.Split("|").Skip(5).ToArray();

            _formats = formats
                .Where(f => f.StartsWith("[Gen"))
                .Select(f => f.Split(",")[0])
                .ToArray();
        }

        private async Task ChatMessage(string message, string senderName, string roomName, string timestampVal)
        {
            int triggerLength = Config.Trigger.Length;
            if (!message.Substring(0, triggerLength).Equals(Config.Trigger))
                return;

            var text = message.Substring(triggerLength);
            int spaceIndex = text.IndexOf(' ');
            var command = spaceIndex > 0 
                ? text.Substring(0, spaceIndex).ToLower() 
                : text.Trim().ToLower();

            if (string.IsNullOrEmpty(Text.ToLowerAlphaNum(command)))
                return;

            var target = spaceIndex > 0
                ? text.Substring(spaceIndex + 1)
                : "";

            var room = _rooms[roomName];
            var context = new RoomContext(this, target, 
                room.Users[Text.ToLowerAlphaNum(senderName)], command, room);
            
            if (_commands.ContainsKey(command))
            {
                try
                {
                    await _commands[command].Call(context);
                } 
                catch(Exception e)
                {
                    Logger.Error(e.Message);
                }
            }
        }

        private async Task CheckConnection(string[] parts)
        {
            var name = parts[2].Substring(1);
            if (name.Equals(Config.Name))
            {
                Logger.Info($"Connection sucessful as {name} ");
                foreach(var room in Config.Rooms)
                {
                    await Send($"|/join {room}");
                    await Task.Delay(300);
                }
            }
        }

        private async Task Login(string challstr)
        {
            var url = "http://play.pokemonshowdown.com/action.php";
            var parameters = new Dictionary<string, string>
            {
                ["act"] = "login",
                ["name"] = Config.Name,
                ["pass"] = Config.Password,
                ["challstr"] = challstr
            };
            var textResponse = await Http.PostAsync(url, parameters);
            JObject jsonResponse = (JObject)JsonConvert.DeserializeObject(textResponse.Substring(1));
            var nonce = jsonResponse.GetValue("assertion");
            await Send($"|/trn {Config.Name},0,{nonce}");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                }

                BotRepository.Dispose();
                _client.Dispose();
                _client = null;
                _disposedValue = true;
            }
        }

        ~Bot()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
