using Dci.Mnm.Mwa.Core;
using Dci.Mnm.Mwa.Infrastructure.Core;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using Shark.PdfConvert;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace Dci.Mnm.Mwa.Infrastructure.HtmlToPdf
{
    // https://github.com/jmanm/PDFsharp-netstandard2.0
    // https://github.com/cp79shark/Shark.PdfConvert
    public class WkHtmlToPdfGenerator : IHtmlToPdfGenerator
    {
        private readonly AppConfig settings;

        public WkHtmlToPdfGenerator(AppConfig settings)
        {
            this.settings = settings;
        }

        public string GetBase64Image(string name)
        {
            using var bitmap = Reports.ReportImages.ResourceManager.GetObject(name) as Bitmap;
            using var memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, ImageFormat.Png);

            return System.Convert.ToBase64String(memoryStream.ToArray());
        }

        public void Convert(IEnumerable<Stream> htmlSources, Stream outputStream)
        {
            var pdfConversionSettings = new PdfConversionSettings
            {

                PdfToolPath =
                    Path.IsPathRooted(settings.Files.htmlToPdfPath) ?
                    settings.Files.htmlToPdfPath :
                    Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), settings.Files.htmlToPdfPath),

            };

            //pdfConversionSettings.PdfToolPath=pdfConversionSettings.PdfToolPath.Remove(pdfConversionSettings.PdfToolPath.Length-1);

            var pdfStreams =
                htmlSources
                .ToList()
                .Select(html =>
                {
                    var pdf = new MemoryStream();
                    ToPdf(html, pdf, pdfConversionSettings);
                    return pdf;
                });

            MergePdfs(pdfStreams, outputStream);
        }

        public void Convert(IEnumerable<string> htmlSources, Stream outputStream)
        {
            var htmlStreams =
                htmlSources.Select(src => new MemoryStream(Encoding.UTF8.GetBytes(src)));

            Convert(htmlStreams, outputStream);
        }

        public void Convert(IEnumerable<string> htmlSources, Stream outputStream, PdfSettings pdfSettings)
        {
            var htmlStreams =
               htmlSources.Select(src => new MemoryStream(Encoding.UTF8.GetBytes(src)));

            var pdfConversionSettings = new PdfConversionSettings
            {

                PdfToolPath = Path.IsPathRooted(settings.Files.htmlToPdfPath) ? settings.Files.htmlToPdfPath : Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), settings.Files.htmlToPdfPath),
                PageWidth = pdfSettings.PageWidth,
                Orientation = pdfSettings.Orientation == null ? PdfPageOrientation.Default : (PdfPageOrientation)pdfSettings.Orientation,
                Size = pdfSettings.PageSize == null ? PdfPageSize.Default : (PdfPageSize)pdfSettings.PageSize,
                Margins = pdfSettings.Margin == null ? null : new PdfPageMargins { Top = pdfSettings.Margin.Top, Bottom = pdfSettings.Margin.Bottom, Right = pdfSettings.Margin.Right, Left = pdfSettings.Margin.Left }
            };

            var pdfStreams =
               htmlStreams
               .ToList()
               .Select(html =>
               {
                   var pdf = new MemoryStream();
                   ToPdf(html, pdf, pdfConversionSettings);
                   return pdf;
               });

            MergePdfs(pdfStreams, outputStream);
        }

        public static void ToPdf(Stream src, Stream dest, PdfConversionSettings pdfConversionSettings)
        {
            using (src)
            {
                PdfConvert.Convert(pdfConversionSettings, dest, src);
            }
        }

        public static void MergePdfs(IEnumerable<Stream> pdfsStreams, Stream outputStream)
        {
            using (PdfDocument targetDoc = new PdfDocument())
            {
                foreach (var pdf in pdfsStreams)
                {
                    using (PdfDocument pdfDoc = PdfReader.Open(pdf, PdfDocumentOpenMode.Import))
                    {
                        for (int i = 0; i < pdfDoc.PageCount; i++)
                        {
                            targetDoc.AddPage(pdfDoc.Pages[i]);
                        }
                    }
                }

                //AddPageNumbers(targetDoc);

                targetDoc.Save(outputStream);
            }
        }

        private static void AddPageNumbers(PdfDocument pdf)
        {
            var totalPages = pdf.PageCount;

            for (var pageIndex = 0; pageIndex < totalPages; pageIndex++)
            {
                AddPageNumber(pdf.Pages[pageIndex], pageIndex, totalPages);
            }

        }

        private static void AddPageNumber(PdfPage page, int pageIndex, int totalPages)
        {
            using (XGraphics gfx = XGraphics.FromPdfPage(page))
            {
                XFont font = new XFont("Helvetica", 12, XFontStyle.Regular);

                var pageNumberString = $"Page {pageIndex + 1} of {totalPages}";

                gfx.DrawString(pageNumberString, font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height - 20), XStringFormat.BottomCenter);
            }
        }


    }
}









