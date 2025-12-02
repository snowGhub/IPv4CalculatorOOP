namespace IPv4Calculator.Core;

public class IPv4Address(uint value) : IpAddressBase
{
    private readonly uint _value = value;

    public static IPv4Address Parse(string? input)
    {
        // Input leer!
        if (string.IsNullOrWhiteSpace(input)) throw new ArgumentException("Input darf nicht leer sein!");
        var parts = input.Split('.');
        if (parts.Length != 4) throw new FormatException("Input muss aus 4 Oktetts bestehen!");

        uint result = 0;
        for (var i = 0; i < 4; i++)
        {
            if (!byte.TryParse(parts[i], out byte octet))
                throw new FormatException($"Oktett {i + 1} ('{parts[i]}') ist ungültig. Muss zwischen 0 und 255 liegen!");

            result |= (uint)(octet << (8 * (3 - i)));
        }
        
        return new IPv4Address(result);
    }
    
    public override byte[] GetBytes()
    {
        return
        [
            (byte)(_value >> 24),
            (byte)(_value >> 16),
            (byte)(_value >> 8),
            (byte)(_value)
        ];
    }

    public override string ToString() => string.Join('.', GetBytes());

    public int ToCidr()
    {
        var temp = _value;
        var cidr = 0;
        // 8 HEX = 1000
        var checker = 0x80000000;

        for (var i = 0; i < 32; i++)
        {
            if ((temp & checker) == checker)
            {
                cidr++;
                checker = checker >> 1;
            }
            else
            {
                // Sobald eine 0 reinläuft, ist Schluss (Maske muss immer zusammenhängen)
                break;
            }
        }

        return cidr;
    }
    
    public static IPv4Address operator &(IPv4Address a, IPv4Address b) => new(a._value & b._value);
    public static IPv4Address operator ~(IPv4Address a) => new(~a._value);
    public static IPv4Address operator |(IPv4Address a, IPv4Address b) => new(a._value | b._value);
    public static IPv4Address operator +(IPv4Address a, uint val) => new(a._value + val);
    public static IPv4Address operator -(IPv4Address a, uint val) => new(a._value - val);
}