using DataAccess.Repository.Badge;
using DataAccess.Repository.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repository
{
    public interface IBotRepository : IDisposable
    {
        IBadgeRepository BadgeRepository { get; set; }
        IUserRepository UserRepository { get; set; }
    }
}
