using BitMiracle.LibTiff.Classic;
using PdfSharpCore.Drawing;
using PdfSharpCore.Fonts;
using PdfSharpCore.Pdf;
using PdfSharpCore.Utils;
using SkiaSharp;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace GithubActionsHelloWorld
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            GlobalFontSettings.FontResolver = new FontResolver();

            var document = new PdfDocument();
            using var fileStream = File.OpenRead(args[0]);

            var tempImageFiles = TiffToBitmap(args[0]);
            try
            {
                for (int pageIndex = 0; pageIndex < tempImageFiles.Count; pageIndex++)
                {
                    var pageWithImage = new PdfPage();
                    document.Pages.Add(pageWithImage);
                    var xgr = XGraphics.FromPdfPage(document.Pages[pageIndex]);

                    var xImage = XImage.FromStream(() => File.OpenRead(tempImageFiles[pageIndex]));
                    xgr.DrawImage(xImage, 0, 0);
                }

                document.Save("helloworld.pdf");
            }
            finally
            {
                foreach (var tempImageFile in tempImageFiles)
                {
                    File.Delete(tempImageFile);
                }
            }
        }

        private static List<string> TiffToBitmap(string tiffFilePath)
        {
            var tempPaths = new List<string>();
            using var tiff = Tiff.Open(tiffFilePath, "r");
            var numberIfTiffPages = GetNumberofTiffPages(tiff);

            for (short i = 0; i < numberIfTiffPages; i++)
            {
                tiff.SetDirectory(i);
                var width = tiff.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
                var height = tiff.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
                var bitmap = new SKBitmap();
                var info = new SKImageInfo(width, height);
                var raster = new int[width * height];
                var ptr = GCHandle.Alloc(raster, GCHandleType.Pinned);
                bitmap.InstallPixels(info, ptr.AddrOfPinnedObject(), info.RowBytes, (addr, ctx) => ptr.Free(), null);

                if (!tiff.ReadRGBAImageOriented(width, height, raster, Orientation.TOPLEFT))
                {
                    // not a valid TIF image.
                    return null;
                }

                if (SKImageInfo.PlatformColorType == SKColorType.Bgra8888)
                {
                    SKSwizzle.SwapRedBlue(ptr.AddrOfPinnedObject(), raster.Length);
                }

                var encodedData = bitmap.Encode(SKEncodedImageFormat.Png, 100);
                var tempPath = Path.Combine(Path.GetTempFileName() + ".png");
                using var bitmapImageStream = File.Open(tempPath, FileMode.Create, FileAccess.Write, FileShare.None);
                encodedData.SaveTo(bitmapImageStream);
                tempPaths.Add(tempPath);
            }
            return tempPaths;
        }

        public static int GetNumberofTiffPages(Tiff image)
        {
            int pageCount = 0;
            do
            {
                ++pageCount;
            } while (image.ReadDirectory());

            return pageCount;
        }
    }
}
