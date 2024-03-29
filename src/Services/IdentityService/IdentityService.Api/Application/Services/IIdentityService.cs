﻿
using IdentityService.Api.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Api.Application.Services
{
    public interface IIdentityService
    {
        Task<LoginResponseDto> Login(LoginRequestDto requestModel);
    }
}
