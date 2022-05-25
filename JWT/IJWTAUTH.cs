using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using EscrowService.Models;

namespace EscrowService.JWT
{
    public interface IJWTAUTH
    {
        string GetUserIdentity();

        string GenerateToken(User user, IEnumerable<string> roles);

        JwtSecurityToken GetClaims(string token);

        string GetClaimValue(string type);

        string GenerateSalt();

        public string GetPasswordHash(string password, string salt = null);
        Task<User> FindByNameAsync(string userName);
    }
}