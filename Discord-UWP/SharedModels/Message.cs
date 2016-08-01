﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_UWP.SharedModels
{
    public struct Message
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("channel_id")]
        public string ChannelId { get; set; }
        [JsonProperty("author")]
        public User User { get; set; }
        [JsonProperty("content")]
        public string String { get; set; }
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }
        [JsonProperty("edited_timestamp")]
        public DateTime? EditedTimestamp { get; set; }
        [JsonProperty("tts")]
        public bool TTS { get; set; }
        [JsonProperty("mention_everyone")]
        public bool MentionEveryone { get; set; }
        [JsonProperty("mentions")]
        public IEnumerable<User> Mentions { get; set; }
        [JsonProperty("mention_roles")]
        public IEnumerable<string> MentionRoles { get; set; }
        [JsonProperty("attachments")]
        public IEnumerable<Attachment> Attachments { get; set; }
        [JsonProperty("embeds")]
        public IEnumerable<Embed> Embeds { get; set; }
        [JsonProperty("nonce")]
        public int? Nonce { get; set; }
        [JsonProperty("pinned")]
        public bool Pinned { get; set; }
    }
}
