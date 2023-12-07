using HE.Investments.Common.Domain;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;
using HE.Investments.Loans.Common.Utils.Constants;

namespace HE.Investments.Account.Domain.Organisation.ValueObjects;

public class OrganisationPhoneNumber : ValueObject
{
    public OrganisationPhoneNumber(string? phoneNumber, string? lengthErrorMessage = null)
    {
        Build(phoneNumber, lengthErrorMessage).CheckErrors();
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

    private OperationResult Build(string? phoneNumber, string? lengthErrorMessage)
    {
        var operationResult = OperationResult.New();
        lengthErrorMessage = lengthErrorMessage != null ? ValidationErrorMessage.ShortInputLengthExceeded(lengthErrorMessage) : null;

        PhoneNumber = Validator
            .For(phoneNumber, nameof(PhoneNumber), operationResult)
            .IsProvided(OrganisationErrorMessages.MissingPhoneNumber)
            .IsShortInput(lengthErrorMessage ?? ValidationErrorMessage.ShortInputLengthExceeded("Phone number"));

        return operationResult;
    }
}
