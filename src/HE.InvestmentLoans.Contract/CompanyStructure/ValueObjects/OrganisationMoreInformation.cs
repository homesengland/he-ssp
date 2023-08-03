using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;

public class OrganisationMoreInformation : ValueObject
{
    public string Information { get; private set; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Information;
    }
}
