namespace HE.Investments.AHP.IntegrationTests.ApplicationManagement.Data.Model;

public record AhpProject(string ProjectId, string ProjectName, AhpSite[] Sites);

public record AhpSite(string SiteId, string SiteName, AhpApplication[] Applications);

public record AhpApplication(string ApplicationId, string ApplicationName);
