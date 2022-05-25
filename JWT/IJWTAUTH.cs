using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using EscrowService.DTO;
using EscrowService.Models;

namespace EscrowService.JWT
{
    public interface IJWTAUTH
    {
        string GenerateToken(UserDto user);

        JwtSecurityToken GetClaims(string token);

        string GetClaimValue(string type);

        string GenerateSalt();
        Task<User> FindByNameAsync(string userName);
    }
}