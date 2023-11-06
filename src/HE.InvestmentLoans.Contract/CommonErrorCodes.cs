namespace HE.InvestmentLoans.Contract;

public static class CommonErrorCodes
{
    public const string IdCannotBeModified = nameof(IdCannotBeModified);

    public const string ValueWasNotProvided = nameof(ValueWasNotProvided);

    public const string LoanApplicationNotReadyToSubmit = nameof(LoanApplicationNotReadyToSubmit);

    public const string ApplicationHasBeenSubmitted = nameof(ApplicationHasBeenSubmitted);

    public const string ContactAlreadyLinkedWithOrganization = nameof(ContactAlreadyLinkedWithOrganization);

    public const string ContactIsNotLinkedWithRequestedOrganization = nameof(ContactIsNotLinkedWithRequestedOrganization);

    public const string LoanApplicationCannotBeWithdrawn = nameof(LoanApplicationCannotBeWithdrawn);

    public const string IncorrectOrganisationId = nameof(IncorrectOrganisationId);
}
