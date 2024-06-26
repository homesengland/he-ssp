using HE.Investment.AHP.Contract.FinancialDetails.Constants;
using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;

public class ExpectedWorksCosts : TheRequiredIntValueObject
{
    public ExpectedWorksCosts(string value)
        : base(value, FinancialDetailsValidationFieldNames.ExpectedWorksCosts, "expected works costs", 0, 999999999, MessageOptions.Money)
    {
    }

    public ExpectedWorksCosts(decimal value)
        : base(decimal.ToInt32(value), FinancialDetailsValidationFieldNames.ExpectedWorksCosts, "expected works costs")
    {
    }
}
