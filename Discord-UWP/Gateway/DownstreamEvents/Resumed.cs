﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_UWP.Gateway.DownstreamEvents
{
    public struct Resumed
    {
        [JsonProperty("_trace")]
        public IEnumerable<string> Trace { get; set; }
    }
}
