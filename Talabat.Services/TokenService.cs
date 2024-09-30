using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;

namespace Talabat.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> CreateToken(AppUser User, UserManager<AppUser> userManager)
        {
            var AuthClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName,User.DisplayName),
                new Claim(ClaimTypes.Email,User.Email)
            };
            var RolesAsync = await userManager.GetRolesAsync(User);
            foreach (var role in RolesAsync)
            {
                AuthClaims.Add(new Claim(ClaimTypes.Role,role));
            }
           var EncodedKey= Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
            var Key = new SymmetricSecurityKey(EncodedKey);
            var Token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"], 
                audience: _configuration["JWT:ValidAudience"],
                claims:AuthClaims,
                expires:DateTime.Now.AddDays(double.Parse(_configuration["JWT:DTime"])),
                signingCredentials:new SigningCredentials(Key,SecurityAlgorithms.HmacSha256Signature));
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}
