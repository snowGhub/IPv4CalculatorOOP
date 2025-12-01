namespace IPv4Calculator.Logic;

public class CountBasedGenerator : SubnetGeneratorBase
{
    protected override int CalculateNewCidr(int currentCidr, int count)
    {
        int bitsNeeded = (int)Math.Ceiling(Math.Log2(count));
        return currentCidr + bitsNeeded;
    }
}