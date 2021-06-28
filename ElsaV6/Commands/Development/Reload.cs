using ElsaV6.Contexts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElsaV6.Commands.Development
{
    class Reload : Command
    {
        public Reload()
        {
            Name = "reload";
            WLOnly = true;
            AllowedInPM = true;
            Aliases = new string[] { "refresh" };
        }

        public override async Task Run(Context context)
        {
            try
            {
                // Recompiler les DLL et les recharger ?
                // Une DLL par plugin ?
                //context.Bot.LoadCommandsFromAssembly();
                await context.Reply("Commandes rechargées");
            } 
            catch(Exception e)
            {
                await context.Reply("Impossible de recharger les commandes");
                Utils.Logger.Error("Reload commands error : " + e.StackTrace);
                //await context.Reply("!code " + e.StackTrace);
            }
        }
    }
}
