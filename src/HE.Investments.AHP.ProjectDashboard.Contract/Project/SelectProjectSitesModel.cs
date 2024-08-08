namespace HE.Investments.AHP.ProjectDashboard.Contract.Project;

public record SelectProjectSitesModel(
    ProjectSitesModel ProjectSites,
    string? CallbackUrl);
