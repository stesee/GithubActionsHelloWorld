using System.Threading.Tasks;
using Xunit;

namespace GithubActionsHelloWorldTests
{
    public class AsyncCodeAnalyzerDemo
    {
        [Fact]
        public async Task TriggersCS414()
        {
            // triggers https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/cs4014?f1url=%3FappId%3Droslyn%26k%3Dk(CS4014)
            Task.Delay(3000);

            // dummy to silence CS1998
            await Task.Delay(1);

            Assert.True(true);
        }

        [Fact]
        public void TriggersVSTHRD110()
        {
            //  does not trigger https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/cs4014?f1url=%3FappId%3Droslyn%26k%3Dk(CS4014)
            // because the method is not async
            // installing Microsoft.VisualStudio.Threading.Analyzers will enable an analyzer that will trigger https://microsoft.github.io/vs-threading/analyzers/VSTHRD110.html
            Task.Delay(3000);
            Assert.True(true);
        }

        [Fact]
        public void NoAnalyzerFindsThatIssue()
        {
            //  does not trigger https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/cs4014?f1url=%3FappId%3Droslyn%26k%3Dk(CS4014) or https://microsoft.github.io/vs-threading/analyzers/VSTHRD110.html
            var bla = Task.Delay(3000);
            Assert.NotNull(bla);
            Assert.True(bla.IsCompleted);
        }
    }
}
