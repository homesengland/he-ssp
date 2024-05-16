namespace HE.Investment.AHP.Contract.Project;

public record ProjectModel(AhpProjectId ProjectId, string ProjectName, IList<SiteProjectModel> Sites);
