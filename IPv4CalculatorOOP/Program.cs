// See https://aka.ms/new-console-template for more information

using IPv4Calculator.Core;
using IPv4Calculator.Logic;

namespace IPv4Calculator.CLI;

internal static class Program
{
    private static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("IPv4Calculator v2");
            Console.WriteLine("=================");
            Console.WriteLine("Bitte wählen Sie zwischen folgende Optionen:");
            Console.WriteLine("1. Netzwerkanalyse");
            Console.WriteLine("2. Subnetze nach Anzahl");
            Console.WriteLine("3. Subnetze nach Hosts");
            Console.WriteLine("4. Exit");
            Console.Write("> ");
            var choice = Console.ReadLine();
            if (choice == "4") break;

            try
            {
                HandleSection(choice);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"FEHLER. {e.Message}");
                Console.ResetColor();    
            }
            
            Console.WriteLine("\nTaste drücken...");
            Console.ReadKey();
        }
    }

    static void HandleSection(string choice)
    {
        if (choice == "1")
        {
            var net = GetNeworkFromUser();
            var analyzer = new NetworkAnalyzer();
            var result = analyzer.Analyze(net);
            PrintResult(result);
            return;
        }

        SubnetGeneratorBase generator = null;
        int param = 0;
        var basenet = GetNeworkFromUser();

        if (choice == "2")
        {
            generator = new CountBasedGenerator();
            Console.WriteLine("Anzal gewünschter Subnetze eingeben: ");
            param = int.Parse(Console.ReadLine());
        }
         else if (choice == "3")
         {
             generator = new HostBasedGenerator();
             Console.Write("Min. Hosts pro Subnetz: ");
             param = int.Parse(Console.ReadLine());
         }
         else return;
        
        List<SubnetResult> results = generator.Generate(basenet, param);
        
        Console.WriteLine($"\nGeneriert: {results.Count} Subnetze");
        foreach (var VARIABLE in results)
        {
            Console.WriteLine($"Net: {VARIABLE.NetworkId, -15} | Mask: /{VARIABLE.Cidr, -2} | Hosts: {VARIABLE.TotalHosts}");
        }
    }

    static IpNetwork GetNeworkFromUser()
    {
        Console.WriteLine("IP/CIDR eingeben (z.B. 192.168.1.0/24)");
        var input = Console.ReadLine();

        var parts = input.Split("/");
        if (parts.Length != 2) throw new FormatException("Bitte Format IP/CIDR nutzen.");
        
        var ip = IPv4Address.Parse(parts[0]);
        int cidr = int.Parse(parts[1]);
        
        return new IpNetwork(ip, cidr);
    }

    static void PrintResult(SubnetResult result)
    {
        Console.WriteLine("\n--- INFO ---");
        Console.WriteLine($"Netz-ID:    {result.NetworkId}");
        Console.WriteLine($"Broadcast:  {result.Broadcast}");
        Console.WriteLine($"Host-Range:   {result.FirstHost} - {result.LastHost}");
        Console.WriteLine($"Hosts:      {result.TotalHosts}");
    }
}

