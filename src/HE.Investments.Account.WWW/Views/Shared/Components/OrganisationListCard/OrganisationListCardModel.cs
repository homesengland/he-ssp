namespace HE.Investments.Account.WWW.Views.Shared.Components.OrganisationListCard;

public record OrganisationListCardModel(
    string Header,
    string HeaderLinkUrl,
    string Street,
    string City,
    string PostalCode,
    string? CompanyHouseNumber);
