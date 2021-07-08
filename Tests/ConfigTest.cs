using ElsaV6;
using NUnit.Framework;

namespace Tests
{
    public class ConfigTest
    {
        [Test]
        public void LoadFromFile()
        {
            Config config = Config.LoadFromFile(@"Resources/test-config.json");
            Assert.AreEqual(config.Host, "sim3.psim.us");
            Assert.AreEqual(config.Port, 80);
            Assert.AreEqual(config.Name, "TestBot");
            Assert.AreEqual(config.Password, "password123");
            Assert.IsTrue(config.Log);
            Assert.AreEqual(config.Rooms, new string[] { "botdevelopment", "overused" });
            Assert.AreEqual(config.Whitelist, new string[] { "panur" });
            Assert.AreEqual(config.Trigger, "-");
            Assert.AreEqual(config.Blacklist, new string[] { "wally" });
            Assert.AreEqual(config.RoomBlacklist, new string[] { "lobby", "ignoredroom" });
            Assert.AreEqual(config.DefaultRoom, "franais");
            Assert.IsTrue(config.Keys.ContainsKey("key1"));
            Assert.IsTrue(config.Keys.ContainsKey("key2"));
            Assert.AreEqual(config.Keys["key1"], "value1");
            Assert.AreEqual(config.Keys["key2"], "value2");
        }
    }
}
