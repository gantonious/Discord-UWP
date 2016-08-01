﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_UWP.SharedModels
{
    public struct Emoji
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("roles")]
        public IEnumerable<string> Roles { get; set; }
        [JsonProperty("require_colons")]
        public bool RequireColons { get; set; }
        [JsonProperty("managed")]
        public bool Managed { get; set; }
    }
}
