﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Refit;
using Discord_UWP.API.Channel.Models;
using Discord_UWP.SharedModels;

namespace Discord_UWP.API.Channel
{
    public interface IChannelService
    {
        [Get("/channels/{channelId}")]
        Task GetChannel([AliasAs("channelId")] string channelId);

        [Put("/channels/{channelId}")]
        Task ModifyChannel([AliasAs("channelId")] string channelId, [Body] ModifyChannel modifyChannel);

        [Delete("/channels/{channelId}")]
        Task DeleteChannel([AliasAs("channelId")] string channelId);

        [Get("/channels/{channelId}/messages")]
        Task DeleteChannelMessages([AliasAs("channelId")] string channelId);

        [Get("/channels/{channelId}/messages/{messageId)")]
        Task<Message> GetChannelMessage([AliasAs("channelId")] string channelId, [AliasAs("messageId")] string messageId);

        [Get("/channels/{channelId}/messages")]
        Task<IEnumerable<Message>> GetChannelMessages([AliasAs("channelId")] string channelId);

        [Post("/channels/{channelId}/messages")]
        Task<Message> CreateMessage([AliasAs("channelId")] string channelId, [Body] MessageUpsert message);

        // Set up Properly
        [Post("/channels/{channelId}/messages")]
        Task<Message> UploadFile([AliasAs("channelId")] string channelId);

        [Patch("/channels/{channelId}/messages/{messageId}")]
        Task<Message> EditMessage([AliasAs("channelId")] string channelId, [AliasAs("messageId")] string messageId, [Body] EditMessage editMessage);

        [Delete("/channels/{channelId}/messages/{messageId}")]
        Task DeleteMessage([AliasAs("channelId")] string channelId, [AliasAs("messageId")] string messageId);

        [Post("/channels/{channelId}/messages/bulk_delete")]
        Task BulkDeleteMessages([AliasAs("channelId")] string channelId, [Body] BulkDelete messages);

        [Post("/channels/{channelId}/messages/{messageId}/ack")]
        Task AckMessage([AliasAs("channelId")] string channelId, [AliasAs("messageId")] string messageId);

        [Put("/channels/{channelId}/permissions/{overwriteId}")]
        Task EditChannelPermissions([AliasAs("channelId")] string channelId, [AliasAs("overwriteId")] string overwriteId, [Body] EditChannel editChannel);

        [Get("/channels/{channelId}/invites")]
        Task<IEnumerable<SharedModels.Invite>> GetChannelInvites([AliasAs("channelId")] string channelId);

        [Post("/channels/{channelId}/invites")]
        Task<SharedModels.Invite> CreateChannelInvite([AliasAs("channelId")] string channelId);

        [Delete("/channels/{channelId}/permissions/{overwriteId}")]
        Task DeleteChannelPermission([AliasAs("channelId")] string channelId, [AliasAs("overwriteId")] string overwriteId);

        [Post("/channels/{channelId}/typing")]
        Task TriggerTypingIndicator([AliasAs("channelId")] string channelId);

        [Get("/channels/{channelId}/pins")]
        Task<IEnumerable<Message>> GetPinnedMessages([AliasAs("channelId")] string channelId);

        [Put("/channels/{channelId}/pins/{messageId}")]
        Task AddPinnedChannelMessage([AliasAs("channelId")] string channelId, [AliasAs("messageId")] string messageId);

        [Delete("/channels/{channelId}/pins/{messageId}")]
        Task DeletePinnedChannelMessage([AliasAs("channelId")] string channelId, [AliasAs("messageId")] string messageId);
    }
}
