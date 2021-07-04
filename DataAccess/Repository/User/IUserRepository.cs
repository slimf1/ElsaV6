using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repository.User
{
    public interface IUserRepository : IDisposable
    {
        IEnumerable<UserModel> GetUsers();
        UserModel GetUser(string userID);
        bool InsertUser(UserModel userModel);
        bool DeleteUser(string userID);
        bool UpdateUser(UserModel userModel);
        void Save();
    }
}
