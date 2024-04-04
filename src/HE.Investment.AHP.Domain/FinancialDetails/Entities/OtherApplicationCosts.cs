using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.FinancialDetails.Entities;

public class OtherApplicationCosts : ValueObject, IQuestion
{
    public OtherApplicationCosts(ExpectedWorksCosts? expectedWorksCosts, ExpectedOnCosts? expectedOnCosts)
    {
        ExpectedWorksCosts = expectedWorksCosts;
        ExpectedOnCosts = expectedOnCosts;
    }

    public OtherApplicationCosts()
    {
    }

    public ExpectedWorksCosts? ExpectedWorksCosts { get; }

    public ExpectedOnCosts? ExpectedOnCosts { get; }

    public decimal? ExpectedTotalCosts()
    {
        if (AreAllNotAnswered())
        {
            return null;
        }

        return (ExpectedWorksCosts?.Value ?? 0) + (ExpectedOnCosts?.Value ?? 0);
    }

    public bool IsAnswered()
    {
        return ExpectedWorksCosts.IsProvided() && ExpectedOnCosts.IsProvided();
    }

    public bool AreAllNotAnswered()
    {
        return ExpectedWorksCosts.IsNotProvided() && ExpectedOnCosts.IsNotProvided();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return ExpectedWorksCosts;
        yield return ExpectedOnCosts;
    }
}
