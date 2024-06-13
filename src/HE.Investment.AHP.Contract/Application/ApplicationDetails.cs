using HE.Investments.Common.Contract;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investment.AHP.Contract.Application;

public record ApplicationDetails(
    FrontDoorProjectId ProjectId,
    AhpApplicationId Id,
    string Name,
    Tenure Tenure,
    ApplicationStatus Status,
    IReadOnlyCollection<AhpApplicationOperation> AllowedOperations)
{
    public bool IsEditable => AllowedOperations.Contains(AhpApplicationOperation.Modification);

    public bool IsReadOnly => !IsEditable;
}
