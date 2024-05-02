using HE.Investments.Common.WWW.Utils;

namespace HE.Investments.Common.WWW.Routing;

public abstract class BreadcrumbsBuilderBase
{
    private readonly List<Breadcrumb> _breadcrumbs = [];

    public IList<Breadcrumb> Build()
    {
        return _breadcrumbs;
    }

    protected void AddBreadcrumb(string text, string? action = null, string? controller = null, object? parameters = null, string? cssClass = null)
    {
        _breadcrumbs.Add(new Breadcrumb(text, action, controller, parameters, cssClass));
    }

    protected string GetControllerName(string fullTypeName)
    {
        return new ControllerName(fullTypeName).WithoutPrefix();
    }
}
