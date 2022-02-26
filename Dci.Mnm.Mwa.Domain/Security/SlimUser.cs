using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dci.Mnm.Mwa.Domain
{
    public class SlimUser
    {
        public Guid Id { get; set; }
        public string ShortId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string PhoneNumber { get; set; }
        public string SessionId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public UserStatus Status { get; set; }
        public string[] Operations { get; set; }
        public string[] Roles { get; set; }
        public string UMAID { get; set; }
        public string FullName { get; set; }
        public string[] Groups { get; set; }
    }
}
