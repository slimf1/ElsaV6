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

namespace ElsaV6
{
    public class Bot : IDisposable
    {
        private const int MAX_MESSAGE_LENGTH = 100000;
        private static readonly TimeSpan SAME_MESSAGE_COOLDOWN = new TimeSpan(0, 0, 2); 

        private IClient _client;
        private Config _config;

        private DateTime? _lastMessageTime;
        private string _lastMessage;
        private string _currentRoom;
        private bool _disposedValue;
        private IDictionary<string, Command> _commands;

        public Config Config { get; }

        public Bot(IClient client, Config config)
        {
            _client = client;
            _config = config;
            _lastMessage = "";
            _lastMessageTime = null;
            _commands = new Dictionary<string, Command>();

            _client.ReconnectTimeout = new TimeSpan(0, 0, 20);
            
            Logger.Enabled = _config.Log;
            LoadCommands();
        }

        private void LoadCommands()
        {
            var commandTypes = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(Command)));

            foreach(var commandType in commandTypes)
            {
                var commandInstance = (Command)Activator.CreateInstance(commandType);
                _commands.Add(commandInstance.Name, commandInstance);
            }
        }

        public Bot(Config config) 
            : this(new Client(new Uri($"ws://{config.Host}:{config.Port}/showdown/websocket")), config)
        {

        }

        public async Task Say(string room, string message)
        {
            var now = DateTime.Now;
            if (_lastMessage.Equals(message) && now - _lastMessageTime < SAME_MESSAGE_COOLDOWN) ;
                //return;

            if (message.Length > MAX_MESSAGE_LENGTH)
                return;

            await _client.Send($"{room}|{message}");
            Logger.Message($"[S] ({room}) {message}");

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

        private void LoadRoom(string message, string room)
        {
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
                default:
                    Logger.Debug($"Unsupported message type: {parts[1]}");
                    break;
            }
        }

        private async Task ChatMessage(string message, string senderName, string roomName, string timestampVal)
        {
            int triggerLength = _config.Trigger.Length;
            if (!message.Substring(0, triggerLength).Equals(_config.Trigger))
                return;

            var text = message.Substring(triggerLength);
            int spaceIndex = text.IndexOf(' ');
            var command = spaceIndex > 0 
                ? text.Substring(spaceIndex).ToLower() 
                : text.Trim().ToLower();

            if (string.IsNullOrEmpty(Text.ToLowerAlphaNum(command)))
                return;

            var target = spaceIndex > 0
                ? text.Substring(spaceIndex + 1)
                : "";

            var context = new RoomContext(this, target, new User(" Panur"), command, new Room("botdevelopment"));
            
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
            if (name.Equals(_config.Name))
            {
                Logger.Info($"Connection sucessful as {name} ");
                foreach(var room in _config.Rooms)
                {
                    await _client.Send($"|/join {room}");
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
                ["name"] = _config.Name,
                ["pass"] = _config.Password,
                ["challstr"] = challstr
            };
            var textResponse = await Http.PostAsync(url, parameters);
            JObject jsonResponse = (JObject)JsonConvert.DeserializeObject(textResponse.Substring(1));
            var nonce = jsonResponse.GetValue("assertion");
            await _client.Send($"|/trn {_config.Name},0,{nonce}");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: supprimer l'état managé (objets managés)
                }

                _client.Dispose();
                _client = null;
                _disposedValue = true;
            }
        }

        // // TODO: substituer le finaliseur uniquement si 'Dispose(bool disposing)' a du code pour libérer les ressources non managées
        // ~Bot()
        // {
        //     // Ne changez pas ce code. Placez le code de nettoyage dans la méthode 'Dispose(bool disposing)'
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Ne changez pas ce code. Placez le code de nettoyage dans la méthode 'Dispose(bool disposing)'
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
