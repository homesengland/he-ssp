using HE.Investments.Common.Domain;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.Investments.Account.Domain.Organisation.ValueObjects;

public class OrganisationName : ValueObject
{
    public OrganisationName(
        string? name,
        string notProvidedErrorMessage = OrganisationErrorMessages.MissingOrganisationName,
        string? fieldName = null)
    {
        Build(name, notProvidedErrorMessage, fieldName).CheckErrors();
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

    private OperationResult Build(string? name, string? notProvidedErrorMessage, string? fieldName)
    {
        var operationResult = OperationResult.New();
        var errorMessage = fieldName != null ? ValidationErrorMessage.ShortInputLengthExceeded(fieldName) : null;

        Name = Validator
            .For(name, nameof(Name), "Organisation name", operationResult)
            .IsProvided(notProvidedErrorMessage)
            .IsShortInput(errorMessage);

        return operationResult;
    }
}
