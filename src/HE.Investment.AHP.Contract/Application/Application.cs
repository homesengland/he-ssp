using HE.Investments.Common.Contract;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investment.AHP.Contract.Application;

public record Application(
    AhpApplicationId Id,
    FrontDoorProjectId ProjectId,
    string Name,
    Tenure Tenure,
    ApplicationStatus Status,
    IReadOnlyCollection<AhpApplicationOperation> AllowedOperations,
    string? ReferenceNumber,
    ModificationDetails? LastModificationDetails,
    ModificationDetails? LastSubmissionDetails,
    IList<ApplicationSection> Sections)
{
    public bool IsEditable => AllowedOperations.Contains(AhpApplicationOperation.Modification);

    public bool IsReadOnly => !IsEditable;
}
