using MediatR;

namespace HE.InvestmentLoans.Contract.Organization;
public record SearchOrganizations(string SearchPhrase, int Page, int PageSize) : IRequest<SearchOrganisationResponse>;

public record SearchOrganisationResponse(OrganizationViewModel Result);
