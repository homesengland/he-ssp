namespace HE.Investments.Common.WWW.Infrastructure.ErrorHandling;

public interface IErrorViewPaths
{
    string PageNotFoundViewPath { get; }

    string PageNotFoundRoute { get; }

    string ProblemWithTheServiceViewPath { get; }

    string ErrorRoute { get; }
}
