using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.Contract.Organization.ValueObjects;
public class CompaniesHouseNumber : ValueObject
{
    private readonly string _value;

    public CompaniesHouseNumber(string value)
    {
        _value = value;
    }

    public override string? ToString()
    {
        return _value;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return _value;
    }
}
