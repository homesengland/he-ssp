using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investments.AHP.ProjectDashboard.Contract.Project;

public record ProjectModel(FrontDoorProjectId ProjectId, string ProjectName, IList<SiteProjectModel>? Sites);
