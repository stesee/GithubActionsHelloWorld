using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace GithubActionsHelloWorldTests
{
    public class AsyncCodeAnalyzerDemo
    {
        [Fact]
        public async Task CS414()
        {
            // triggers https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/cs4014?f1url=%3FappId%3Droslyn%26k%3Dk(CS4014)
            File.WriteAllTextAsync("test.txt", "Hello World");

            // dummy to silence CS1998
            await Task.Delay(1);

            Assert.True(true);
        }

        [Fact]
        public void CS4141()
        {
            //  does not trigger https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/cs4014?f1url=%3FappId%3Droslyn%26k%3Dk(CS4014)
            // because the method is not async
            // installing Microsoft.VisualStudio.Threading.Analyzers will enable an analyzer that will trigger https://microsoft.github.io/vs-threading/analyzers/VSTHRD110.html
            File.WriteAllTextAsync("test.txt", "Hello World");
            Assert.True(true);
        }

        [Fact]
        public void NoAnalyzerFindsThat()
        {
            //  does not trigger https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/cs4014?f1url=%3FappId%3Droslyn%26k%3Dk(CS4014) or https://microsoft.github.io/vs-threading/analyzers/VSTHRD110.html
            var bla = File.WriteAllTextAsync("test.txt", "Hello World");
            Assert.NotNull(bla);
        }
    }
}
