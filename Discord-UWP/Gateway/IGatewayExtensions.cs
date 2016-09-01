using Discord_UWP.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_UWP.Gateway
{
    public static class IGatewayExtensions
    {
        public static IObservable<Message> MessageCreated(this IGateway gateway)
        {
            return Observable.FromEventPattern<GatewayEventArgs<Message>>(e => gateway.MessageCreated += e, e => gateway.MessageCreated -= e)
                             .Select(e => e.EventArgs.EventData);
        }
    }
}
