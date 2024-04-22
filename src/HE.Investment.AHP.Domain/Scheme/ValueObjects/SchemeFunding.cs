using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Scheme.ValueObjects;

public class SchemeFunding : ValueObject, IQuestion
{
    private readonly RequiredFunding? _requiredFunding;

    private readonly HousesToDeliver? _housesToDeliver;

    public SchemeFunding(string? requiredFunding, string? housesToDeliver)
    {
        var operationResult = new OperationResult();
        _requiredFunding = operationResult.Aggregate(() => new RequiredFunding(requiredFunding));
        _housesToDeliver = operationResult.Aggregate(() => new HousesToDeliver(housesToDeliver));

        operationResult.CheckErrors();
    }

    public SchemeFunding(int? requiredFunding, int? housesToDeliver)
    {
        _requiredFunding = requiredFunding.IsProvided() ? new RequiredFunding(requiredFunding!.Value) : null;
        _housesToDeliver = housesToDeliver.IsProvided() ? new HousesToDeliver(housesToDeliver!.Value) : null;
    }

    public int? RequiredFunding => _requiredFunding?.Value;

    public int? HousesToDeliver => _housesToDeliver?.Value;

    public static SchemeFunding Empty() => new((int?)null, null);

    public void CheckIsComplete()
    {
        var operationResult = new OperationResult();
        operationResult.Aggregate(() => new RequiredFunding(_requiredFunding?.ToString()));
        operationResult.Aggregate(() => new HousesToDeliver(_housesToDeliver?.ToString()));

        operationResult.CheckErrors();
    }

    public bool IsAnswered() => RequiredFunding.IsProvided() && HousesToDeliver.IsProvided();

    public bool IsNotAnswered() => !IsAnswered();

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return RequiredFunding;
        yield return HousesToDeliver;
    }
}
