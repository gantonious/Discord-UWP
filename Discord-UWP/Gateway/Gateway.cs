using Discord_UWP.Authentication;
using Discord_UWP.Gateway.DownstreamEvents;
using Discord_UWP.Gateway.Sockets;
using Discord_UWP.Gateway.UpstreamEvents;
using Discord_UWP.SharedModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_UWP.Gateway
{
    public class GatewayEventArgs<T> : EventArgs
    {
        public T EventPayload { get; set; }

        public GatewayEventArgs(T eventPayload)
        {
            EventPayload = eventPayload;
        }
    }

    public class Gateway : IGateway
    {
        private delegate void GatewayEventHandler(GatewayEvent gatewayEvent);

        private IDictionary<int, GatewayEventHandler> operationHandlers;
        private IDictionary<string, GatewayEventHandler> eventHandlers;

        private GatewayEvent lastGatewayEvent;

        private readonly IWebMessageSocket _webMessageSocket;
        private readonly IAuthenticator _authenticator;
        private readonly GatewayConfig _gatewayConfig;

        public event EventHandler<GatewayEventArgs<Ready>> Ready;
        public event EventHandler<GatewayEventArgs<Message>> MessageCreated;
        public event EventHandler<GatewayEventArgs<Message>> MessageUpdated;
        public event EventHandler<GatewayEventArgs<MessageDelete>> MessageDeleted;

        public Gateway(GatewayConfig config, IAuthenticator authenticator)
        {
            _webMessageSocket = new WebMessageSocket();
            _authenticator = authenticator;
            _gatewayConfig = config;

            eventHandlers = GetEventHandlers();
            operationHandlers = GetOperationHandlers();

            PrepareSocket();
        }

        private void PrepareSocket()
        {
            _webMessageSocket.MessageReceived += OnSocketMessageReceived;
        }

        public async Task ConnectAsync()
        {
            await _webMessageSocket.ConnectAsync(_gatewayConfig.GetFullGatewayUrl("json", "6"));
        }

        private IDictionary<int, GatewayEventHandler> GetOperationHandlers()
        {
            return new Dictionary<int, GatewayEventHandler>
            {
                { OperationCode.Hello.ToInt() , OnHelloReceived }
            };
        }

        private IDictionary<string, GatewayEventHandler> GetEventHandlers()
        {
            return new Dictionary<string, GatewayEventHandler>
            {
                { "READY", OnReady },
                { "MESSAGE_CREATE", OnMessageCreated },
                { "MESSAGE_UPDATE", OnMessageUpdated },
                { "MESSAGE_DELETE", OnMessageDeleted }
            };
        }

        private void OnSocketMessageReceived(object sender, MessageReceivedEventArgs args)
        {
            var gatewayEvent = JsonConvert.DeserializeObject<GatewayEvent>(args.Message);
            lastGatewayEvent = gatewayEvent;

            if (operationHandlers.ContainsKey(gatewayEvent.Operation.GetValueOrDefault()))
            {
                operationHandlers[gatewayEvent.Operation.GetValueOrDefault()](gatewayEvent);
            }

            if (gatewayEvent.Type != null && eventHandlers.ContainsKey(gatewayEvent.Type))
            {
                eventHandlers[gatewayEvent.Type](gatewayEvent);
            }
        }
        
        private void OnHelloReceived(GatewayEvent gatewayEvent)
        {
            IdentifySelfToGateway();
            BeginHeartbeat(gatewayEvent.GetData<Hello>().HeartbeatInterval);
        }

        private async void IdentifySelfToGateway()
        {
            var authToken = await _authenticator.GetToken();

            var properties = new Properties
            {
                OS = "DISCORD-UWP",
                Device = "DISCORD-UWP",
                Browser = "DISCORD-UWP",
                Referrer = "",
                ReferringDomain = ""
            };

            var identity = new Identify
            {
                Token = authToken,
                Properties = properties,
                LargeThreshold = 50
            };

            var identifyEvent = new GatewayEvent
            {
                Type = "IDENTIFY",
                Operation = OperationCode.Identify.ToInt(),
                Data = identity
            };

            await _webMessageSocket.SendJsonObjectAsync(identifyEvent);
        }

        private void OnReady(GatewayEvent gatewayEvent)
        {
            var readyEvent = new GatewayEventArgs<Ready>(gatewayEvent.GetData<Ready>());

            Ready?.Invoke(this, readyEvent);
        }

        private void OnMessageCreated(GatewayEvent gatewayEvent)
        {
            var messageCreatedEvent = new GatewayEventArgs<Message>(gatewayEvent.GetData<Message>());

            MessageCreated?.Invoke(this, messageCreatedEvent);
        }

        private void OnMessageUpdated(GatewayEvent gatewayEvent)
        {
            var messageUpdatedEvent = new GatewayEventArgs<Message>(gatewayEvent.GetData<Message>());

            MessageUpdated?.Invoke(this, messageUpdatedEvent);
        }

        private void OnMessageDeleted(GatewayEvent gatewayEvent)
        {
            var messageDeletedEvent = new GatewayEventArgs<MessageDelete>(gatewayEvent.GetData<MessageDelete>());

            MessageDeleted?.Invoke(this, messageDeletedEvent);
        }

        private async void BeginHeartbeat(int interval)
        {
            while (true)
            {
                await Task.Delay(interval);
                await SendHeartbeat();
            }
        }

        private async Task SendHeartbeat()
        {
            var heartbeatEvent = new GatewayEvent
            {
                Operation = OperationCode.Heartbeat.ToInt(),
                Data = lastGatewayEvent.SequenceNumber ?? 0
            };

            await _webMessageSocket.SendJsonObjectAsync(heartbeatEvent);
        }

    }
}
