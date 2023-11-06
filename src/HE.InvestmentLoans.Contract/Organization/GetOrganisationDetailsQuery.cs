using MediatR;

namespace HE.InvestmentLoans.Contract.Organization;

public record GetOrganisationDetailsQuery() : IRequest<GetOrganisationDetailsQueryResponse>;

public record GetOrganisationDetailsQueryResponse(OrganisationDetailsViewModel OrganisationDetailsViewModel);
