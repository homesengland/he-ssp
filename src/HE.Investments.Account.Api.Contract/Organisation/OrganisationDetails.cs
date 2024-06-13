namespace HE.Investments.Account.Api.Contract.Organisation;

public record OrganisationDetails(
    string OrganisationId,
    string CompanyRegisteredName,
    string CompanyRegistrationNumber,
    string CompanyAddressLine1,
    string City,
    string PostalCode,
    bool IsUnregisteredBody);
