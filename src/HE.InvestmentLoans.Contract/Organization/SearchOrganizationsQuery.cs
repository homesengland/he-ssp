using MediatR;

namespace HE.InvestmentLoans.Contract.Organization;
public record SearchOrganizationsQuery(string SearchPhrase, int Page, int PageSize) : IRequest<SearchOrganisationsQueryResponse>;

public record SearchOrganisationsQueryResponse(OrganizationViewModel Result);
