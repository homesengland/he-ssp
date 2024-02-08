using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Site.ValueObjects;

public class SiteTypeDetails : ValueObject, IQuestion
{
    public SiteTypeDetails(SiteType? siteType = null, bool? isOnGreenBelt = null, bool? isRegenerationSite = null)
    {
        SiteType = siteType;
        IsOnGreenBelt = isOnGreenBelt;
        IsRegenerationSite = isRegenerationSite;
    }

    public SiteType? SiteType { get; }

    public bool? IsOnGreenBelt { get; }

    public bool? IsRegenerationSite { get; }

    public bool IsAnswered()
    {
        return SiteType.IsProvided() && IsOnGreenBelt.IsProvided() && IsRegenerationSite.IsProvided();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return SiteType;
        yield return IsOnGreenBelt;
        yield return IsRegenerationSite;
    }
}
