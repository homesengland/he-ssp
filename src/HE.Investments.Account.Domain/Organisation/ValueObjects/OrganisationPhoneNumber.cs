using HE.Investments.Common.Domain;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.Investments.Account.Domain.Organisation.ValueObjects;

public class OrganisationPhoneNumber : ValueObject
{
    public OrganisationPhoneNumber(string? phoneNumber, string? fieldName = null)
    {
        Build(phoneNumber, fieldName).CheckErrors();
    }

    public string PhoneNumber { get; private set; }

    public override string ToString()
    {
        return PhoneNumber;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return PhoneNumber;
    }

    private OperationResult Build(string? phoneNumber, string? fieldName)
    {
        var operationResult = OperationResult.New();
        var errorMessage = fieldName != null ? ValidationErrorMessage.ShortInputLengthExceeded(fieldName) : null;

        PhoneNumber = Validator
            .For(phoneNumber, nameof(PhoneNumber), "Phone number", operationResult)
            .IsProvided(OrganisationErrorMessages.MissingPhoneNumber)
            .IsShortInput(errorMessage);

        return operationResult;
    }
}
