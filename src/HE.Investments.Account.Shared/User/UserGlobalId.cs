namespace HE.Investments.Account.Shared.User;

public record UserGlobalId(string Value)
{
    public static UserGlobalId From(string value) => new(value);

    public override string ToString()
    {
        return Value;
    }
}
