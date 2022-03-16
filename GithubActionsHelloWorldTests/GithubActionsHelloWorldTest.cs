using GithubActionsHelloWorld;
using Xunit;

namespace GithubActionsHelloWorldTests
{
    public class GithubActionsHelloWorldTest
    {
        [Fact]
        public void Main_ShouldRunWithoutError()
        {
            var args = new string[] { "WrongParameterCount" };
            var path = Program.CreateExcelDocument();

            Assert.True(System.IO.File.Exists(path));
        }
    }
}