using BSNTNext.Application.Common;
using BSNTNext.Application.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace BSNTNext.Application.Interfaces.Services
{
    public interface IAuthServices
    {
        Task<Result> RegisterAsync(RegisterDto dto);
        Task<Result> LoginAsync(LoginDto dto);
        Task<Result> LogoutAsync();
    }
}
