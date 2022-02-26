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
    public class UserLogin : IdentityUserLogin<Guid>, IEntity<Guid>
    {
        [NotMapped]
        public Guid Id { get; set; }
        public Guid? CreatedById { get; set; }
        public Guid? ModifiedById { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset? CreationDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset? ModificationDate { get; set; }
        public byte[] RowVersion { get; set; }
    }

}
