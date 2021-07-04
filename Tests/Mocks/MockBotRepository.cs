using DataAccess.Repository;
using DataAccess.Repository.Badge;
using DataAccess.Repository.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Mocks
{
    class MockBotRepository : IBotRepository
    {
        private bool disposedValue;

        public IBadgeRepository BadgeRepository { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IUserRepository UserRepository { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                disposedValue = true;
            }
        }

        ~MockBotRepository()
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
