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

    public static string TryToGuidAsString(string value) => TryDecode(value, out var guid) ? guid.ToString() : value;

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
        if (TryDecode(value, out var guid))
        {
            return guid;
        }

        throw new FormatException($"Invalid encoded short guid {value}");
    }

    private static bool TryDecode(string value, out Guid guid)
    {
        if (Guid.TryParse(value, out guid))
        {
            return true;
        }

        value = value.Replace("_", "/").Replace("-", "+");

        var buffer = new Span<byte>(new byte[16]);
        if (Convert.TryFromBase64String(value + "==", buffer, out _))
        {
            guid = new Guid(buffer);
            return true;
        }

        return false;
    }
}
