using FFmpeg.NET;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace GithubActionsHelloWorld
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            if (args?.Length != 2)
            {
                Console.WriteLine("Only two parameters supported:");
                Console.WriteLine("e.g. GithubActionsHelloWorld.exe <inputFile> <outputfile>");
                return;
            }

            var inputFilePath = args[0];
            var outputFilePath = args[1];

            var inputFile = new InputFile(inputFilePath);
            var outputFile = new OutputFile(outputFilePath);

            string ffmpgeBinPath;
            ffmpgeBinPath = CalcOsSpecificFfmpegPath();

            var ffmpeg = new Engine(ffmpgeBinPath);

            var options = new ConversionOptions
            {
                RemoveAudio = true
            };

            var output = await ffmpeg.ConvertAsync(inputFile, outputFile, options, default).ConfigureAwait(false);

            var metadata = await ffmpeg.GetMetaDataAsync(new InputFile(output.FileInfo.FullName), default).ConfigureAwait(false);

            Console.WriteLine(metadata.FileInfo.FullName);
            Console.WriteLine(metadata);
        }

        private static string CalcOsSpecificFfmpegPath()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return @"../../../../GithubActionsHelloWorld\ffmpebBins\win32\ffmpeg.exe";
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return @"../../../../GithubActionsHelloWorld\ffmpebBins\macos64\ffmpeg.exe";
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var wellKnownPathOnLinuxSystems = "/usr/bin/ffmpeg";
                if (File.Exists(wellKnownPathOnLinuxSystems))
                {
                    return wellKnownPathOnLinuxSystems;
                }
                throw new FileNotFoundException($"Expected ffmpeg at {wellKnownPathOnLinuxSystems} not found.");
            }

            throw new NotSupportedException("Your Os was not recognized / is not supported");
        }
    }
}