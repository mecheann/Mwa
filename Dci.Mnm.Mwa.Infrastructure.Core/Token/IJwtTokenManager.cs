using Dci.Mnm.Mwa.Core;
using Dci.Mnm.Mwa.Domain;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Dci.Mnm.Mwa.Infrastructure.Core
{
    public interface IJwtTokenManager
    {
        Task<ClaimsIdentity> CreateSimpleIdentityAsync(User user, List<Role> roles, params Claim[] additionalClaims);
        Task<SecurityToken> CreateTokenFromPrincipalAsync(ClaimsPrincipal principal);
        Task<TokenResponse> GenerateTokenAsync(User user, List<Role> roles, params Claim[] additionalClaims);
        Task<MwaPrincipal> GetPrincipalFromTokenString(string token);
        string GetTokenString(SecurityToken token);
    }
}









