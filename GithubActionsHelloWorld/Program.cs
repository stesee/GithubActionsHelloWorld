using System;
using System.Threading.Tasks;

namespace GithubActionsHelloWorld
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            await Task.Delay(100);
            Console.WriteLine("Hello World!");
        }
    }
}