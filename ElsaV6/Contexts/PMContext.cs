using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElsaV6.Contexts
{
    public class PMContext : Context
    {
        public PMContext(Bot bot, string target, User sender, string command)
            : base(bot, target, sender, command)
        {

        }

        public override string RoomID => Bot.Config.DefaultRoom;

        public override bool IsPM => true;

        public override bool HasRank(char requiredRank)
        {
            return true;
        }

        public override async Task Reply(string message)
        {
            await Bot.Send($"|/pm {Sender.UserID}, {message}");
        }

        public override async Task SendHtml(string html, string room = null)
        {
            await Bot.Say(room != null ? room : Bot.Config.DefaultRoom,
                $"/pminfobox {Sender.UserID}, {html}");
        }

        public override async Task SendUHtml(string id, string html, bool changes)
        {
            var command = changes ? "pmchangeuhtml" : "pmuhtml";
            await Bot.Say(Bot.Config.DefaultRoom,
                $"/{command} {Sender.UserID}, {id}, {html}");
        }
    }
}
