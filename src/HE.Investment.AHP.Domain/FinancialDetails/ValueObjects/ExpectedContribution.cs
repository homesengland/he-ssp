using HE.Investments.Common.Domain;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;

public class ExpectedContribution : PoundsValueObject
{
    public ExpectedContribution(ExpectedContributionFields field, decimal value)
        : base(value, new UiFields(field.ToString()))
    {
        Field = field;
    }

    public ExpectedContribution(ExpectedContributionFields field, string value)
        : base(value, new UiFields(field.ToString()))
    {
        Field = field;
    }

    public ExpectedContributionFields Field { get; }
}
