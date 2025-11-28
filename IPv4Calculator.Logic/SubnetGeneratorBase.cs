using IPv4Calculator.Core;

namespace IPv4Calculator.Logic;

public abstract class SubnetGeneratorBase
{
    public List<SubnetResult> Generate(IpNetwork baseNetwork, int parameter)
    {
        var newCidr = CalculateNewCidr(baseNetwork.Cidr, parameter);

        if (newCidr <= baseNetwork.Cidr) throw new InvalidOperationException("Parameter passt nicht ins Ausgangsnetz!");
    }

    protected abstract int CalculateNewCidr(int currentCidr, int parameter);

    protected List<SubnetResult> CreateSubnets(IpNetwork baseNetwork, int newCidr)
    {
        var results = new List<SubnetResult>();
        
        var currentIp = baseNetwork.Address & baseNetwork.Mask;
        
        var step = (uint)Math.Pow(2, 32 - newCidr);
        
        var totalSlots = (long)Math.Pow(2, 32 - baseNetwork.Cidr) / (long)Math.Pow(2, 32 - newCidr);

        for (var i = 0; i < totalSlots; i++)
        {
            var mask = new IPv4Address(0xFFFFFFFF << (32 - newCidr));
            var broadcast = currentIp | (~mask);
            var hosts = (long)Math.Pow(2, 32 - newCidr) - 2;
            
            results.Add(new SubnetResult(
                currentIp,
                broadcast,
                currentIp + 1,
                broadcast - 1,
                newCidr,
                hosts > 0 ? hosts : 0
                ));
            
            currentIp = currentIp + step;
        }
        
        return results;
    }
}