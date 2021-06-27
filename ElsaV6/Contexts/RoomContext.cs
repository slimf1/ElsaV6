using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsaV6.Contexts
{
    class RoomContext : Context
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

        public override bool HasRank(char requiredRank)
        {
            return Bot.Config.Whitelist.Contains(Sender.UserID)
                || RANKS[Sender.Rank] >= RANKS[requiredRank];
        }

        public override bool IsPM()
        {
            return false;
        }

        public override async Task Reply(string message)
        {
            await Bot.Say(RoomID, message);
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
