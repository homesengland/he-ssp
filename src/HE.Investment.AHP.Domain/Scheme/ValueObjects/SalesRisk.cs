using HE.InvestmentLoans.BusinessLogic.Organization;
using HE.InvestmentLoans.Common.Validation;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Scheme.ValueObjects;

public class SalesRisk : ValueObject
{
    public SalesRisk(string? value)
    {
        Build(value).CheckErrors();
    }

    public string Value { get; private set; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }

    private OperationResult Build(string? evidence)
    {
        var operationResult = OperationResult.New();

        Value = Validator
            .For(evidence, nameof(SalesRisk), operationResult)
            .IsProvided()
            .IsLongInput();

        return operationResult;
    }
}
