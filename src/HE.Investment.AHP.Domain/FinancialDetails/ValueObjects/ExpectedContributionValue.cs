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

    private ExpectedContributionValue(ExpectedContributionFields field, int value)
        : base(value, field.ToString(), field.GetDescription())
    {
        Field = field;
    }

    public ExpectedContributionFields Field { get; }

    public static ExpectedContributionValue FromCrm(ExpectedContributionFields field, decimal value) => new(field, decimal.ToInt32(value));
}
