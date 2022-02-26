using Dci.Mnm.Mwa.Core;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dci.Mnm.Mwa.Domain
{
    public class Role : IdentityRole<Guid>, IEntity<Guid>
    {
        public Guid? CreatedById { get; set; }
        public Guid? ModifiedById { get; set; }
        public string Description { get; set; }
        public RoleStatus Status { get; set; }

        public string CreatedBy { get; set; }
        public DateTimeOffset? CreationDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset? ModificationDate { get; set; }
        public byte[] RowVersion { get; set; }
    }
}









