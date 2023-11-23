using Microsoft.AspNetCore.Routing;

namespace HE.Investments.Common.WWWTestsFramework.Framework;

internal sealed class CustomRouter : IRouter
{
    public VirtualPathData? GetVirtualPath(VirtualPathContext context)
    {
        return null;
    }

    public Task RouteAsync(RouteContext context)
    {
        return Task.CompletedTask;
    }
}
