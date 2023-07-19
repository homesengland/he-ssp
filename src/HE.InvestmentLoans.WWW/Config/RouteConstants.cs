namespace HE.InvestmentLoans.WWW.Config;

public static class RouteConstants
{
    public const string Homepage = "/";

    public const string FirstApplicationPage = "/PreApplication/DataPolicy";

    public const string LastApplicationPage = "/CheckYourAnswers";

    public const string EmailProblemPage = "/PersonalDetails/EmailProblem?Applicant={0}";

    public const string ErrorPageNotFound = "/page-not-found";

    public const string ErrorServiceUnavailable = "/service-unavailable";

    public const string ErrorProblemWithTheService = "/problem-with-the-service";
}
