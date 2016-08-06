using Discord_UWP.API.User;
using Discord_UWP.API.Channel;
using Discord_UWP.Authentication;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Discord_UWP.API.Gateway;

namespace Discord_UWP.API
{
    public class RestFactory
    {
        private readonly IAuthenticator _authenticator;
        private readonly DiscordApiConfiguration _apiConfig;
        
        public RestFactory(DiscordApiConfiguration config, IAuthenticator authenticator)
        {
            _apiConfig = config;
            _authenticator = authenticator;
        }

        public IUserApi GetUserApi()
        {
            return RestService.For<IUserApi>(GetAuthenticatingHttpClient());
        }

        public IChannelApi GetChannelApi()
        {
            return RestService.For<IChannelApi>(GetAuthenticatingHttpClient());
        }

        public IGatewayConfigApi GetGatewayConfigApi()
        {
            return RestService.For<IGatewayConfigApi>(GetBasicHttpClient());
        }

        private HttpClient GetBasicHttpClient()
        {
            return new HttpClient()
            {
                BaseAddress = new Uri(_apiConfig.BaseUrl)
            };
        }

        private HttpClient GetAuthenticatingHttpClient()
        {
            return new HttpClient(GetAuthenticationHandler())
            {
                BaseAddress = new Uri(_apiConfig.BaseUrl)
            };
        }

        private HttpClientHandler GetAuthenticationHandler()
        {
            return new AuthenticatingHttpClientHandler(_authenticator);
        }
    }
}
