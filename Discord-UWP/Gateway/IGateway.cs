using Discord_UWP.Gateway.DownstreamEvents;
using Discord_UWP.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_UWP.Gateway
{
    public interface IGateway
    {
        event EventHandler<GatewayEventArgs<Ready>> Ready;
        event EventHandler<GatewayEventArgs<Resumed>> Resumed;

        event EventHandler<GatewayEventArgs<GuildChannel>> GuildChannelCreated;
        event EventHandler<GatewayEventArgs<DirectMessageChannel>> DirectMessageChannelCreated;

        event EventHandler<GatewayEventArgs<Message>> MessageCreated;
        event EventHandler<GatewayEventArgs<Message>> MessageUpdated;
        event EventHandler<GatewayEventArgs<MessageDelete>> MessageDeleted;

        Task ConnectAsync();
        Task ResumeAsync();
    }
}
