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

    private ExpectedWorksCosts(int value)
        : base(value, FinancialDetailsValidationFieldNames.ExpectedWorksCosts, "expected works costs")
    {
    }

    public static ExpectedWorksCosts FromCrm(decimal value) => new(decimal.ToInt32(value));
}
