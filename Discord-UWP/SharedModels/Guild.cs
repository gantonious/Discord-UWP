﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_UWP.SharedModels
{
    public struct Guild
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("icon")]
        public string Icon { get; set; }
        [JsonProperty("splash")]
        public string Splash { get; set; }
        [JsonProperty("owner_id")]
        public string OwnerId { get; set; }
        [JsonProperty("region")]
        public string Region { get; set; }
        [JsonProperty("afk_channel_id")]
        public string AfkChannelId { get; set; }
        [JsonProperty("afk_timeout")]
        public int AfkTimeout { get; set; }
        [JsonProperty("embed_enabled")]
        public bool EmbedEnabled { get; set; }
        [JsonProperty("embed_channel_id")]
        public string EmbedChannelId { get; set; }
        [JsonProperty("verification_level")]
        public int VerificationLevel { get; set; }
        [JsonProperty("voice_states")]
        public IEnumerable<VoiceState> VoiceStates { get; set; }
        [JsonProperty("roles")]
        public IEnumerable<Role> Roles { get; set; }
        [JsonProperty("emojis")]
        public IEnumerable<Emoji> Emoji { get; set; }
        [JsonProperty("features")]
        public IEnumerable<string> Features { get; set; }
        [JsonProperty("unavailable")]
        public bool Unavailable { get; set; }
    }
}
