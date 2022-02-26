using System;
using System.Collections.Generic;
using System.Text;

namespace Dci.Mnm.Mwa.Infrastructure.Core
{
    public class PdfSettings
    {
        public PageSize? PageSize { get; set; }
        public float? PageWidth { get; set; }
        public PageOrientation? Orientation { get; set; }
        public PageMargin? Margin { get; set; }
    }
}
