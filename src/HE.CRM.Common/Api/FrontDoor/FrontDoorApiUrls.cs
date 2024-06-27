using System;

namespace HE.CRM.Common.Api.FrontDoor
{
    internal static class FrontDoorApiUrls
    {
        public const string SaveSite = "upsertProjectSite";

        public const string RemoveSite = "DeactivateProjectSite";

        public const string CheckProjectExists = "CheckProjectExists";

        public const string SaveProject = "UpsertProject";

        public static string GetProjects(Guid organisationId) => $"getProjects/{organisationId.ToString()}";

        public const string DeactivateProject = "DeactivateProject";

        public static string GetProject(Guid projectId) => $"getProject/{projectId.ToString()}";

        public static string GetSite(Guid siteId) => $"getProjectSite/{siteId.ToString()}";

        public static string GetSites(Guid projectId) => $"getProjectSites/{projectId.ToString()}";
    }
}
