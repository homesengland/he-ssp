using HE.Investments.Common.Extensions;
using Microsoft.AspNetCore.Http.Extensions;

namespace HE.Investments.FrontDoor.Shared.Project.Storage.Api;

public static class CommonProjectApiUrls
{
    public static string Project(string organisationId, string projectId, string? userGlobalId = null, bool? includeInactive = null)
    {
        var url = $"{organisationId.ToGuidAsString()}/projects/{projectId.ToGuidAsString()}";
        var query = new QueryBuilder();
        if (!string.IsNullOrEmpty(userGlobalId))
        {
            query.Add("userId", userGlobalId);
        }

        if (includeInactive != null)
        {
            query.Add("includeInactive", includeInactive.Value.ToString().ToLowerInvariant());
        }

        return $"{url}{query.ToQueryString()}";
    }

    public static string DeleteProject(string projectId) => $"projects/{projectId.ToGuidAsString()}";

    public static string Site(string projectId, string siteId) =>
        $"projects/{projectId.ToGuidAsString()}/sites/{siteId.ToGuidAsString()}";

    public static string Sites(string projectId, int pageNumber, int pageSize) =>
        $"projects/{projectId.ToGuidAsString()}/sites?pageNumber={pageNumber}&pageSize={pageSize}";
}
