using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Messages;

namespace HE.Investments.FrontDoor.Domain.Site.ValueObjects;

public class PlanningStatus : ValueObject, IQuestion
{
    public PlanningStatus(SitePlanningStatus? value)
    {
        if (value is null or SitePlanningStatus.Undefined)
        {
            OperationResult.ThrowValidationError(nameof(PlanningStatus), ValidationErrorMessage.MustBeSelected("planning status of the site"));
        }

        Value = value!.Value;
    }

    private PlanningStatus()
    {
        Value = SitePlanningStatus.Undefined;
    }

    public SitePlanningStatus Value { get; }

    public static PlanningStatus Create(SitePlanningStatus? value) => value != null ? new PlanningStatus(value.Value) : Empty();

    public static PlanningStatus Empty() => new();

    public bool IsAnswered()
    {
        return Value != SitePlanningStatus.Undefined;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
