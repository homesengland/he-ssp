using HE.Investments.Common.WWW.Config;

namespace HE.Investments.Account.WWW.Config;

public class AccountExternalLinks : CommonExternalLinks, IAccountExternalLinks
{
    public AccountExternalLinks(IConfiguration configuration)
        : base(configuration)
    {
    }

    public string AccountPrivacyPolicy => GetExternalLink(
        nameof(AccountPrivacyPolicy),
        "https://www.gov.uk/government/publications/homes-england-privacy-notice/homes-england-privacy-notice#home-building-fund-and-affordable-housing-programme");

    public string AhpInvestmentPartner => GetExternalLink(
        nameof(AhpInvestmentPartner),
        "https://www.gov.uk/government/publications/apply-to-be-an-investment-partner-for-the-affordable-homes-programme");
}
