using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Planning;

public class LandRegistryTitleNumber : ShortText
{
    public LandRegistryTitleNumber(string? value)
        : base(value, "LandRegistryTitleNumber", "Land Registry title number")
    {
    }
}
