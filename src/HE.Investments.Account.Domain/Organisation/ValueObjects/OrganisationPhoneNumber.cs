using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.Investments.Account.Domain.Organisation.ValueObjects;

public class OrganisationPhoneNumber : ValueObject
{
    public OrganisationPhoneNumber(string? phoneNumber)
    {
        Build(phoneNumber).CheckErrors();
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

    private OperationResult Build(string? phoneNumber)
    {
        var operationResult = OperationResult.New();

        PhoneNumber = Validator
            .For(phoneNumber, nameof(PhoneNumber), "Phone number", operationResult)
            .IsProvided(OrganisationErrorMessages.MissingPhoneNumber)
            .IsShortInput();

        return operationResult;
    }
}
