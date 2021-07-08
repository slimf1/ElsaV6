using ElsaV6.Contexts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ElsaV6;

namespace ElsaV6.Commands.Development
{
    class Ping : Command
    {
        public Ping()
        {
            Name = "ping";
            AllowedInPM = true;
            RequiredRank = ' ';
            Aliases = new string[] { "pingalias" };
        }

        public override async Task Run(Context context)
        {
            await context.Reply("pong");
        }
    }
}
