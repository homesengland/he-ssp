using HE.Investments.Common.Contract;
using Microsoft.AspNetCore.Http;

namespace HE.Investments.Common.WWW.Extensions;

public static class HttpContextExtensions
{
    public static OrganisationId? GetOrganisationIdFromRoute(this HttpContext request)
    {
        var organisationId = request.Request.GetRouteValue("organisationId");
        return organisationId != null ? OrganisationId.From(organisationId) : null;
    }
}
