using GithubActionsHelloWorld;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace GithubActionsHelloWorldTests
{
    public class GithubActionsHelloWorldTest
    {
        [Fact]
        public async Task Main_ShouldRunWithoutError()
        {
            var args = new string[] { "WrongParameterCount" };
            await Program.Main(args);
        }

        [Fact]
        public async Task Main_ShouldConvertVideo()
        {
            var inputPath = "../../../small.mp4";
            var outputPath = "../../../smallNoSound.mp4";

            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }

            await Program.Main(new[] { inputPath, outputPath });

            Assert.True(File.Exists(outputPath));
        }
    }
}