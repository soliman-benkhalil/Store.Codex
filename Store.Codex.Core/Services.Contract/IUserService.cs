using Store.Codex.Core.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Codex.Core.Services.Contract
{
    public interface IUserService
    {
        Task<UserDto>LoginAsync(LoginDto loginDto);
        Task<UserDto>RegisterAsyn(RegisterDto registerDto);
        Task<bool> CheckEmailExistsAsync(string email);
    }
}
