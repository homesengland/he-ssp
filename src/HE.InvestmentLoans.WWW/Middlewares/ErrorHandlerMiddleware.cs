using System.Diagnostics.CodeAnalysis;
using System.Net;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Models.Error;
using HE.InvestmentLoans.Common.Services.Interfaces;
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
            var hostEnvironment = context.RequestServices.GetRequiredService<IHostEnvironment>();
            var cache = context.RequestServices.GetRequiredService<ICacheService>();

            var key = ex switch
            {
                NotFoundException => $"{RouteConstants.ErrorPageNotFound}?key={Guid.NewGuid()}",
                _ => $"{RouteConstants.ErrorProblemWithTheService}?key={Guid.NewGuid()}",
            };

            var error = hostEnvironment.IsDevelopment() ? new ErrorModel(ex.Message, ex.StackTrace) : new ErrorModel(ex.Message);
            cache.SetValue(key, error, 600);
            context.Response.Redirect(key);
        }
    }
}
