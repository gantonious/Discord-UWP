﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_UWP.Gateway.DownstreamEvents
{
    public struct Hello
    {
        [JsonProperty("heartbeat_interval")]
        public int HeartbeatInterval { get; set; }
        [JsonProperty("_trace")]
        public IEnumerable<string> Trace { get; set; }
    }
}
