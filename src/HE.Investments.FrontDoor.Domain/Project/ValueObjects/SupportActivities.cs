using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.FrontDoor.Shared.Project.Contract;

namespace HE.Investments.FrontDoor.Domain.Project.ValueObjects;

public class SupportActivities : ValueObject, IQuestion
{
    private const string UiFieldName = "SupportActivityTypes";

    private const string InvalidValueErrorMessage = "Select activities you require support for, or select ‘other'";

    public SupportActivities(IList<SupportActivityType> values)
    {
        if (values.Count == 0)
        {
            OperationResult.ThrowValidationError(UiFieldName, InvalidValueErrorMessage);
        }

        var distinctValues = values.Distinct().ToList();
        if (distinctValues.Contains(SupportActivityType.Other) && values.Count > 1)
        {
            OperationResult.ThrowValidationError(UiFieldName, InvalidValueErrorMessage);
        }

        Values = distinctValues;
    }

    private SupportActivities()
    {
        Values = new List<SupportActivityType>();
    }

    public IList<SupportActivityType> Values { get; }

    public static SupportActivities Empty() => new();

    public bool IsTenureRequired() => Values.Count == 1 && Values.Contains(SupportActivityType.DevelopingHomes);

    public bool IsInfrastructureRequired() => Values.Count == 1 && Values.Contains(SupportActivityType.ProvidingInfrastructure);

    public bool IsAnswered() => Values.Count > 0;

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return string.Join(',', Values.OrderBy(x => x));
    }
}
