using HE.Investments.Common.WWW.Infrastructure.ErrorHandling;

namespace HE.Investments.Account.WWW.Routing;

public class AccountErrorViewPaths : IErrorViewPaths
{
    public string PageNotFoundViewPath => "/Views/Home/PageNotFound.cshtml";

    public string PageNotFoundRoute => "/home/page-not-found";

    public string ProblemWithTheServiceViewPath => "/Views/Home/Error.cshtml";

    public string ErrorRoute => "/home/error";
}
