namespace HE.Investments.Account.WWW.Middlewares;

internal sealed class PageNotFoundMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        await next(context);
        if (context.Response is { StatusCode: 404, HasStarted: false })
        {
            context.Items["originalPath"] = context.Request.Path.Value;
            context.Items["backUrl"] = context.Request.Headers["Referer"];
            context.Request.Path = "/home/page-not-found";
            await next(context);
        }
    }
}
