using ElsaV6.Contexts;
using System;
using System.Threading.Tasks;

namespace Commands.Development
{
    public class Say : ElsaV6.Command
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
