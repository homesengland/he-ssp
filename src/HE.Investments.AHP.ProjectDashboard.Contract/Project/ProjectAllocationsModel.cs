using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investments.AHP.ProjectDashboard.Contract.Project;

public record ProjectAllocationsModel(
    FrontDoorProjectId ProjectId,
    string ProjectName,
    string ProgrammeName,
    string OrganisationName,
    List<AllocationProjectModel> Allocations,
    int CurrentPage);
