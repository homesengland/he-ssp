using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;

public class ExpectedContributionValue : TheRequiredIntValueObject
{
    public ExpectedContributionValue(ExpectedContributionFields field, string value)
    : base(value, field.ToString(), "expected contribution value", 0, 999999999, MessageOptions.IsMoney)
    {
        Field = field;
    }

    private ExpectedContributionValue(ExpectedContributionFields field, int value)
        : base(value, field.ToString(), "expected contribution value")
    {
        Field = field;
    }

    public ExpectedContributionFields Field { get; }

    public static ExpectedContributionValue FromCrm(ExpectedContributionFields field, decimal value) => new(field, decimal.ToInt32(value));
}
