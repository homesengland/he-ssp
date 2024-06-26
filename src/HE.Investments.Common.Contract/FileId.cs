namespace HE.Investments.Common.Contract;

public record FileId : StringIdValueObject
{
    private FileId(string value)
        : base(value)
    {
    }

    public static FileId From(string value) => new(value);

    public static FileId GenerateNew() => new(Guid.NewGuid().ToString("N"));

    public override string ToString()
    {
        return Value;
    }
}
