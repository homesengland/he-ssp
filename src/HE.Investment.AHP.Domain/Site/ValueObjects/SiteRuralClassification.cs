using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Site.ValueObjects;

public class SiteRuralClassification : ValueObject, IQuestion
{
    public SiteRuralClassification(bool? isWithinRuralSettlement = null, bool? isRuralExceptionSite = null)
    {
        IsWithinRuralSettlement = isWithinRuralSettlement;
        IsRuralExceptionSite = isRuralExceptionSite;
    }

    public bool? IsWithinRuralSettlement { get; }

    public bool? IsRuralExceptionSite { get; }

    public bool IsAnswered()
    {
        return IsWithinRuralSettlement.IsProvided() && IsRuralExceptionSite.IsProvided();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return IsWithinRuralSettlement;
        yield return IsRuralExceptionSite;
    }
}
