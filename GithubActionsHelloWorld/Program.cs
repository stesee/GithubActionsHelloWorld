using FFMpegCore;
using Mono.Unix;
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

            GlobalFFOptions.Configure(options => options.BinaryFolder = CalcOsSpecificFfmpegPath());

            FFMpeg.Mute(inputFilePath, outputFilePath);
        }

        private static string CalcOsSpecificFfmpegPath()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return @"../../../../GithubActionsHelloWorld\ffmpebBins\win32\";
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var ffmpegExecutable = Path.Combine(macosFfmpegBinarySource, "ffmpeg");

                var unixFileInfo = new UnixFileInfo(ffmpegExecutable)
                {
                    FileAccessPermissions = FileAccessPermissions.OtherExecute |
                    FileAccessPermissions.UserRead | FileAccessPermissions.UserWrite
                    | FileAccessPermissions.GroupRead
                    | FileAccessPermissions.OtherRead
                };
                unixFileInfo.Refresh();

                return macosFfmpegBinarySource;
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
    }
}