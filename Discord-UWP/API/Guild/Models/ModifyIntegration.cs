﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_UWP.API.Guild.Models
{
    public struct ModifyIntegration
    {
        [JsonProperty("expire_behavior")]
        public int ExpireBehaviour { get; set; }
        [JsonProperty("expire_grace_period")]
        public int ExpireGracePeriod { get; set; }
        [JsonProperty("enable_emoticons")]
        public bool EnableEmoticions { get; set; }
    }
}
