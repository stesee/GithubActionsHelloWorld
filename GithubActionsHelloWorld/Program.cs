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
            var fileStream = File.OpenRead(args[0]); ;

            var pageImages = tiffToBitmap(args[0]);
            for (int pageIndex = 0; pageIndex < pageImages.Count; pageIndex++)

            {
                var pageWithImage = new PdfPage();
                document.Pages.Add(pageWithImage);
                var xgr = XGraphics.FromPdfPage(document.Pages[pageIndex]);

                //    var memorystream = new MemoryStream(pageImage.Bytes);
                //memorystream.Position = 0;
                //var xImage = XImage.FromStream(() => memorystream);
                var xImage = XImage.FromStream(() => File.OpenRead(pageImages[pageIndex]));
                xgr.DrawImage(xImage, 0, 0);
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

        private static List<string> tiffToBitmap(string fileName)
        {
            var tempPaths = new List<string>();

            using var tiff = Tiff.Open(fileName, "r");

            var numberIfTiffPages = getNumberofTiffPages(tiff);

            for (short i = 0; i < numberIfTiffPages; i++)
            {
                tiff.SetDirectory(i);

                // read the dimensions
                var width = tiff.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
                var height = tiff.GetField(TiffTag.IMAGELENGTH)[0].ToInt();

                // create the bitmap
                var bitmap = new SKBitmap();
                var info = new SKImageInfo(width, height);

                // create the buffer that will hold the pixels
                var raster = new int[width * height];

                // get a pointer to the buffer, and give it to the bitmap
                var ptr = GCHandle.Alloc(raster, GCHandleType.Pinned);
                bitmap.InstallPixels(info, ptr.AddrOfPinnedObject(), info.RowBytes, null, (addr, ctx) => ptr.Free(), null);

                // read the image into the memory buffer
                if (!tiff.ReadRGBAImageOriented(width, height, raster, Orientation.TOPLEFT))
                {
                    // not a valid TIF image.
                    return null;
                }

                // swap the red and blue because SkiaSharp may differ from the tiff
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

        public static int getNumberofTiffPages(Tiff image)
        {
            int pageCount = 0;
            do
            {
                ++pageCount;
            } while (image.ReadDirectory());

            return pageCount;
        }

        public static SKBitmap OpenTiff(Stream tiffStream)
        {
            // https://stackoverflow.com/questions/50312937/skiasharp-tiff-support

            tiffStream.Position = 0;
            // open a TIFF stored in the stream
            using (var tifImg = Tiff.ClientOpen("in-memory", "r", tiffStream, new TiffStream()))
            {
                tifImg.SetDirectory(1);

                // read the dimensions
                var width = tifImg.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
                var height = tifImg.GetField(TiffTag.IMAGELENGTH)[0].ToInt();

                // create the bitmap
                var bitmap = new SKBitmap();
                var info = new SKImageInfo(width, height);

                // create the buffer that will hold the pixels
                var raster = new int[width * height];

                // get a pointer to the buffer, and give it to the bitmap
                var ptr = GCHandle.Alloc(raster, GCHandleType.Pinned);
                bitmap.InstallPixels(info, ptr.AddrOfPinnedObject(), info.RowBytes, null, (addr, ctx) => ptr.Free(), null);

                // read the image into the memory buffer
                if (!tifImg.ReadRGBAImageOriented(width, height, raster, Orientation.TOPLEFT, true))
                {
                    // not a valid TIF image.
                    return null;
                }

                // swap the red and blue because SkiaSharp may differ from the tiff
                if (SKImageInfo.PlatformColorType == SKColorType.Bgra8888)
                {
                    SKSwizzle.SwapRedBlue(ptr.AddrOfPinnedObject(), raster.Length);
                }

                File.WriteAllBytes("test.bmp", bitmap.Bytes);

                return bitmap;
            }
        }
    }
}