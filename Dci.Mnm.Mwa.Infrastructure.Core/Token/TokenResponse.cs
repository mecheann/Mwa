using Dci.Mnm.Mwa.Core;
using Microsoft.IdentityModel.Tokens;

namespace Dci.Mnm.Mwa.Infrastructure.Core
{
    public class TokenResponse
    {
        public SecurityToken Token { get; set; }
        public string TokenString { get; set; }
        public MwaPrincipal Principal { get; set; }
    }
}









