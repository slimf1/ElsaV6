using ElsaV6.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElsaV6
{
    public abstract class Reader
    {
        public abstract void Read(Bot bot, string[] messageParts, Room room);
    }
}
