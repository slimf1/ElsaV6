using DataAccess.Repository.Badge;
using DataAccess.Repository.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repository
{
    public class BotRepository : IBotRepository
    {
        private bool _disposedValue;

        public IBadgeRepository BadgeRepository { get; set; }
        public IUserRepository UserRepository { get; set; }
        
        public BotRepository()
            : this(new ElsaContext())
        {

        }

        public BotRepository(ElsaContext context)
            : this(new BadgeRepository(context), new UserRepository(context))
        {

        }

        public BotRepository(IBadgeRepository badgeRepository, IUserRepository userRepository)
        {
            BadgeRepository = badgeRepository;
            UserRepository = userRepository;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                }

                BadgeRepository.Dispose();
                UserRepository.Dispose();
                _disposedValue = true;
            }
        }

        ~BotRepository()
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
