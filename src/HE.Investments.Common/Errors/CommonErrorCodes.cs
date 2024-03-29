namespace HE.Investments.Common.Errors;

public static class CommonErrorCodes
{
    public const string IdCannotBeModified = nameof(IdCannotBeModified);

    public const string ValueWasNotProvided = nameof(ValueWasNotProvided);

    public const string LoanApplicationNotReadyToSubmit = nameof(LoanApplicationNotReadyToSubmit);

    public const string ApplicationHasBeenSubmitted = nameof(ApplicationHasBeenSubmitted);

    public const string ContactAlreadyLinkedWithOrganization = nameof(ContactAlreadyLinkedWithOrganization);

    public const string ContactIsNotLinkedWithRequestedOrganization = nameof(ContactIsNotLinkedWithRequestedOrganization);

    public const string ApplicationCannotBeWithdrawn = nameof(ApplicationCannotBeWithdrawn);

    public const string IncorrectOrganisationId = nameof(IncorrectOrganisationId);

    public const string InvalidDomainOperation = nameof(InvalidDomainOperation);
}
