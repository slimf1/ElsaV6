using ElsaV6.Contexts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Commands.Profile
{
    public class Profile : ElsaV6.Command
    {
        public Profile()
        {
            Name = "profile";
            AllowedInPM = true;
            RequiredRank = '+';
        }

        public override async Task Run(Context context)
        {
            await context.Reply($"/w {context.Sender.UserID}, wip");
        }
    }
}
