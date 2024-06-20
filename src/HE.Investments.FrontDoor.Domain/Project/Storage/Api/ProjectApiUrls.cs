using HE.Investments.Common.Extensions;

namespace HE.Investments.FrontDoor.Domain.Project.Storage.Api;

internal static class ProjectApiUrls
{
    public static string ProjectExists(string organisationId) => $"{organisationId.ToGuidAsString()}/projects/exists";

    public static string Projects(string organisationId, string? userId = null)
    {
        var url = $"{organisationId.ToGuidAsString()}/projects";
        return string.IsNullOrEmpty(userId) ? url : $"{url}?userId={userId}";
    }

    public static string Project(string organisationId, string? projectId = null)
    {
        var url = $"{organisationId.ToGuidAsString()}/projects";
        return string.IsNullOrEmpty(projectId) ? url : $"{url}/{projectId.ToGuidAsString()}";
    }
}
