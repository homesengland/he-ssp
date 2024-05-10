using HE.Investment.AHP.Contract.FinancialDetails.Constants;
using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;

public class ExpectedOnCosts : TheRequiredIntValueObject
{
    public ExpectedOnCosts(string value)
        : base(value, FinancialDetailsValidationFieldNames.ExpectedOnCosts, "expected on costs", 0, 999999999, MessageOptions.Money)
    {
    }

    public ExpectedOnCosts(decimal value)
        : base(decimal.ToInt32(value), FinancialDetailsValidationFieldNames.ExpectedOnCosts, "expected on costs")
    {
    }
}
