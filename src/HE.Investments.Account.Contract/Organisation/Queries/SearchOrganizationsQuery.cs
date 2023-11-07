using MediatR;

namespace HE.Investments.Account.Contract.Organisation.Queries;

public record SearchOrganizationsQuery(string SearchPhrase, int Page, int PageSize) : IRequest<SearchOrganisationsQueryResponse>;

public record SearchOrganisationsQueryResponse(OrganisationSearchModel Result);
