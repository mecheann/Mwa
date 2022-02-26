using System.Collections.Generic;
using System.IO;

namespace Dci.Mnm.Mwa.Infrastructure.Core
{
    public interface IHtmlToPdfGenerator
    {
        void Convert(IEnumerable<Stream> htmlSources, Stream outputStream);
        void Convert(IEnumerable<string> htmlSources, Stream outputStream);
        //TODO: add convert method /w pdfsettings
        void Convert(IEnumerable<string> htmlSources, Stream outputStream, PdfSettings pdfSettings);
        string GetBase64Image(string name);
    }
}








