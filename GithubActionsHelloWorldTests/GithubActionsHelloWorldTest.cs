using GithubActionsHelloWorld;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace GithubActionsHelloWorldTests
{
    public class GithubActionsHelloWorldTest
    {
        private const string expectedCreatedFilepath = "helloworld.pdf";

        [Fact]
        public async Task Main_ShouldRunWithoutError()
        {
            var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().Location);
            var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
            var dirPath = Path.GetDirectoryName(codeBasePath);
            var multipageTiff = Path.GetFullPath(Path.Combine(dirPath, "../../../MultiPage.tiff"));

            if (File.Exists(expectedCreatedFilepath))
            {
                File.Delete(expectedCreatedFilepath);
            }

            var args = new string[] { multipageTiff };
            await Program.Main(args);

            Assert.True(File.Exists(expectedCreatedFilepath));

            using var process = new Process();
            process.StartInfo.FileName = expectedCreatedFilepath;
            process.StartInfo.UseShellExecute = true;
            process.Start();
        }
    }
}