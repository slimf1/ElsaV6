using ElsaV6.Contexts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace Commands.Development
{
    class Eval : ElsaV6.Command
    {
        public Eval()
        {
            Name = "eval";
            WLOnly = true;
            AllowedInPM = true;
        }

        public override async Task Run(Context context)
        {
            try
            {
                // Eg System.Math.Pow(2, 4)
                var result = await CSharpScript.EvaluateAsync(context.Target);
                await context.Reply(result?.ToString());
            } catch(Exception e)
            {
                await context.Reply("Error: " + e.Message);
            }

        }
    }
}
