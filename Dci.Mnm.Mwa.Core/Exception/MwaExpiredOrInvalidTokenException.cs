using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dci.Mnm.Mwa.Core
{
    public class MwaExpiredOrInvalidTokenException: ApplicationException
    {
        public MwaExpiredOrInvalidTokenException() : base()
        {
        }

        public MwaExpiredOrInvalidTokenException(string message) : base(message)
        {
        }

        public MwaExpiredOrInvalidTokenException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
