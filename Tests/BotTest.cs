using ElsaV6;
using NUnit.Framework;
using System.Text;
using System.Threading.Tasks;
using Tests.Mocks;

namespace Tests
{
    public class BotTest
    {
        MockClient _mockClient;
        MockBotRepository _mockRepository;
        Bot _bot;

        [SetUp]
        public async Task Setup()
        {
            _mockClient = new MockClient();
            _mockRepository = new MockBotRepository();
            _bot = new Bot(
                _mockClient, 
                new Config { 
                    Host = "", 
                    Log = true, 
                    Name = "TestBot", 
                    Password = "", 
                    Port = 0, 
                    Blacklist = new string[] { }, 
                    DefaultRoom = "franais", 
                    Rooms = new string[] { "franais" }, 
                    Trigger = "-", 
                    RoomBlacklist = new string[] { }, 
                    Whitelist = new string[] { "panur" } }, 
                _mockRepository);

            await _bot.Start();

            // Initialize the bot with one room
            _mockClient.ReceiveMessage(">franais\n" +
                "|init|chat\n" +
                "|title|Français|\n" +
                "|users|4,*TestUser1,@Panur, TestUser2,#TestUser3");
        }

        [Test]
        public async Task TestCooldown()
        {
            await _bot.Say("room1", "test");
            await _bot.Say("room1", "test");
            await _bot.Say("room1", "test");
            await _bot.Say("room2", "test");
            await _bot.Say("room2", "test");
            await _bot.Say("room3", "test");
            Assert.AreEqual(_mockClient.Messages.Count, 3);
        }

        [Test]
        public async Task TestLimit()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < 20001; ++i)
                builder.Append("xxxxx");
            await _bot.Say("room", builder.ToString());
            Assert.AreEqual(_mockClient.Messages.Count, 0);
        }

        [Test]
        public void TestSayCommand()
        {
            _mockClient.ReceiveMessage(">franais\n|c:|1625422909|@Panur|-say test");
            Assert.AreEqual(_mockClient.Messages[0], "franais|test");
        }

        [TearDown]
        public void TearDown()
        {
            _bot.Dispose();
        }
    }
}
