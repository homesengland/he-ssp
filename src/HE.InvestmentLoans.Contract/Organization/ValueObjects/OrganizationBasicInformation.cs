namespace HE.InvestmentLoans.Contract.Organization.ValueObjects;

public record OrganizationBasicInformation(string RegisteredCompanyName, string CompanyRegistrationNumber, Address Address);

public record Address(string Line1, string Line2, string Line3, string City, string PostalCode, string Country);
