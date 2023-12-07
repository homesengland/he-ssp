using HE.Investments.Common.Domain;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;
using HE.Investments.Loans.Common.Utils.Constants;

namespace HE.Investments.Account.Domain.Organisation.ValueObjects;

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
            .IsShortInput(lengthErrorMessage ?? ValidationErrorMessage.ShortInputLengthExceeded("Organisation name"));

        return operationResult;
    }
}
