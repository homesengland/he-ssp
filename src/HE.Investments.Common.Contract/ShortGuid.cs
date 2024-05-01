namespace HE.Investments.Common.Contract;

public class ShortGuid
{
    public ShortGuid(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static ShortGuid FromGuid(Guid value) => new(Encode(value));

    public static ShortGuid FromString(string value) => new(Encode(value));

    public static Guid ToGuid(string shortGuid) => Decode(shortGuid);

    public static string ToGuidAsString(string shortGuid) => Decode(shortGuid).ToString();

    private static string Encode(Guid guid)
    {
        var encoded = Convert.ToBase64String(guid.ToByteArray());
        encoded = encoded.Replace("/", "_").Replace("+", "-");
        return encoded[..22];
    }

    private static string Encode(string value)
    {
        return !Guid.TryParse(value, out var guid) ? value : Encode(guid);
    }

    private static Guid Decode(string value)
    {
        if (Guid.TryParse(value, out _))
        {
            return new Guid(value);
        }

        value = value.Replace("_", "/").Replace("-", "+");
        var buffer = Convert.FromBase64String(value + "==");
        return new Guid(buffer);
    }
}
