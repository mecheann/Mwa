using Dci.Mnm.Mwa.Core;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dci.Mnm.Mwa.Domain
{
    public class UserClaim : IdentityUserClaim<Guid>, IEntity<int>
    {
        public int Id { get; set; }
        public Guid? CreatedById { get; set; }
        public Guid? ModifiedById { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset? CreationDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset? ModificationDate { get; set; }
        public byte[] RowVersion { get; set; }

    }
}
