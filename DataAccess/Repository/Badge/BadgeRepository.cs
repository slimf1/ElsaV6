using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repository.Badge
{
    class BadgeRepository : IBadgeRepository
    {
        private readonly ElsaContext _context;
        private bool _disposedValue;

        public BadgeRepository(ElsaContext context)
        {
            _context = context;
            _disposedValue = false;
        }

        public bool DeleteBadge(int badgeID)
        {
            BadgeModel badge = _context.Badges.Find(badgeID);
            if (badge == null)
            {
                return false;
            }
            _context.Badges.Remove(badge);
            return true;
        }

        public BadgeModel GetBadge(int badgeID)
        {
            return _context.Badges.Find(badgeID);
        }

        public IEnumerable<BadgeModel> GetBadges()
        {
            return _context.Badges.ToList();
        }

        public bool InsertBadge(BadgeModel badge)
        {
            _context.Badges.Add(badge);
            return true;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public bool UpdateBadge(BadgeModel badge)
        {
            BadgeModel existingBadge = _context.Badges.Find(badge.BadgeID);
            if (existingBadge == null)
            {
                return false;
            }
            existingBadge.Description = badge.Description;
            existingBadge.Image = badge.Image;
            existingBadge.IsTrophy = badge.IsTrophy;
            existingBadge.Name = badge.Name;
            return true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                }

                _context.Dispose();
                _disposedValue = true;
            }
        }

        ~BadgeRepository()
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
