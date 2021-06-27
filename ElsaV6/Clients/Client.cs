using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Websocket.Client;

namespace ElsaV6.Clients
{
    class Client : IClient
    {
        private WebsocketClient _client;
        private bool _disposedValue;

        public Client(Uri uri) // IDP ?
        {
            _client = new WebsocketClient(uri);
        }

        public TimeSpan? ReconnectTimeout 
        { 
            get => _client.ReconnectTimeout; 
            set => _client.ReconnectTimeout = value; 
        }

        IObservable<string> IClient.MessageReceived
            => _client.MessageReceived.Select(res => res.Text);

        public async Task Send(string message)
        {
            await Task.Run(() => _client.Send(message));
        }

        public async Task Start()
        {
            await _client.Start();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: supprimer l'état managé (objets managés)
                }
                
                _client.Dispose();
                _client = null;
                _disposedValue = true;
            }
        }

        // // TODO: substituer le finaliseur uniquement si 'Dispose(bool disposing)' a du code pour libérer les ressources non managées
        // ~Client()
        // {
        //     // Ne changez pas ce code. Placez le code de nettoyage dans la méthode 'Dispose(bool disposing)'
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Ne changez pas ce code. Placez le code de nettoyage dans la méthode 'Dispose(bool disposing)'
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
