using GithubActionsHelloWorld;
using Xunit;

namespace GithubActionsHelloWorldTests
{
    public class GithubActionsHelloWorldTest
    {
        [Fact]
        public void Main_ShouldRunWithoutError()
        {
            var args = new string[] { "someParameter" };
            Program.Main(args);
        }
    }
}