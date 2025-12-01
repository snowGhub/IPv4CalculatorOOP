namespace IPv4Calculator.Core;

public abstract class IpAddressBase
{
    public abstract byte[] GetBytes();
    public abstract override string ToString();

    // Jede IP muss wissen, ob sie valid ist (ich erzwingen hierdurch eine Implementierung)
    public abstract bool IsValid();
}