using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repository.User
{
    public class UserRepository : IUserRepository
    {
        private readonly ElsaContext _context;
        private bool _disposedValue;

        public UserRepository(ElsaContext context)
        {
            _context = context;
            _disposedValue = false;
        }

        public bool DeleteUser(string userID)
        {
            UserModel user = _context.Users.Find(userID);
            if (user == null)
            {
                return false;
            }
            _context.Users.Remove(user);
            return true;
        }

        public UserModel GetUser(string userID)
        {
            return _context.Users.Find(userID);
            //return _context.Users.FirstOrDefault(u => u.UserID == userID);
        }

        public IEnumerable<UserModel> GetUsers()
        {
            return _context.Users.ToList();
        }

        public bool InsertUser(UserModel userModel)
        {
            _context.Users.Add(userModel);
            return true;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public bool UpdateUser(UserModel userModel)
        {
            UserModel existingUser = _context.Users.Find(userModel.UserID);
            if (existingUser == null)
            {
                return false;
            }
            existingUser.Name = userModel.Name;
            existingUser.OnTime = userModel.OnTime;
            existingUser.Name = userModel.Name;
            return true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: supprimer l'état managé (objets managés)
                }

                _context.Dispose();
                _disposedValue = true;
            }
        }

        ~UserRepository()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
