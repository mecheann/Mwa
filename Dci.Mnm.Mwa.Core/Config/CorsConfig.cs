using System.Collections.Generic;

namespace Dci.Mnm.Mwa.Core
{
    public class CorsConfig
    {
        public CorsConfig()
        {
            this.AllowedHost = new List<string>();
        }
        public List<string> AllowedHost { get; set; }
    }
}








