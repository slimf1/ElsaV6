using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElsaV6.Clients
{
    public interface IClient : IDisposable
    {
        TimeSpan? ReconnectTimeout { get; set; }
        IObservable<string> MessageReceived { get; }
        Task Send(string message);
        Task Start();
    }
}
