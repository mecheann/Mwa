using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dci.Mnm.Mwa.Core
{
    public class Entity : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string CreatedBy { get; set; }
        public Guid? CreatedById { get; set; }

        public DateTimeOffset? CreationDate { get; set; }

        public string ModifiedBy { get; set; }

        public Guid? ModifiedById { get; set; }
        public DateTimeOffset? ModificationDate { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
