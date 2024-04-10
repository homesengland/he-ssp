using HE.Investments.Common.WWW.Config;

namespace HE.Investments.FrontDoor.WWW.Config;

public class FrontDoorExternalLinks : CommonExternalLinks, IFrontDoorExternalLinks
{
    public FrontDoorExternalLinks(IConfiguration configuration)
        : base(configuration)
    {
    }

    public string EnquiriesEmail => GetExternalLink(
        nameof(EnquiriesEmail),
        "enquiries@homesengland.gov.uk");

    public string FindLocalCouncil => GetExternalLink(
        nameof(FindLocalCouncil),
        "https://www.gov.uk/find-local-council");
}
