using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repository.Badge
{
    public interface IBadgeRepository : IDisposable
    {
        IEnumerable<BadgeModel> GetBadges();
        BadgeModel GetBadge(int badgeID);
        bool InsertBadge(BadgeModel badge);
        bool DeleteBadge(int badgeID);
        bool UpdateBadge(BadgeModel badge);
        void Save();
    }
}
