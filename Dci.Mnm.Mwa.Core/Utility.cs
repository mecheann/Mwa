using RT.Comb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Dci.Mnm.Mwa.Core
{
    public static class Utility
    {
        public static SqlCombProvider GuidGenerator = new SqlCombProvider(new RT.Comb.SqlDateTimeStrategy());

        public static Guid CreateNewId()
        {
            return GuidGenerator.Create();
        }

        public static int GenerateRandomNumber(int minValue = 1000, int maxValue = 9999)
        {
            var random = new Random();
            return random.Next(minValue, maxValue);
        }

        public static String GetInnerMessages(this Exception exception, int? maxDepth = null)
        {
            if (maxDepth == null) maxDepth = 5;

            StringBuilder message = new StringBuilder();
            message.Append(exception.Message);

            if (exception.InnerException != null && maxDepth > 0)
            {
                message.AppendFormat("with Inner Exception: {0}", exception.InnerException.GetInnerMessages(--maxDepth));
                return message.ToString();
            }
            else
            {
                return message.ToString();
            }
        }

        public static class Validator
        {
            public static void ThrowIf(Boolean condition, String message, params string[] messageParams)
            {
                if (condition) throw new MwaException(String.Format(message, messageParams));
            }

            public static void ThrowNoPermissionIf(Boolean condition, String message, params string[] messageParams)
            {
                if (condition) throw new MwaNoPermissonException(String.Format(message, messageParams));
            }

            public static void ThrowSystemIf(Boolean condition, String message, params string[] messageParams)
            {
                if (condition) throw new SystemException(String.Format(message, messageParams));
            }

            public static void ThrowIfStringEmpty(String value, String message, params string[] messageParams)
            {
                Validator.ThrowIf(String.IsNullOrEmpty(value), message, messageParams);
            }

            public static void ThrowIfValueNull(object value, String message, params string[] messageParams)
            {
                Validator.ThrowIf(value == null, message, messageParams);
            }

            public static void ThrowIfStringNotSameIgnoreCase(string right, string left, String message, params string[] messageParams)
            {
                ThrowIf(String.Compare(right, left, true) != 0, message, messageParams);
            }

            public static void ThrowIfStringNotSameCaseSenative(string right, string left, String message, params string[] messageParams)
            {
                ThrowIf(String.Compare(right, left, false) != 0, message, messageParams);
            }

            public static void ThrowIfUserDoesHavePermission(IPrincipal principal, string action, String message = null, params string[] messageParams)
            {
                if (String.IsNullOrEmpty(message))
                {
                    message = $"{principal.UserName()} does not have permission to {action} operation";
                }
                ThrowNoPermissionIf(principal.Cannot(action), message, messageParams);
            }

        }

    }
}
