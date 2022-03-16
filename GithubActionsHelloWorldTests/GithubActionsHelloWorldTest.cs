using GithubActionsHelloWorld;
using System;
using System.Threading.Tasks;
using Xunit;

namespace GithubActionsHelloWorldTests
{
    public class GithubActionsHelloWorldTest
    {
        [Fact]
        public async Task CreateSelfHostedRunner()
        {
            await System.IO.File.WriteAllTextAsync("c:/temp/SelfHostedGithubActionRunner.txt", DateTime.Now.ToString());
        }

        [Fact]
        public async Task Main_ShouldRunWithoutError()
        {
            var args = new string[] { "WrongParameterCount" };
            await Program.Main(args);
        }
    }
}