using Dci.Mnm.Mwa.Core;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dci.Mnm.Mwa.Domain
{
    public class User : IdentityUser<Guid>, IEntity<Guid>
    {
        public Guid? CreatedById { get; set; }
        public Guid? ModifiedById { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public UserStatus Active { get; set; }
        public ICollection<string> Group { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset? CreationDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset? ModificationDate { get; set; }
        [NotMapped]
        public byte[] RowVersion { get; set; }
    }
}
