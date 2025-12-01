namespace IPv4Calculator.Core;

public class IpNetwork
{
    public IPv4Address Address { get; }
    public int Cidr { get; }
    public IPv4Address Mask { get;  }
    
    public IpNetwork(IPv4Address address, int cidr)
    {
        Address = address;
        Cidr = cidr;
        Mask = cidr == 0 ? new IPv4Address(0) : new IPv4Address(0xFFFFFFFF << (32 - cidr));
    }

    public IpNetwork(IPv4Address address, IPv4Address mask)
    {
        Address = address;
        Mask = mask;
        Cidr = mask.ToCidr();
    }
}