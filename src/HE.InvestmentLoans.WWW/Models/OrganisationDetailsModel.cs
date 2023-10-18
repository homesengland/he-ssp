namespace HE.InvestmentLoans.WWW.Models;

public record OrganisationDetailsModel(
    string OrganisationName,
    string OrganisationPhoneNumber,
    IEnumerable<string> OrganisationAddress,
    string CompaniesHouseNumber,
    string CurrentOrganisationDataState);
