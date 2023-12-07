using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;

public class ExpectedOnCosts : PoundsValueObject
{
    public static readonly UiFields Fields = new(FinancialDetailsValidationFieldNames.ExpectedOnCosts, "The expected on works costs");

    public ExpectedOnCosts(decimal landValue)
        : base(landValue, Fields)
    {
    }

    public ExpectedOnCosts(string landValue)
        : base(landValue, Fields, FinancialDetailsValidationErrors.InvalidExpectedOnCosts)
    {
    }
}
