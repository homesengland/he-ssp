using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.FinancialDetails.Entities;

public class OtherApplicationCosts
{
    public OtherApplicationCosts(ExpectedWorksCosts? expectedWorksCosts, ExpectedOnCosts? expectedOnCosts)
    {
        ExpectedWorksCosts = expectedWorksCosts;
        ExpectedOnCosts = expectedOnCosts;
    }

    public ExpectedWorksCosts? ExpectedWorksCosts { get; private set; }

    public ExpectedOnCosts? ExpectedOnCosts { get; private set; }

    public decimal ExpectedTotalCosts() => (ExpectedWorksCosts?.Value ?? 0) + (ExpectedOnCosts?.Value ?? 0);

    public bool IsAnswered()
    {
        return ExpectedWorksCosts.IsProvided() && ExpectedOnCosts.IsProvided();
    }
}
