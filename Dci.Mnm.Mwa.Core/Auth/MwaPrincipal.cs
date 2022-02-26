using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Dci.Mnm.Mwa.Core
{
    public class MwaPrincipal : ClaimsPrincipal
    {
        public MwaPrincipal(IIdentity identity) : base(identity)
        {

        }
    }
}
