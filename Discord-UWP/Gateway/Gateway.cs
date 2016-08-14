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
        public T EventData { get; }

        public GatewayEventArgs(T eventData)
        {
            EventData = eventData;
        }
    }

    public class Gateway : IGateway
    {
        private delegate void GatewayEventHandler(GatewayEvent gatewayEvent);

        private IDictionary<int, GatewayEventHandler> operationHandlers;
        private IDictionary<string, GatewayEventHandler> eventHandlers;

        private Ready? lastReady;
        private GatewayEvent? lastGatewayEvent;

        private readonly IWebMessageSocket _webMessageSocket;
        private readonly IAuthenticator _authenticator;
        private readonly GatewayConfig _gatewayConfig;

        public event EventHandler<GatewayEventArgs<Ready>> Ready;
        public event EventHandler<GatewayEventArgs<Resumed>> Resumed;

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

        private IDictionary<int, GatewayEventHandler> GetOperationHandlers()
        {
            return new Dictionary<int, GatewayEventHandler>
            {
                { OperationCode.Hello.ToInt(), OnHelloReceived },
                { OperationCode.Resume.ToInt(), OnResumeReceived }
            };
        }

        private IDictionary<string, GatewayEventHandler> GetEventHandlers()
        {
            return new Dictionary<string, GatewayEventHandler>
            {
                { EventNames.READY, OnReady },
                { EventNames.MESSAGE_CREATED, OnMessageCreated },
                { EventNames.MESSAGE_UPDATED, OnMessageUpdated },
                { EventNames.MESSAGE_DELETED, OnMessageDeleted }
            };
        }

        private void PrepareSocket()
        {
            _webMessageSocket.MessageReceived += OnSocketMessageReceived;
        }

        public async Task ConnectAsync()
        {
            await _webMessageSocket.ConnectAsync(_gatewayConfig.GetFullGatewayUrl("json", "6"));
        }

        // TODO: good chance the socket will be disposed when attempting to resume so yah
        public async Task ResumeAsync()
        {
            var token = await _authenticator.GetToken();

            var resume = new GatewayResume
            {
                Token = token,
                SessionId = lastReady?.SessionId,
                LastSequenceNumberReceived = lastGatewayEvent?.SequenceNumber.Value ?? 0
            };

            await _webMessageSocket.SendJsonObjectAsync(token);
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
            BeginHeartbeatAsync(gatewayEvent.GetData<Hello>().HeartbeatInterval);
        }

        private async void IdentifySelfToGateway()
        {
            var identifyEvent = new GatewayEvent
            {
                Type = EventNames.IDENTIFY,
                Operation = OperationCode.Identify.ToInt(),
                Data = await GetIdentityAsync()
            };

            await _webMessageSocket.SendJsonObjectAsync(identifyEvent);
        }

        private async Task<Identify> GetIdentityAsync()
        {
            return new Identify
            {
                Token = await _authenticator.GetToken(),
                Properties = GetClientProperties(),
                LargeThreshold = 50
            };
        }

        // TODO: move propeties to config
        private Properties GetClientProperties()
        {
            return new Properties
            {
                OS = "DISCORD-UWP",
                Device = "DISCORD-UWP",
                Browser = "DISCORD-UWP",
                Referrer = "",
                ReferringDomain = ""
            };
        }

        private void OnResumeReceived(GatewayEvent gatewayEvent)
        {
            FireEventOnDelegate(gatewayEvent, Resumed);
        }

        private void OnReady(GatewayEvent gatewayEvent)
        {
            var ready = gatewayEvent.GetData<Ready>();
            lastReady = ready;

            FireEventOnDelegate(gatewayEvent, Ready);
        }

        private void OnMessageCreated(GatewayEvent gatewayEvent)
        {
            FireEventOnDelegate(gatewayEvent, MessageCreated);
        }

        private void OnMessageUpdated(GatewayEvent gatewayEvent)
        {
            FireEventOnDelegate(gatewayEvent, MessageUpdated);
        }

        private void OnMessageDeleted(GatewayEvent gatewayEvent)
        {
            FireEventOnDelegate(gatewayEvent, MessageDeleted);
        }

        private void FireEventOnDelegate<TEventData>(GatewayEvent gatewayEvent, EventHandler<GatewayEventArgs<TEventData>> eventHandler)
        {
            var eventArgs = new GatewayEventArgs<TEventData>(gatewayEvent.GetData<TEventData>());
            eventHandler?.Invoke(this, eventArgs);
        }

        private async void BeginHeartbeatAsync(int interval)
        {
            while (true)
            {
                await Task.Delay(interval);
                await SendHeartbeatAsync();
            }
        }

        private async Task SendHeartbeatAsync()
        {
            var heartbeatEvent = new GatewayEvent
            {
                Operation = OperationCode.Heartbeat.ToInt(),
                Data = lastGatewayEvent?.SequenceNumber ?? 0
            };

            await _webMessageSocket.SendJsonObjectAsync(heartbeatEvent);
        }

    }
}
