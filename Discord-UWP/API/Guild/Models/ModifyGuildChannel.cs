﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_UWP.API.Guild.Models
{
    public struct ModifyGuildChannel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("position")]
        public int Position { get; set; }
    }
}
