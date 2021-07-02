using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsaV6.Contexts
{
    public class RoomContext : Context
    {
        private IDictionary<char, int> RANKS = new Dictionary<char, int>
        {
            ['&'] = 6, ['#'] = 5, ['*'] = 4,
            ['@'] = 3, ['%'] = 2, ['+'] = 1,
            [' '] = 0
        };

        private Room _room;

        public RoomContext(Bot bot, string target, User sender, string command, Room room)
            : base(bot, target, sender, command)
        {
            _room = room;
        }

        public override string RoomID => _room.RoomID;

        public override bool IsPM => false;

        public override bool HasRank(char requiredRank)
        {
            return Bot.Config.Whitelist.Contains(Sender.UserID)
                || RANKS[Sender.Rank] >= RANKS[requiredRank];
        }

        public override async Task Reply(string message)
        {
            await Bot.Say(RoomID, message);
        }

        public override async Task SendHtml(string html, string room = null)
        {
            await Bot.Say(RoomID, $"/addhtmlbox {html}");
        }

        public override async Task SendUHtml(string id, string html, bool changes)
        {
            var command = changes ? "changeuhtml" : "adduhtml";
            await Bot.Say(RoomID, $"/{command} {id}, {html}");
        }
    }
}
