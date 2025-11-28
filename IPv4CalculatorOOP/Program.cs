// See https://aka.ms/new-console-template for more information

using IPv4Calculator.Core;

namespace IPv4Calculator.CLI;

internal static class Program
{
    private static void Main(string[] args)
    {
     Console.WriteLine("IPv4Calculator v2");
     Console.WriteLine("=================");
     Console.WriteLine("Bitte IP eingeben:");
     var ipInput = Console.ReadLine();
     var ip = IPv4Address.Parse(ipInput); 
    }
}

