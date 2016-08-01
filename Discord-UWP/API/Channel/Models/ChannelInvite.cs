﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_UWP.API.Channel.Models
{
    public struct ChannelInvite
    {
        [JsonProperty("max_age")]
        public int MaxAge { get; set; }
        [JsonProperty("max_uses")]
        public int MaxUses { get; set; }
        [JsonProperty("temporary")]
        public bool Temporary { get; set; }
    }
}
