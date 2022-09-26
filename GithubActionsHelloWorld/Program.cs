using PdfSharpCore.Drawing;
using PdfSharpCore.Fonts;
using PdfSharpCore.Pdf;
using PdfSharpCore.Utils;
using SixLabors.ImageSharp;
using System.IO;
using System.Threading.Tasks;

namespace GithubActionsHelloWorld
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            GlobalFontSettings.FontResolver = new FontResolver();

            var document = new PdfDocument();

            using (var tiff = Image.Load(args[0]))

                for (var pageIndex = 0; pageIndex < tiff.Frames.Count; pageIndex++)
                {
                    var pageImage = await ConvertToXImageAsync(tiff.Frames.CloneFrame(pageIndex));
                    var pageWithImage = new PdfPage();
                    document.Pages.Add(pageWithImage);
                    var xgr = XGraphics.FromPdfPage(document.Pages[pageIndex]);
                    xgr.DrawImage(pageImage, 0, 0);
                }

            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            var font = new XFont("Arial", 20, XFontStyle.Bold);

            var textColor = XBrushes.Black;
            var layout = new XRect(20, 20, page.Width, page.Height);
            var format = XStringFormats.Center;

            gfx.DrawString("Hello World!", font, textColor, layout, format);

            document.Save("helloworld.pdf");
        }

        private static async Task<XImage> ConvertToXImageAsync(Image imageFrame)
        {
            using var memoryStream = new MemoryStream();
            await imageFrame.SaveAsPngAsync(memoryStream);
            memoryStream.Position = 0;
            var image = XImage.FromStream(() => memoryStream);
            return image;
        }
    }
}