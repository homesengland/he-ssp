using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Contract.Application;

public record ApplicationDetails(
    AhpApplicationId Id,
    string Name,
    Tenure Tenure,
    ApplicationStatus Status,
    IReadOnlyCollection<AhpApplicationOperation> AllowedOperations)
{
    public bool IsEditable => AllowedOperations.Contains(AhpApplicationOperation.Modification);

    public bool IsReadOnly => !IsEditable;
}
