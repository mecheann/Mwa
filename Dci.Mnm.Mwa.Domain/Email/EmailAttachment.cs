using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dci.Mnm.Mwa.Core;
using System.IO;

namespace Dci.Mnm.Mwa.Domain
{
    public class EmailAttachment : Entity
    {
        public Stream Content { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public string FileId { get; set; }
    }
}
