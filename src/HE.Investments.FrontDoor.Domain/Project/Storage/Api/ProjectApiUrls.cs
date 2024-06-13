using HE.Investments.Common.Extensions;

namespace HE.Investments.FrontDoor.Domain.Project.Storage.Api;

internal static class ProjectApiUrls
{
    public const string IsThereProjectWithName = "CheckProjectExists";

    public const string SaveProject = "UpsertProject";

    public static string GetProject(string projectId) => $"getProject/{projectId.ToGuidAsString()}";

    public static string GetProjects(string organisationId) => $"getProjects/{organisationId.ToGuidAsString()}";
}
