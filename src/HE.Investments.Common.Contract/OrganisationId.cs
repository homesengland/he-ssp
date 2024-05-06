namespace HE.Investments.Common.Contract;

public record OrganisationId : StringIdValueObject
{
    public OrganisationId(string value)
        : base(value)
    {
    }

    public static OrganisationId From(string value) => new(ShortGuid.FromString(value).Value);

    public static OrganisationId From(Guid value) => new(ShortGuid.FromGuid(value).Value);

    public string ToGuidAsString() => ShortGuid.ToGuidAsString(Value);

    public override string ToString()
    {
        return base.ToString();
    }
}
