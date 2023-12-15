using HE.Investments.Common.Domain;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.Scheme.ValueObjects;

public class SalesRisk : ValueObject
{
    public SalesRisk(string? value)
    {
        Build(value).CheckErrors();
    }

    public string? Value { get; private set; }

    public void CheckIsComplete()
    {
        Build(Value, true).CheckErrors();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }

    private OperationResult Build(string? evidence, bool isCompleteCheck = false)
    {
        var operationResult = OperationResult.New();

        Value = Validator
            .For(evidence, nameof(SalesRisk), "sales risk of shared ownership", operationResult)
            .IsProvidedIf(isCompleteCheck)
            .IsLongInput();

        return operationResult;
    }
}
