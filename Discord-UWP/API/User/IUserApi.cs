﻿using Discord_UWP.API.User.Models;
using Discord_UWP.SharedModels;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_UWP.API.User
{
    public interface IUserApi
    {
        [Get("/users")]
        Task<IEnumerable<SharedModels.User>> GetUsers([AliasAs("q")] string usernameQuery, [AliasAs("limit")] int limit);

        [Get("/users/@me")]
        Task<SharedModels.User> GetCurrentUser();

        [Patch("/users/@me")]
        Task<SharedModels.User> ModifyCurrentUser([Body] ModifyUser modifyUser);

        [Get("/users/{userId}")]
        Task<SharedModels.User> GetUser([AliasAs("userId")] string userId);

        [Get("/users/@me/guilds")]
        Task<IEnumerable<UserGuild>> GetCurrentUserGuilds();

        [Delete("/users/@me/guilds/{guildId}")]
        Task LeaveGuild([AliasAs("guildId")] string guildId);

        [Get("/users/@me/channels")]
        Task<IEnumerable<DirectMessageChannel>> GetCurrentUserDirectMessageChannels();

        [Post("/users/@me/channels")]
        Task<DirectMessageChannel> CreateDirectMessageChannelForCurrentUser();

        [Get("/users/@me/connections")]
        Task<IEnumerable<Connection>> GetCurrentUserConnections();
    }
}
