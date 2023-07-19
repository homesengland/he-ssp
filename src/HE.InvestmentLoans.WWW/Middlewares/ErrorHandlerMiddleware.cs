using System.Diagnostics.CodeAnalysis;
using System.Net;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.WWW.Config;

namespace HE.InvestmentLoans.WWW.Middlewares;

[SuppressMessage("Design", "CA1031", Justification = "Need to for the errors handling")]
public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);

            if (context.Response.StatusCode == (int)HttpStatusCode.NotFound)
            {
                context.Response.Redirect(RouteConstants.ErrorPageNotFound);
            }
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case LoanUserAccountIsMissingException:
                case NotFoundException:
                    context.Response.Redirect(RouteConstants.ErrorPageNotFound);
                    break;
                case DeleteFailureException:
                case ValidationException:
                    context.Response.Redirect(RouteConstants.ErrorProblemWithTheService);
                    break;
                default:
                    context.Response.Redirect(RouteConstants.ErrorServiceUnavailable);
                    break;
            }
        }
    }
}
