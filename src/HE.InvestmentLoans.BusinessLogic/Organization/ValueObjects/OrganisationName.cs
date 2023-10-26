using HE.InvestmentLoans.Common.Domain;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Validation;

namespace HE.InvestmentLoans.BusinessLogic.Organization.ValueObjects;

public class OrganisationName : ValueObject
{
    public OrganisationName(string name)
    {
        Build(name).CheckErrors();
    }

    public string Value { get; private set; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }

    private OperationResult Build(string name)
    {
        var operationResult = OperationResult.New();

        Value = Validator
            .For(name, nameof(Value), operationResult)
            .IsProvided(OrganisationErrorMessages.MissingOrganisationName)
            .IsShortInput();

        return operationResult;
    }
}
