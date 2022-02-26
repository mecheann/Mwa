using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dci.Mnm.Mwa.Core
{
    public class MwaException : ApplicationException
    {
        public MwaException() : base()
        {
        }

        public MwaException(string message) : base(message)
        {
        }

        public MwaException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
