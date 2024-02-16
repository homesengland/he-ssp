using MediatR;

namespace HE.Investments.Account.Contract.Organisation.Queries;

public record SearchOrganisationsQuery(string SearchPhrase, int Page, int PageSize) : IRequest<SearchOrganisationsQueryResponse>;

public record SearchOrganisationsQueryResponse(OrganisationSearchModel Result);
