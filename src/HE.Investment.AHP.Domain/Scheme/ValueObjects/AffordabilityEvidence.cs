using HE.Investments.Account.Domain.Organisation;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Validators;

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
            .IsLongInput();

        return operationResult;
    }
}
