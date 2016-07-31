﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_UWP.Authentication
{
    public interface IAuthenticator
    {
        Task<string> GetToken();
    }
}
