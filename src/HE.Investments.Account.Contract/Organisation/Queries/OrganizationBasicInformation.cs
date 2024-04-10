namespace HE.Investments.Account.Contract.Organisation.Queries;

public record OrganizationBasicInformation(
    string RegisteredCompanyName,
    string CompanyRegistrationNumber,
    Address Address,
    ContactInformation ContactInformation,
    InvestmentPartnerStatus InvestmentPartnerStatus);

public record Address(string Line1, string Line2, string Line3, string City, string PostalCode, string Country);

public record ContactInformation(string PhoneNUmber, string EmailAddress);
