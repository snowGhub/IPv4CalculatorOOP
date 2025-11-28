using IPv4Calculator.Core;

namespace IPv4Calculator.Logic;

public record SubnetResult(
        IPv4Address NetworkId,
        IPv4Address Broadcast,
        IPv4Address FirstHost,
        IPv4Address LastHost,
        int Cidr,
        long TotalHosts
    );