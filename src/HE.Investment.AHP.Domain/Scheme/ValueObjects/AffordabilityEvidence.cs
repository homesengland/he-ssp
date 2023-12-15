using HE.Investment.AHP.Domain.Scheme.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.Scheme.ValueObjects;

public class AffordabilityEvidence : ValueObject
{
    public AffordabilityEvidence(string? evidence)
    {
        Build(evidence).CheckErrors();
    }

    public string? Evidence { get; private set; }

    public void CheckIsComplete()
    {
        Build(Evidence, true).CheckErrors();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Evidence;
    }

    private OperationResult Build(string? evidence, bool isCompleteCheck = false)
    {
        var operationResult = OperationResult.New();

        Evidence = Validator
            .For(evidence, "AffordabilityEvidence", "affordability of shared ownership", operationResult)
            .IsProvidedIf(isCompleteCheck)
            .IsLongInput();

        return operationResult;
    }
}
