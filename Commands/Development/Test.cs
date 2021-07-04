using ElsaV6.Contexts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Commands.Development
{
    public class Test : ElsaV6.Command
    {
        public Test()
        {
            Name = "test";
            WLOnly = true;
            AllowedInPM = true;
        }

        public override async Task Run(Context context)
        {
            //context.Bot.BotRepository.UserRepository.InsertUser(new DataAccess.Models.UserModel() { Name = "Allah", UserID = "allah" });
            //context.Bot.BotRepository.UserRepository.Save();
            var test = context.Bot.BotRepository.UserRepository.GetUsers();
            await context.Reply("test");
        }
    }
}
