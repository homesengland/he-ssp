using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Validation;
using HE.Investments.Common.Domain;

namespace HE.InvestmentLoans.BusinessLogic.Organization.ValueObjects;

public class OrganisationName : ValueObject
{
    public OrganisationName(
        string? name,
        string notProvidedErrorMessage = OrganisationErrorMessages.MissingOrganisationName,
        string? lengthErrorMessage = null)
    {
        Build(name, notProvidedErrorMessage, lengthErrorMessage).CheckErrors();
    }

    public string Name { get; private set; }

    public override string ToString()
    {
        return Name;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Name;
    }

    private OperationResult Build(string? name, string? notProvidedErrorMessage, string? lengthErrorMessage)
    {
        var operationResult = OperationResult.New();
        lengthErrorMessage = lengthErrorMessage != null ? ValidationErrorMessage.ShortInputLengthExceeded(lengthErrorMessage) : null;

        Name = Validator
            .For(name, nameof(Name), operationResult)
            .IsProvided(notProvidedErrorMessage)
            .IsShortInput(lengthErrorMessage);

        return operationResult;
    }
}
