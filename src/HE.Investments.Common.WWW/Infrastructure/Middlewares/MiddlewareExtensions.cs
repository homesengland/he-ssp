using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;

namespace HE.Investments.Common.WWW.Infrastructure.Middlewares;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UsePageNotFound(this IApplicationBuilder app, string pageNotFoundRoute)
    {
        return app.Use(async (context, next) =>
        {
            await next(context);
            if (context.Response is { StatusCode: 404, HasStarted: false })
            {
                context.Items["originalPath"] = context.Request.Path.Value;
                context.Items["backUrl"] = context.Request.Headers.Referer;
                context.Request.Path = pageNotFoundRoute;
                await next(context);
            }
        });
    }

    public static IApplicationBuilder UseHttps(this IApplicationBuilder app)
    {
        return app.Use((context, next) =>
        {
            context.Request.Scheme = "https";
            return next();
        });
    }

    public static IApplicationBuilder UseCustomDisableRequestLimitSize(this IApplicationBuilder app, params string[] pathsWithDisabledRequestSizeLimit)
    {
        return app.Use((context, next) =>
        {
            var requestPath = context.Request.Path.Value ?? string.Empty;
            if (Array.Exists(pathsWithDisabledRequestSizeLimit, x => requestPath.Contains(x)))
            {
                var httpMaxRequestBodySizeFeature = context.Features.Get<IHttpMaxRequestBodySizeFeature>();
                if (httpMaxRequestBodySizeFeature is not null)
                {
                    httpMaxRequestBodySizeFeature.MaxRequestBodySize = null;
                }
            }

            return next();
        });
    }
}
