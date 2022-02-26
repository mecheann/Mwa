using Dci.Mnm.Mwa.Core;
using Dci.Mnm.Mwa.Domain;
using Dci.Mnm.Mwa.Infrastructure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace sa.jsix.Infrastructure.AuthToken
{
    public class JwtTokenManager : IJwtTokenManager
    {
        AppConfig config;
        readonly UserManager<User> userManager;
        readonly RoleManager<Role> roleManager;

        public JwtTokenManager(AppConfig config, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            this.config = config;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<ClaimsIdentity> CreateSimpleIdentityAsync(User user, List<Role> roles, params Claim[] additionalClaims)
        {
            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(AppConst.ClaimTypes.Name, user.UserName));
            identity.AddClaim(new Claim(AppConst.ClaimTypes.Email, user.Email));
            if (!string.IsNullOrEmpty(user.PhoneNumber))
            {
                identity.AddClaim(new Claim(AppConst.ClaimTypes.MobilePhone, user.PhoneNumber));
            }
            if (!string.IsNullOrEmpty(user.FullName))
            {
                identity.AddClaim(new Claim(AppConst.ClaimTypes.GivenName, user.FullName));
            }
            else
            {
                identity.AddClaim(new Claim(AppConst.ClaimTypes.GivenName, user.FirstName));
                identity.AddClaim(new Claim(AppConst.ClaimTypes.Surname, user.LastName));
            }
            identity.AddClaim(new Claim(AppConst.ClaimTypes.UserId, user.Id.ToString()));
            identity.AddClaim(new Claim(AppConst.ClaimTypes.CurrentSessionId, Dci.Mnm.Mwa.Core.Utility.CreateNewId().ToShortGuid()));

            foreach (var role in roles)
            {
                identity.AddClaim(new Claim(AppConst.ClaimTypes.Role, role.Name));

                var roleClaims = await roleManager.GetClaimsAsync(role);

                //Add operation claims to the user
                identity.AddClaims(roleClaims.Where(x => x.Type == AppConst.ClaimTypes.Operation));
            }

            var userClaims = (await userManager.GetClaimsAsync(user)).ToList();

            if (additionalClaims != null && additionalClaims.Length > 0)
            {
                userClaims.AddRange(additionalClaims);
            }

            identity.AddClaims(userClaims);
            return identity;
        }

        public async Task<SecurityToken> CreateTokenFromPrincipalAsync(ClaimsPrincipal principal)
        {
            var jwtConfig = config.Security.Token;

            // Removing jwtClaims if they are already added
            var jwtClaimTypes = typeof(JwtRegisteredClaimNames).GetFields().Where(x => x.FieldType.Name.Contains("String"))
            .Select(x => x.GetRawConstantValue() as string).ToList();
            jwtClaimTypes.Add(AppConst.ClaimTypes.NameIdentifier);
            var filteredClaims = principal.Claims.Where(x => !jwtClaimTypes.Contains(x.Type)).ToList();

            //Add JWT Claims
            var jwtClaims = new List<Claim>();
            jwtClaims.Add(new Claim(JwtRegisteredClaimNames.Sub, principal.UserName()));
            jwtClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, ShortGuid.NewGuid().Value));
            jwtClaims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()));

            var allClaims = jwtClaims.Union(filteredClaims);
            var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.Default.GetBytes(jwtConfig.SecretKey));
            SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: jwtConfig.Issuer,
                audience: jwtConfig.Audience,
                claims: allClaims,
                expires: DateTime.Now.AddMinutes(jwtConfig.TokenLifeTimeInMinutes),
                notBefore: DateTime.Now,
                signingCredentials: signingCredentials);

            return await Task.FromResult(jwt);
        }

        public string GetTokenString(SecurityToken token)
        {
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<TokenResponse> GenerateTokenAsync(User user, List<Role> roles, params Claim[] additionalClaims)
        {
            var identity = await CreateSimpleIdentityAsync(user, roles, additionalClaims);
            var principal = new MwaPrincipal(identity);

            var token = await CreateTokenFromPrincipalAsync(principal);
            var tokenString = GetTokenString(token);
            return new TokenResponse
            {
                Token = token,
                TokenString = tokenString,
                Principal = principal
            };
        }

        public Task<MwaPrincipal> GetPrincipalFromTokenString(string token)
        {
            var jwtConfig = config.Security.Token;
            var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.Default.GetBytes(jwtConfig.SecretKey));
            var principal = new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateLifetime = false // we check expired tokens here
            }, out var securityToken);


            if (!(securityToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return Task.FromResult(new MwaPrincipal(principal.Identities.FirstOrDefault()));
        }

    }
}









