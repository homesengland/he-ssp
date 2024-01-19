namespace HE.Investments.Common.Contract;

public record UserGlobalId : StringIdValueObject
{
    public UserGlobalId(string value)
        : base(value)
    {
    }

    public static UserGlobalId From(string value) => new(value);

    public override string ToString()
    {
        return Value;
    }
}
