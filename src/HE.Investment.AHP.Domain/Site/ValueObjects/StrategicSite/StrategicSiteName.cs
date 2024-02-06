using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.StrategicSite;

public class StrategicSiteName : ShortText
{
    public StrategicSiteName(string? value)
        : base(value, "StrategicSiteName", "strategic site name")
    {
    }
}
