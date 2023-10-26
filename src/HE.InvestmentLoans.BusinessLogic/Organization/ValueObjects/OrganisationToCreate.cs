using HE.InvestmentLoans.Common.Domain;

namespace HE.InvestmentLoans.BusinessLogic.Organization.ValueObjects;

public class OrganisationToCreate : ValueObject
{
    public OrganisationToCreate(OrganisationName name, OrganisationAddress address)
    {
        Name = name;
        Address = address;
    }

    public OrganisationName Name { get; private set; }

    public OrganisationAddress Address { get; private set; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Name;
        yield return Address;
    }
}
