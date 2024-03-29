using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;

public class ExpectedContributionValue : PoundsValueObject
{
    public ExpectedContributionValue(ExpectedContributionFields field, decimal value)
        : base(value, new UiFields(field.ToString(), InvalidValueValidationMessage: FinancialDetailsValidationErrors.GenericAmountValidationError))
    {
        Field = field;
    }

    public ExpectedContributionValue(ExpectedContributionFields field, string value)
        : base(value, new UiFields(field.ToString(), InvalidValueValidationMessage: FinancialDetailsValidationErrors.GenericAmountValidationError))
    {
        Field = field;
    }

    public ExpectedContributionFields Field { get; }
}
