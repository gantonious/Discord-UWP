﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_UWP.SharedModels
{
    public struct GuildEmbed
    {
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }
        [JsonProperty("channel_id")]
        public string ChannelId { get; set; }
    }
}
