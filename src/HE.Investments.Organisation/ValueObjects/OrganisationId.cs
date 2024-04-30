using HE.Investments.Common.Contract;

namespace HE.Investments.Organisation.ValueObjects;

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
