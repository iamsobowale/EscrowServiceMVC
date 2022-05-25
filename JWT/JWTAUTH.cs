using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EscrowService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EscrowService.JWT
{
    public class JWTAUTH:IJWTAUTH
    {
        private readonly IHttpContextAccessor _context;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher<User> _passwordHasher;

        public JWTAUTH(IHttpContextAccessor context, IConfiguration configuration, IPasswordHasher<User> passwordHasher)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration = configuration ?? throw new ArgumentException(nameof(configuration));
            _passwordHasher = passwordHasher ?? throw new ArgumentException(nameof(passwordHasher));
        }

        public string GetUserIdentity()
        {
            return _context.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
        }

        public string GenerateToken(User user, IEnumerable<string> roles)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JwtTokenSettings:TokenKey")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);
            IList<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };
            claims.Add(new Claim(ClaimTypes.Role, user.Role.ToString()));
            var token = new JwtSecurityToken(_configuration.GetValue<string>("JwtTokenSettings:TokenIssuer"),
                _configuration.GetValue<string>("JwtTokenSettings:TokenIssuer"),
                claims,
                DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration.GetValue<string>("JwtTokenSettings:TokenExpiryPeriod"))),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public JwtSecurityToken GetClaims(string token)
        {
            throw new System.NotImplementedException();
        }

        public string GetClaimValue(string type)
        {
            throw new System.NotImplementedException();
        }

        public string GenerateSalt()
        {
            throw new System.NotImplementedException();
        }

        public string GetPasswordHash(string password, string salt = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> FindByNameAsync(string userName)
        {
            throw new System.NotImplementedException();
        }
    }
}