namespace HE.Investments.Account.Api.Contract.User;

public record ProfileDetails(
    string? FirstName,
    string? LastName,
    string? JobTitle,
    string? Email,
    string? TelephoneNumber,
    string? SecondaryTelephoneNumber,
    bool? IsTermsAndConditionsAccepted);
