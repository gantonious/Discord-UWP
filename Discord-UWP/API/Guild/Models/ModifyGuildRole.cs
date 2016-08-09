﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_UWP.API.Guild.Models
{
    public struct ModifyGuildRole
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("permission")]
        public int Permissions { get; set; }
        [JsonProperty("position")]
        public int Position { get; set; }
        [JsonProperty("color")]
        public int Color { get; set; }
        [JsonProperty("hoist")]
        public bool Hoist { get; set; }
    }
}
