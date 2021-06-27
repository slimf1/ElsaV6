using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElsaV6.Contexts
{
    abstract class Context
    {
        public Bot Bot { get; }
        public string Target { get; }
        public User Sender { get; }
        public string Command { get; }

        public abstract string RoomID { get; }

        public Context(Bot bot, string target, User sender, string command)
        {
            Bot = bot;
            Target = target;
            Sender = sender;
            Command = command;
        }

        public abstract bool IsPM();

        public abstract bool HasRank(char requiredRank);

        public abstract Task Reply(string message);

        public abstract Task SendHtml(string html);

        public abstract Task SendUHtml(string id, string html, bool changes);
    }
}
