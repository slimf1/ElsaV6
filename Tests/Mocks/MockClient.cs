using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ElsaV6.Clients;

namespace Tests.Mocks
{
    class MockClient : IClient
    {
        public IList<string> Messages { get; }
        private bool _connected;

        public MockClient()
        {
            Messages = new List<string>();
            _connected = false;
        }

        public TimeSpan? ReconnectTimeout { get; set; }

        public IObservable<string> MessageReceived { get; }

        public void Dispose()
        {
        }

        public Task Send(string message)
        {
            if (!_connected)
                throw new Exception("Cannot send while not connected");
            Messages.Add(message);
            return Task.FromResult<object>(null);
        }

        public Task Start()
        {
            _connected = true;
            return Task.FromResult<object>(null);
        }
    }
}
