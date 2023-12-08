using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;

public class ExpectedWorksCosts : PoundsValueObject
{
    public static readonly UiFields Fields = new(
        FinancialDetailsValidationFieldNames.ExpectedWorksCosts,
        "The expected works costs",
        FinancialDetailsValidationErrors.InvalidExpectedWorksCosts);

    public ExpectedWorksCosts(decimal landValue)
        : base(landValue, Fields)
    {
    }

    public ExpectedWorksCosts(string landValue)
        : base(landValue, Fields)
    {
    }
}
