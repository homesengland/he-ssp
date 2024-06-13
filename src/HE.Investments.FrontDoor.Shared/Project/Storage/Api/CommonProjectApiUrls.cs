using HE.Investments.Common.Extensions;

namespace HE.Investments.FrontDoor.Shared.Project.Storage.Api;

public static class CommonProjectApiUrls
{
    public const string DeactivateProject = "DeactivateProject";

    public static string GetProject(string projectId) => $"getProject/{projectId.ToGuidAsString()}";

    public static string GetSite(string siteId) => $"getProjectSite/{siteId.ToGuidAsString()}";

    public static string GetSites(string projectId) => $"getProjectSites/{projectId.ToGuidAsString()}";
}
