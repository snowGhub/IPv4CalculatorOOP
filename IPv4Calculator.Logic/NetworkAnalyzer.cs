using IPv4Calculator.Core;

namespace IPv4Calculator.Logic;

public class NetworkAnalyzer
{
    public SubnetResult Analyze(IpNetwork network)
    {
        var netId = network.Address & network.Mask;
        var broadcast = netId | (~network.Mask);
        var totalHosts = (long)Math.Pow(2, 32 - network.Cidr) - 2;

        return new SubnetResult(
            netId,
            broadcast,
            netId + 1,
            broadcast - 1,
            network.Cidr,
            totalHosts > 0 ? totalHosts : 0
            );
    }
}