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
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            string expected = "9/2/2010 12:00:00 AM";
            DateTime parsedDate = DateTime.Parse(expected);
            var actual = parsedDate.ToString();

            Assert.Equal(expected, actual);
        }
    }
}