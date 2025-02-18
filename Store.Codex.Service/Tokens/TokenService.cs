using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Store.Codex.Core.Entities.Identity;
using Store.Codex.Core.Services.Contract;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Store.Codex.Service.Tokens
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager)
        {
            // 1. Header (algo,Type)
            // 2. Payload (claims)
            // 3. Signature 

            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName,user.DisplayName),
                new Claim(ClaimTypes.MobilePhone,user.PhoneNumber),
                //new Claim(ClaimTypes.StreetAddress,user.Address.Street),
            };

            var userRoles = await userManager.GetRolesAsync(user);

            foreach(var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }


            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])); // here signingCredentials take the key as type of security key and we have to convert the key to array byte here

            var token = new JwtSecurityToken
                (
                issuer: _configuration["Jwt:Issuer"], // issuer is the person who made the token .. my web api (like value of baseurl)
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddDays(double.Parse(_configuration["Jwt:DurationInDays"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
