using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Websocket.Client;

namespace ElsaV6
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Bot bot = new Bot(Config.LoadFromFile(Path.Combine("Resources", "config.json")));
            await bot.Start();
            var exitEvent = new ManualResetEvent(false);
            exitEvent.WaitOne();
        }
    }
}
