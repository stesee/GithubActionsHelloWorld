using GithubActionsHelloWorld;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GithubActionsHelloWorldTests
{
    public class GithubActionsHelloWorldTest
    {
        [Fact]
        public async Task Main_ShouldRunWithoutError()
        {
            var args = new string[] { "SomeParameter" };
            await Program.Main(args);
        }

        [Fact]
        public void ShouldDoTheSameThingOnEachGHARunner()
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

            string expected = "09/02/2010 00:00:00";
            DateTime parsedDate = DateTime.Parse(expected);
            var actual = parsedDate.ToString();

            Assert.Equal(expected, actual);
        }
    }
}