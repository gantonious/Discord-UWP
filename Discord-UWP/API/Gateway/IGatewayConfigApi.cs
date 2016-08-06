﻿using Discord_UWP.SharedModels;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_UWP.API.Gateway
{
    public interface IGatewayConfigApi
    {
        [Get("/gateway")]
        Task<GatewayConfig> GetGatewayConfig();
    }
}
