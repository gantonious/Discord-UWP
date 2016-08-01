using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace Discord_UWP.Gateway.Sockets
{
    public class WebMessageSocket : IWebMessageSocket
    {
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;
        public event EventHandler<ConnectionClosedEventArgs> ConnectionClosed;      

        private readonly MessageWebSocket _socket;

        public WebMessageSocket()
        {
            _socket = GetMessageWebSocket();
        }

        private MessageWebSocket GetMessageWebSocket()
        {
            var socket = new MessageWebSocket();
            socket.Control.MessageType = SocketMessageType.Utf8;
            socket.MessageReceived += HandleMessage;
            socket.Closed += HandleClosed;

            return socket;
        }

        public async Task ConnectAsync(string connectionUrl)
        {
            await _socket.ConnectAsync(new Uri(connectionUrl));
        }

        public async Task SendMessageAsync(string message)
        {
            using (var dataWriter = new DataWriter(_socket.OutputStream))
            {
                dataWriter.WriteString(message);
                await dataWriter.StoreAsync();
            }
        }

        private void HandleMessage(object sender, MessageWebSocketMessageReceivedEventArgs e)
        {
            using (var dataReader = e.GetDataReader())
            {
                string messageString = dataReader.ReadString(dataReader.UnconsumedBufferLength);
                OnMessageReceived(messageString);
            }
        }

        private void OnMessageReceived(string message)
        {
            var messageReceivedEvent = new MessageReceivedEventArgs
            {
                Message = message
            };

            MessageReceived?.Invoke(this, messageReceivedEvent);
        }

        private void HandleClosed(object sender, WebSocketClosedEventArgs args)
        {
            OnClosed();
        }

        private void OnClosed()
        {
            var connectionClosedEvent = new ConnectionClosedEventArgs();

            ConnectionClosed?.Invoke(this, connectionClosedEvent);
        }
    }
}
