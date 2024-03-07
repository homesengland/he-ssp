using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investments.FrontDoor.Domain.Site.ValueObjects;

public class SiteName : YourShortText
{
    public SiteName(string value)
        : base(value, "Name", "site name")
    {
    }
}
