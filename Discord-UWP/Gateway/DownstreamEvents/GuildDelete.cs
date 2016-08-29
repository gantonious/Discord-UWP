using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_UWP.Gateway.DownstreamEvents
{
    public struct GuildDelete
    {
        [JsonProperty("id")]
        public string MessageId { get; set; }
        [JsonProperty("unavailable")]
        public bool Unavailable { get; set; }
    }
}
