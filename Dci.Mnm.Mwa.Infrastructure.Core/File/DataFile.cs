using System.IO;
using Dci.Mnm.Mwa.Core;

namespace Dci.Mnm.Mwa.Infrastructure.Core.File
{
    public class DataFile : Entity
    {
        public string Name { get; set; }
        public string MimeType { get; set; }
        public MemoryStream Data { get; set; }
    }
}








