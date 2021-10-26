using CliWrap;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace GithubActionsHelloWorld
{
    public class Program
    {
        private const string macosFfmpegBinarySource = @"../../../../GithubActionsHelloWorld/ffmpebBins/macos64/";

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

            await FfmpegRemoveAudio(await CalcOsSpecificFfmpegPathAsync(), inputFilePath, outputFilePath);
        }

        private static async Task<string> CalcOsSpecificFfmpegPathAsync()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return Path.Combine(@"../../../../GithubActionsHelloWorld\ffmpebBins\win32\", "ffmpeg.exe");
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var ffmpegExecutable = Path.Combine(macosFfmpegBinarySource, "ffmpeg");

                await SetPermissionsAsync(ffmpegExecutable, "+x");

                return ffmpegExecutable;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var wellKnownPathOnLinuxSystems = "/usr/bin/";
                if (File.Exists(wellKnownPathOnLinuxSystems))
                {
                    return wellKnownPathOnLinuxSystems;
                }
                throw new FileNotFoundException($"Expected ffmpeg at {wellKnownPathOnLinuxSystems} not found.");
            }

            throw new NotSupportedException("Your Os was not recognized / is not supported");
        }

        public static async ValueTask SetPermissionsAsync(string filePath, string permissions)
        {
            await Cli.Wrap("/bin/bash").WithArguments(new[] { "-c", $"chmod {permissions} {filePath}" }).ExecuteAsync();
        }

        public static async ValueTask FfmpegRemoveAudio(string ffmpegPath, string inputFilePath, string outputFilePath)
        {
            await Cli.Wrap(ffmpegPath).WithArguments(new[] { "-i", inputFilePath, "-c", "copy", "-an", outputFilePath }).ExecuteAsync();
        }
    }
}