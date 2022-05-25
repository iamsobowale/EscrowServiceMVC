using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EscrowService.DTO;
using EscrowService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EscrowService.JWT
{
    public class JWTAUTH:IJWTAUTH
    {
        private readonly string _key;

        public JWTAUTH(string key)
        {
            _key = key;
        }

        // public string GetUserIdentity()
        // {
        //     return _context.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
        // }

        public string GenerateToken(UserDto user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenKey = Encoding.ASCII.GetBytes(_key);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                IssuedAt = DateTime.Now,
                Expires = DateTime.Now.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        public JwtSecurityToken GetClaims(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                
                var handler = new JwtSecurityTokenHandler();

                var decodedToken = handler.ReadToken(token) as JwtSecurityToken;

                return decodedToken;
            }
            return null;
        }

        public string GetClaimValue(string type)
        {
            throw new System.NotImplementedException();
        }

        public string GenerateSalt()
        {
            throw new System.NotImplementedException();
        }
        
        public Task<User> FindByNameAsync(string userName)
        {
            throw new System.NotImplementedException();
        }
    }
}