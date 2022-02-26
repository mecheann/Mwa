using System;

namespace Dci.Mnm.Mwa.Core
{
    public class MwaNoPermissonException:ApplicationException
    {
        public MwaNoPermissonException() : base()
        {
        }

        public MwaNoPermissonException(string message) : base(message)
        {
        }

        public MwaNoPermissonException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
