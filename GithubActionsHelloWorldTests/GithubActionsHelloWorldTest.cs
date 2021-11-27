using GithubActionsHelloWorld;
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
    }
}