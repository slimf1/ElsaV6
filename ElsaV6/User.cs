using System;
using System.Collections.Generic;
using System.Text;

namespace ElsaV6
{
    class User
    {
        private char _rank;
        private string _userName;

        public User(string userName)
        {
            _rank = userName[0];
            _userName = userName.Substring(1);

            UserID = Utils.Text.ToLowerAlphaNum(_userName);
        }

        public string UserID { get; }
    }
}
