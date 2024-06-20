using HE.Investments.Common.Extensions;
using Microsoft.AspNetCore.Http;

namespace HE.Investments.FrontDoor.Shared.Project.Storage.Api;

public static class CommonProjectApiUrls
{
    public static string Project(string organisationId, string projectId, string? userGlobalId = null, bool? includeInactive = null)
    {
        var url = $"{organisationId.ToGuidAsString()}/projects/{projectId.ToGuidAsString()}";
        var query = QueryString.Create([
                new KeyValuePair<string, string?>("userId", userGlobalId),
                new KeyValuePair<string, string?>("includeInactive", includeInactive?.ToString())
            ])
            .ToUriComponent();

        return $"{url}{query}";
    }

    public static string DeleteProject(string projectId) => $"projects/{projectId.ToGuidAsString()}";

    public static string Site(string projectId, string siteId) =>
        $"projects/{projectId.ToGuidAsString()}/sites/{siteId.ToGuidAsString()}";

    public static string Sites(string projectId, int pageNumber, int pageSize) =>
        $"projects/{projectId.ToGuidAsString()}/sites?pageNumber={pageNumber}&pageSize={pageSize}";
}
