using HE.Investments.Common.Domain;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.Scheme.ValueObjects;

public class HousingNeeds : ValueObject
{
    public HousingNeeds(string? typeAndTenureJustification, string? schemeAndProposalJustification)
    {
        Build(typeAndTenureJustification, schemeAndProposalJustification).CheckErrors();
    }

    public string TypeAndTenureJustification { get; private set; }

    public string SchemeAndProposalJustification { get; private set; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return TypeAndTenureJustification;
        yield return SchemeAndProposalJustification;
    }

    private OperationResult Build(string? typeAndTenureJustification, string? schemeAndProposalJustification)
    {
        var operationResult = OperationResult.New();

        TypeAndTenureJustification = Validator
            .For(typeAndTenureJustification, nameof(TypeAndTenureJustification), operationResult)
            .IsLongInput();

        SchemeAndProposalJustification = Validator
            .For(schemeAndProposalJustification, nameof(SchemeAndProposalJustification), operationResult)
            .IsLongInput();

        return operationResult;
    }
}
