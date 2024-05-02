namespace HE.Investments.Common.Contract;

public record OrganisationId : StringIdValueObject
{
    public OrganisationId(string id)
        : base(id)
    {
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
