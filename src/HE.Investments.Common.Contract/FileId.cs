namespace HE.Investments.Common.Contract;

public record FileId : StringIdValueObject
{
    private FileId(string id)
        : base(id)
    {
    }

    public static FileId From(string value) => new(value);

    public static FileId GenerateNew() => new(Guid.NewGuid().ToString("N"));

    public override string ToString()
    {
        return Value;
    }
}
