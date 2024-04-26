using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;

public class ExpectedContributionValue : TheRequiredIntValueObject
{
    public ExpectedContributionValue(ExpectedContributionFields field, string value)
    : base(value, field.ToString(), field.GetDescription(), 0, 999999999, MessageOptions.Money)
    {
        Field = field;
    }

    public ExpectedContributionValue(ExpectedContributionFields field, decimal value)
        : base(decimal.ToInt32(value), field.ToString(), field.GetDescription())
    {
        Field = field;
    }

    public ExpectedContributionFields Field { get; }
}
