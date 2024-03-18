namespace HE.Investments.Account.Shared.Routing;

public static class UserAccountEndpoints
{
    public const string Controller = "user";

    public const string ProfileDetails = $"{Controller}/{ProfileDetailsSuffix}";

    public const string ProfileDetailsSuffix = "profile-details";

    public const string InformationAboutYourAccount = "information-about-your-account";
}
