using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Site.ValueObjects;

public class SiteName : ShortText
{
    public SiteName(string? value)
        : base(value, "Name", "name")
    {
    }
}
