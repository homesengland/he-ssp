using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Messages;
using HE.Investments.FrontDoor.Contract.Project.Enums;

namespace HE.Investments.FrontDoor.Domain.Project.ValueObjects;

public class Regions : ValueObject, IQuestion
{
    private const string DisplayName = "region your project will be located in";

    public Regions(IList<RegionType> values)
    {
        if (values.Count == 0)
        {
            OperationResult.ThrowValidationError(nameof(Regions), ValidationErrorMessage.MustBeSelected(DisplayName));
        }

        Values = values.Distinct().ToList();
    }

    private Regions()
    {
        Values = new List<RegionType>();
    }

    public IList<RegionType> Values { get; }

    public static Regions Empty() => new();

    public bool IsAnswered() => Values.Count > 0;

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return string.Join(',', Values.OrderBy(x => x));
    }
}
