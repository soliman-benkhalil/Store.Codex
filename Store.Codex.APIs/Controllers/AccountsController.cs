using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.Codex.APIs.Errors;
using Store.Codex.APIs.Extensions;

//using Store.Codex.APIs.Extensions;
using Store.Codex.Core.Dtos.Auth;
using Store.Codex.Core.Entities.Identity;
using Store.Codex.Core.Services.Contract;
using Store.Codex.Service.Tokens;
using System.Security.Claims;

namespace Store.Codex.APIs.Controllers
{

    public class AccountsController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountsController(IUserService userService , UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper)
        {
            _userService = userService;
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("login")] // Post : /api/Accounts/login
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userService.LoginAsync(loginDto);
            if(user is null)
            {
                return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));
            }

            return Ok(user);
        }


        [HttpPost("register")] // Post : /api/Accounts/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var user = await _userService.RegisterAsyn(registerDto);
            if (user is null)
            {
                return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest,"Invalid SignUp"));
            }

            return Ok(user);
        }

        [HttpGet("GetCurrentUser")] // GET : /api/Accounts/GetCurrentUser
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (userEmail is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            var user = await _userManager.FindByEmailAsync(userEmail);

            if( user is null ) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(new { DisplayName = user.DisplayName });

        }

        [HttpGet("Address")] // GET : /api/Accounts/Address
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUserAddress()
        {
            var user = await _userManager.FindByEmailWithAddressAsync(User);

            if (user is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(_mapper.Map<AddressDto>(user.Address));

        }


        [HttpPost("UpdateAddress")] // PUT : /api/Accounts/UpdateAddress
        [Authorize]
        public async Task<ActionResult<UserDto>> UpdateCurrentUserAddress(AddressDto addressDto)
        {
            var user = await _userManager.FindByEmailWithAddressAsync(User);

            if (user is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            user.Address.FName = addressDto.FName;
            user.Address.LName = addressDto.LName;
            user.Address.City = addressDto.City;
            user.Address.Country = addressDto.Country;
            user.Address.Street = addressDto.Street;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(_mapper.Map<AddressDto>(user.Address));

        }
    }
}
