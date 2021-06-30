using ElsaV6;
using System;
using System.Collections.Generic;
using System.Text;

namespace Commands.Development
{
    public class Greeter : Reader
    {
        public override async void Read(Bot bot, string[] messageParts, Room room)
        {
            if (messageParts.Length > 1 && messageParts[1].Equals("pm"))
            {
                if (messageParts[4].Equals("hello", StringComparison.OrdinalIgnoreCase) && new Random().NextDouble() > .5)
                {
                    await bot.Send($"|/pm {messageParts[2]}, hi n_n");
                }
            }
        }
    }
}
