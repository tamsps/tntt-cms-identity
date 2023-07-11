using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TNTT.CMS.Models.Entities;
using TNTT.Identity.Models;

namespace TNTT.Identity.Services
{
    public class JwtTokenService
    {
        private readonly List<User> _users = new()
    {
        new("admin", "admin", "Administrator", new[] { "shoes.read" }),
        new("user01", "user01", "User", new[] { "shoes.read" })
    };


        public Models.AuthenticationToken? GenerateAuthToken(LoginModel loginModel)
        {
            var user = _users.FirstOrDefault(u => u.Username == loginModel.Username && u.Password == loginModel.Password);

            if (user is null)
            {
                return null;
            }

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtExtensions.SecurityKey));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var expirationTimeStamp = DateTime.Now.AddMinutes(5);

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Name, user.Username),
            new Claim("role", user.Role),
            new Claim("scope", string.Join(" ", user.Scopes))
        };

            var tokenOptions = new JwtSecurityToken(
                issuer: "https://localhost:5002",
                claims: claims,
                expires: expirationTimeStamp,
                signingCredentials: signingCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return new Models.AuthenticationToken(tokenString, (int)expirationTimeStamp.Subtract(DateTime.Now).TotalSeconds);
        }
    }
}
