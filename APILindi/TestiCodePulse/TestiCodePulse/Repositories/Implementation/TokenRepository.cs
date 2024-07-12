using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TestiCodePulse.Repositories.Interface;
using static System.Net.WebRequestMethods;

namespace TestiCodePulse.Repositories.Implementation
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration configuration;

        public TokenRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string CreateJwtToken(IdentityUser user, List<string> roles)
        {
            // Create Claims from Roles

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));


            // JWT Security params

            

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            // use key to define credentials

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(

                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
                );


            // Return Token

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
