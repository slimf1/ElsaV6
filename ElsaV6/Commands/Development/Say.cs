using ElsaV6.Contexts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElsaV6.Commands.Development
{
    public class Say : Command
    {
        public Say()
        {
            Name = "say";
            WLOnly = true;
            AllowedInPM = true;
        }

        public override async Task Run(Context context)
        {
            await context.Reply(context.Target);
        }
    }
}
