using System.Reflection;
using HE.Investments.Common.WWW.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace HE.Investment.AHP.WWW.Extensions;

internal sealed class OrganisationUrlHelperDecorator : IUrlHelper
{
    private readonly IUrlHelper _decorated;

    public OrganisationUrlHelperDecorator(IUrlHelper decorated)
    {
        _decorated = decorated;
    }

    public ActionContext ActionContext => _decorated.ActionContext;

    public string? Action(UrlActionContext actionContext)
    {
        if (actionContext.Values != null)
        {
            var dynamicRouteData = new System.Dynamic.ExpandoObject() as IDictionary<string, object>;
            foreach (var property in actionContext.Values.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                dynamicRouteData.Add(property.Name, property.GetValue(actionContext.Values)!);
            }

            if (!dynamicRouteData.ContainsKey("organisationId"))
            {
                var organisationId = ActionContext.RouteData.GetPropertyValue<string>("organisationId");
                if (!string.IsNullOrEmpty(organisationId))
                {
                    dynamicRouteData.Add("organisationId", organisationId);
                }
            }

            actionContext.Values = dynamicRouteData;
        }

        return _decorated.Action(actionContext);
    }

    public string? Content(string? contentPath)
    {
        return _decorated.Content(contentPath);
    }

    public bool IsLocalUrl(string? url)
    {
        return _decorated.IsLocalUrl(url);
    }

    public string? RouteUrl(UrlRouteContext routeContext)
    {
        return _decorated.RouteUrl(routeContext);
    }

    public string? Link(string? routeName, object? values)
    {
        return _decorated.Link(routeName, values);
    }
}

public sealed class OrganisationUrlHelperFactoryDecorator : IUrlHelperFactory
{
    private readonly IUrlHelperFactory _factory;

    public OrganisationUrlHelperFactoryDecorator(IUrlHelperFactory factory)
    {
        _factory = factory;
    }

    public IUrlHelper GetUrlHelper(ActionContext context)
    {
        var urlHelper = _factory.GetUrlHelper(context);
        return new OrganisationUrlHelperDecorator(urlHelper);
    }
}
