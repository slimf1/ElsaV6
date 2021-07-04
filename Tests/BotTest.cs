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
        Bot _bot;

        [SetUp]
        public async Task Setup()
        {
            _mockClient = new MockClient();
            _bot = new Bot(_mockClient, new Config { Host = "", Log = true, Name = "", Password = "", Port = 0 }, null);
            await _bot.Start();
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

        [TearDown]
        public void TearDown()
        {
            _bot.Dispose();
        }
    }
}
