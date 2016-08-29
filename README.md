Discord-UWP [![Build status](https://ci.appveyor.com/api/projects/status/2ansr6w4mb8b1gtm/branch/master?svg=true)](https://ci.appveyor.com/project/gantonious/discord-uwp/branch/master)
===

A Discord wrapper in C# compatible with UWP.

Basic Api Services:
---

Api services that do not require authentication can be accessed by doing:
```csharp
BasicRestFactory basicRestFactory = new BasicRestFactory();

ILoginService loginService = basicRestFactory.GetLoginService();
IGatewayService gatewayService = basicRestFactory.GetGatewayService();
```

Authentication
---

Note: This is not the planned implmentation of the Authentication layer, this is just a bandaid solution till time is found to work on it.
```csharp
LoginRequest loginRequest = new LoginRequest
{
    Email = "EMAIL",
    Password = "PASSWORD"
};

LoginResult loginResult = await loginService.Login(loginRequest);

IAuthenticator discordAuthenticator = new DiscordAuthenticator(loginResult.Token);
```

Authenticated Api Services:
---

```csharp
AuthenticatedRestFactory authenticatedRestFactory = new AuthenticatedRestFactory(authenticator);

IUserService userService = authenticatedRestFactory.GetUserService();
IGuildService guildService = authenticatedRestFactory.GetGuildService(); 
IChannelService channelService = authenticatedRestFactory.GetChannelService();
IInviteService inviteService = authenticatedRestFactory.GetInviteService();
```

Gateway
---

To create a Gateway instance, you need a `GatewayConfig` and an `IAuthenticator`.

```csharp
GatewayConfig config = await gatewayService.GetGatewayConfigAsync();

Gateway gateway = new Gateway(config, discordAuthenticator);
```

To connect just call:

```csharp
await gateway.ConnectAsync();
```

Then events can be listend to as follows:

```csharp
gateway.MessageCreated += (o, e) => 
{ 
    Message message = e.EventPayload;
    // Handle message
};
```