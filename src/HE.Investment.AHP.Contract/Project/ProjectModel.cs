using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investment.AHP.Contract.Project;

public record ProjectModel(FrontDoorProjectId ProjectId, string ProjectName, IList<SiteProjectModel>? Sites);
