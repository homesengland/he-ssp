using HE.InvestmentLoans.Common.Domain;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Validation;

namespace HE.InvestmentLoans.BusinessLogic.Organization.ValueObjects;

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
            .IsShortInput(lengthErrorMessage);

        return operationResult;
    }
}
