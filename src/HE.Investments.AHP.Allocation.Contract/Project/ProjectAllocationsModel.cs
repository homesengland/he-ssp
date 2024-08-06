using HE.Investment.AHP.Contract.Project;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investments.AHP.Allocation.Contract.Project;

public record ProjectAllocationsModel(
    FrontDoorProjectId ProjectId,
    string ProjectName,
    string ProgrammeName,
    string OrganisationName,
    List<AllocationProjectModel> Allocations,
    int CurrentPage);
