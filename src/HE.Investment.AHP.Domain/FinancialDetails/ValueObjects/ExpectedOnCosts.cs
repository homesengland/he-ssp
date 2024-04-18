using HE.Investment.AHP.Contract.FinancialDetails.Constants;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;

public class ExpectedOnCosts : TheRequiredIntValueObject
{
    public ExpectedOnCosts(string value)
        : base(value, FinancialDetailsValidationFieldNames.ExpectedOnCosts, "expected on works costs", 0, 999999999)
    {
    }

    private ExpectedOnCosts(int value)
        : base(value, FinancialDetailsValidationFieldNames.ExpectedOnCosts, "expected on works costs")
    {
    }

    public static ExpectedOnCosts FromCrm(decimal value) => new(decimal.ToInt32(value));
}
