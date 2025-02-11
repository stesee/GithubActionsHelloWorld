using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.IO;
using System.Threading.Tasks;
using ZXing.Common;
using ZXing.Rendering;

namespace GithubActionsHelloWorld
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var barcodeWriter = new ZXing.BarcodeWriterPixelData
            {
                Format = ZXing.BarcodeFormat.CODE_128,
                Options = new EncodingOptions
                {
                    Height = 300,
                    Width = 300,
                },
                Renderer = new PixelDataRenderer
                {
                }
            };

            var pixelData = barcodeWriter.Write("Hallo Barcode");
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "barcode.bmp");
            using (var image = Image.LoadPixelData<Rgba32>(pixelData.Pixels, 300, 300))
                await image.SaveAsBmpAsync(path);

            Console.WriteLine("Barcode saved to " + path);
        }
    }
}