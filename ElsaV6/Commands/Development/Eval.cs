using ElsaV6.Contexts;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElsaV6.Commands.Development
{
    public class Eval : Command
    {
        public Eval()
        {
            Name = "eval";
            WLOnly = true;
            AllowedInPM = true;
        }

        class Globals
        {
            public Context context;
        }

        public override async Task Run(Context context)
        {
            try
            {
                var globals = new Globals { context = context };
                var result = await CSharpScript.EvaluateAsync(context.Target, globals: globals);
                await context.Reply(result.ToString());
            }
            catch (Exception e)
            {
                await context.Reply("Error: " + e.Message);
            }
        }
    }
}
