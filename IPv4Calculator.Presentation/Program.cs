
using IPv4Calculator.Core;
using IPv4Calculator.Logic;

namespace IPv4CalculatorOOP;

internal static class Program
{
    private static void Main()
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
                // Hier drin ist immer!!! ein Wert
                HandleSection(choice!);
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
            var net = GetNetworkFromUser();
            var analyzer = new NetworkAnalyzer();
            var result = analyzer.Analyze(net);
            PrintResult(result);
            return;
        }

        SubnetGeneratorBase generator;
        var param = 0;
        var basenet = GetNetworkFromUser();

        switch (choice)
        {
            case "2":
                generator = new CountBasedGenerator();
                Console.WriteLine("Anzal gewünschter Subnetze bitte eingeben:");
                param = int.Parse(Console.ReadLine()!);
                break;
            case "3":
                generator = new HostBasedGenerator();
                Console.Write("Min. Hosts pro Subnetz: ");
                param = int.Parse(Console.ReadLine()!);
                break;
            default: return;
                    
        }
        
       var results = generator.Generate(basenet, param);
        
        Console.WriteLine($"\nGeneriert: {results.Count} Subnetze");
        foreach (var variable in results)
        {
            Console.WriteLine($"Net: {variable.NetworkId, -15} | Mask: /{variable.Cidr, -2} | Hosts: {variable.TotalHosts}");
        }
    }

    static IpNetwork GetNetworkFromUser()
    {
        Console.WriteLine("Eingabgeformat wählen:");
        Console.WriteLine(" - CIDR Schreibweise:");
        Console.WriteLine(" - IP mit Subnetzmaske:");
        Console.WriteLine("Eingabe:");
        
        var input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input)) throw new ArgumentException("Keine Eingabe erkannt.");

        if (input.Contains('/'))
        {
            var parts = input.Split('/');
            if (parts.Length != 2) throw new FormatException("Ungültiges CIDR-Format (zu viele oder keine Slashes).");

            var ip = IPv4Address.Parse(parts[0]);

            if (!int.TryParse(parts[1], out int cidr)) 
                throw new FormatException("CIDR muss eine Zahl sein.");

            if (cidr < 0 || cidr > 32)
                throw new ArgumentException("CIDR muss zwischen 0 und 32 liegen.");

            return new IpNetwork(ip, cidr);
        }
        // Zweiter Usecase mit der Subnetzmasek hier
        else
        {
            var ip = IPv4Address.Parse(input);
            
            Console.Write("Subnetzmaske eingeben:");
            var maskInput = Console.ReadLine()?.Trim();
            var mask = IPv4Address.Parse(maskInput);
            
            return new IpNetwork(ip, mask);
        }
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

