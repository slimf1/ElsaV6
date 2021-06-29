using ElsaV6.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsaV6
{
    public abstract class Command
    {
        public string Name { get; protected set; } = "";
        public string[] Aliases { get; protected set; } = { };
        public bool AllowedInPM { get; protected set; } = false;
        public bool WLOnly { get; protected set; } = false;
        public bool PMOnly { get; protected set; } = false;
        public char RequiredRank { get; protected set; } = '&';
        public string HelpMessage { get; protected set; } = "";
        public bool Hidden { get; protected set; } = false;

        public Command()
        {

        }

        public async Task Call(Context context)
        {
            if (PMOnly && !context.IsPM)
                return;
            if (context.IsPM && !(AllowedInPM || PMOnly))
                return;
            if (WLOnly && !context.Bot.Config.Whitelist.Contains(context.Sender.UserID))
                return;
            if (!context.HasRank(RequiredRank))
                return;

            await Run(context);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public abstract Task Run(Context context);
    }
}
