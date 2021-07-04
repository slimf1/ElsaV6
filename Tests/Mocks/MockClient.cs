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
        private MessageObservable _messageObservable;

        public MockClient()
        {
            Messages = new List<string>();
            _messageObservable = new MessageObservable();
            _connected = false;
        }

        public TimeSpan? ReconnectTimeout { get; set; }

        public IObservable<string> MessageReceived => _messageObservable;

        public void ReceiveMessage(string message)
        {
            _messageObservable.Send(message);
        }

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

    class MessageObservable : IObservable<string>
    {
        private IObserver<string> _observer;
        public IDisposable Subscribe(IObserver<string> observer)
        {
            _observer = observer;
            return null;
        }

        public void Send(string message)
        {
            _observer?.OnNext(message);
        }
    }
}
