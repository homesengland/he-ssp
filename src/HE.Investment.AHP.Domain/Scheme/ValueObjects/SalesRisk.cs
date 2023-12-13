using HE.Investment.AHP.Domain.Scheme.Constants;
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
            .For(evidence, SchemeValidationFieldNames.SalesRisk, operationResult)
            .IsProvidedIf(isCompleteCheck, "Sales risk of shared ownership is missing")
            .IsLongInput();

        return operationResult;
    }
}
