using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CarbCounter.Core.Common.Interfaces;

public interface IJsonWebTokenService
{
    public JwtSecurityToken GetToken(List<Claim> authClaims);
}