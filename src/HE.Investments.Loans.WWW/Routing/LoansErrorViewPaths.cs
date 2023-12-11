using HE.Investments.Common.WWW.Infrastructure.ErrorHandling;

namespace HE.Investments.Loans.WWW.Routing;

public class LoansErrorViewPaths : IErrorViewPaths
{
    public string PageNotFoundViewPath => "Errors/PageNotFound";

    public string PageNotFoundRoute => PageNotFoundViewPath;

    public string ProblemWithTheServiceViewPath => "Errors/ProblemWithTheService";

    public string ErrorRoute => ProblemWithTheServiceViewPath;
}
