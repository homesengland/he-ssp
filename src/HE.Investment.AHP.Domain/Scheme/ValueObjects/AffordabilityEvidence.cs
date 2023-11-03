using HE.InvestmentLoans.BusinessLogic.Organization;
using HE.InvestmentLoans.Common.Validation;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Scheme.ValueObjects;

public class AffordabilityEvidence : ValueObject
{
    public AffordabilityEvidence(string? evidence)
    {
        Build(evidence).CheckErrors();
    }

    public string Evidence { get; private set; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Evidence;
    }

    private OperationResult Build(string? evidence)
    {
        var operationResult = OperationResult.New();

        Evidence = Validator
            .For(evidence, "AffordabilityEvidence", operationResult)
            .IsProvided()
            .IsLongInput();

        return operationResult;
    }
}
