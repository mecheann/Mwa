using System;
using System.Security.Claims;
using System.Security.Principal;


namespace Dci.Mnm.Mwa.Core
{
    public static class PrincipalExtensions
    {

        public static Boolean Cannot(this IPrincipal principal, String action)
        {
            return !principal.Can(action);
        }

        public static Boolean Can(this IPrincipal principal, String action)
        {
            var cliamsPrincipal = principal as ClaimsPrincipal;

            if (cliamsPrincipal != null)
            {
                return cliamsPrincipal.HasClaim(AppConst.ClaimTypes.Operation, action);
            }
            else
            {
                return false;
            }
        }

        public static Guid? UserId(this IPrincipal principal)
        {
            var cliamsPrincipal = principal as ClaimsPrincipal;

            if (cliamsPrincipal != null)
            {
                Guid temp;
                if (Guid.TryParse(cliamsPrincipal.FindFirst(AppConst.ClaimTypes.UserId)?.Value, out temp))
                {
                    return temp;
                }
                else
                {
                    return null;
                }

            }
            else
            {
                throw new ArgumentException("Claims Principal not Set!");
            }
        }

        public static string UserName(this IPrincipal principal)
        {
            var cliamsPrincipal = principal as ClaimsPrincipal;

            if (cliamsPrincipal != null)
            {
                return cliamsPrincipal.FindFirst(AppConst.ClaimTypes.Name)?.Value;
            }
            else
            {
                throw new ArgumentException("Claims Principal not Set!");
            }
        }

        public static String NetworkId(this IPrincipal principal)
        {
            var cliamsPrincipal = principal as ClaimsPrincipal;

            if (cliamsPrincipal != null)
            {
                return cliamsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            }
            else
            {
                throw new ArgumentException("Claims Principal not Set!");
            }
        }

        public static String FullName(this IPrincipal principal)
        {
            var cliamsPrincipal = principal as ClaimsPrincipal;

            if (cliamsPrincipal != null)
            {
                return $" {cliamsPrincipal.FindFirst(ClaimTypes.GivenName)?.Value} {cliamsPrincipal.FindFirst(ClaimTypes.Surname)?.Value} ";

            }
            else
            {
                throw new ArgumentException("Claims Principal not Set!");
            }
        }


        public static String Email(this IPrincipal principal)
        {
            var cliamsPrincipal = principal as ClaimsPrincipal;

            if (cliamsPrincipal != null)
            {
                return cliamsPrincipal.FindFirst(ClaimTypes.Email)?.Value;

            }
            else
            {
                throw new ArgumentException("Claims Principal not Set!");
            }
        }
    }
}
