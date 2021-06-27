using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElsaV6.Contexts
{
    class PMContext : Context
    {
        public PMContext(Bot bot, string target, User sender, string command)
            : base(bot, target, sender, command)
        {

        }

        public override string RoomID => Bot.Config.DefaultRoom;

        public override bool HasRank(char requiredRank)
        {
            throw new NotImplementedException();
        }

        public override bool IsPM()
        {
            throw new NotImplementedException();
        }

        public override Task Reply(string message)
        {
            throw new NotImplementedException();
        }

        public override Task SendHtml(string html)
        {
            throw new NotImplementedException();
        }

        public override Task SendUHtml(string id, string html, bool changes)
        {
            throw new NotImplementedException();
        }
    }
}
