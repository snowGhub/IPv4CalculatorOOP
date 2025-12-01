namespace IPv4Calculator.Logic;

public class HostBasedGenerator : SubnetGeneratorBase
{
    protected override int CalculateNewCidr(int currentCidr, int hosts)
    {
        int bitsNeeded = (int)Math.Ceiling(Math.Log2(hosts));
        return 32 - bitsNeeded;
    }
}