using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.FrontDoor.Contract.Project.Enums;

namespace HE.Investments.FrontDoor.Domain.Project.ValueObjects;

public class ProjectAffordableHomesAmount : ValueObject, IQuestion
{
    public ProjectAffordableHomesAmount(AffordableHomesAmount affordableHomesAmount)
    {
        if (affordableHomesAmount == AffordableHomesAmount.Undefined)
        {
            OperationResult.ThrowValidationError(nameof(AffordableHomesAmount), ValidationErrorMessage.MustBeSelected("amount of affordable homes you are planning to deliver"));
        }

        AffordableHomesAmount = affordableHomesAmount;
    }

    private ProjectAffordableHomesAmount()
    {
        AffordableHomesAmount = AffordableHomesAmount.Undefined;
    }

    public AffordableHomesAmount AffordableHomesAmount { get; }

    public static ProjectAffordableHomesAmount Empty() => new();

    public static ProjectAffordableHomesAmount? Create(AffordableHomesAmount? affordableHomesAmount) =>
        affordableHomesAmount.IsProvided() ? new ProjectAffordableHomesAmount(affordableHomesAmount!.Value) : null;

    public bool IsAnswered()
    {
        return AffordableHomesAmount != AffordableHomesAmount.Undefined;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return AffordableHomesAmount;
    }
}
