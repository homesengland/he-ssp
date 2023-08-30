using Dawn;
using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;

public class OrganisationMoreInformation : ValueObject
{
    public OrganisationMoreInformation(string information)
    {
        Information = Guard.Argument(information.Trim(), nameof(Information)).NotEmpty();
    }

    public string Information { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Information;
    }
}
