﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_UWP.API.Channel.Models
{
    public struct EditMessage
    {
        [JsonProperty("content")]
        public string Content { get; set; }
    }
}
