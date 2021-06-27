using System;
using System.Collections.Generic;
using System.Text;

namespace ElsaV6
{
    class User
    {
        private string _userName;

        public User(string userName)
        {
            _userName = userName.Substring(1);

            UserID = Utils.Text.ToLowerAlphaNum(_userName);
            Rank = userName[0];
        }

        public string UserID { get; }
        public char Rank { get; }
    }
}
