using GithubActionsHelloWorld;
using ServiceReference;
using System;
using System.ServiceModel;
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

        // used these steps to generate the soap client
        // dotnet tool install --global dotnet-svcutil
        // dotnet-svcutil http://www.gcomputer.net/webservices/dilbert.asmx?WSDL
        // get more soap endpoints at https://stackoverflow.com/questions/1958048/public-soap-web-services

        [Fact]
        public async Task ShouldDailyDilbertAsync()
        {
            var client = new DilbertSoapClient(DilbertSoapClient.EndpointConfiguration.DilbertSoap);

            var actual = await client.DailyDilbertAsync(DateTime.Now);

            Assert.NotEmpty(actual.Body.DailyDilbertResult);
        }

        [Fact]
        public async Task ShouldDailyDilbertWithCustomMessageSizeAsync()
        {
            // used these steps to generate the soap client
            // dotnet tool install --global dotnet-svcutil
            // dotnet-svcutil http://www.gcomputer.net/webservices/dilbert.asmx?WSDL
            var binding = new BasicHttpBinding
            {
                // That will trigger an exception on using the client (e.g. client.DailyDilbertAsync). MaxReceivedMessageSize should probably be an int instead of an long and/or I expect an exception while setting the message size.
                MaxReceivedMessageSize = (long)int.MaxValue + 1
            };
            var remoteAddress = new EndpointAddress("http://www.gcomputer.net/webservices/dilbert.asmx");

            var client = new DilbertSoapClient(binding, remoteAddress);

            // System.ArgumentOutOfRangeException : This factory buffers messages, so the message sizes must be in the range of an integer value. (Parameter 'bindingElement.MaxReceivedMessageSize')
            var actual = await client.DailyDilbertAsync(DateTime.Now);

            Assert.NotEmpty(actual.Body.DailyDilbertResult);
        }
    }
}