using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Contract;
using HE.Investments.Common.Domain;

namespace HE.Investments.Account.Domain.Organisation.ValueObjects;

public class CompaniesHouseNumber : ValueObject
{
    private readonly string _value;

    public CompaniesHouseNumber(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new DomainException("Companies house number cannot be empty", CommonErrorCodes.ValueWasNotProvided);
        }

        _value = value;
    }

    public override string ToString()
    {
        return _value;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return _value;
    }
}
