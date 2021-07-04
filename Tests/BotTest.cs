using ElsaV6;
using NUnit.Framework;
using System;
using System.Text;
using System.Threading;
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
            _mockClient.ReceiveMessage(">testroom\n" +
                "|init|chat\n" +
                "|title|Test Room|\n" +
                "|users|4,*BotUser,@Panur, RegularUser,#RoomOwner");
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
            Thread.Sleep(5000);
            await _bot.Say("room1", "test");
            await _bot.Say("room1", "test");
            Assert.AreEqual(_mockClient.Messages.Count, 4);
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
            _mockClient.ReceiveMessage(">testroom\n|c:|1| RegularUser|-say test");
            Assert.AreEqual(_mockClient.Messages.Count, 0);
            _mockClient.ReceiveMessage(">testroom\n|c:|1|#RoomOwner|-say test");
            Assert.AreEqual(_mockClient.Messages.Count, 0);
            _mockClient.ReceiveMessage(">testroom\n|c:|1|@Panur|-say test");
            Assert.AreEqual(_mockClient.Messages[0], "testroom|test");
        }

        [Test]
        public void TestHasRank()
        {
            _mockClient.ReceiveMessage(">testroom\n|c:|1| RegularUser|-profile");
            Assert.AreEqual(_mockClient.Messages.Count, 0);
            _mockClient.ReceiveMessage(">testroom\n|c:|1|#RoomOwner|-profile");
            Assert.AreEqual(_mockClient.Messages.Count, 1);
            _mockClient.ReceiveMessage(">testroom\n|c:|1|@Panur|-profile");
            Assert.AreEqual(_mockClient.Messages.Count, 2);
        }

        [TearDown]
        public void TearDown()
        {
            _bot.Dispose();
        }
    }
}
