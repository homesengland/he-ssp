using MediatR;

namespace HE.Investments.Account.Contract.Organisation.Queries;

public record GetOrganisationDetailsQuery : IRequest<GetOrganisationDetailsQueryResponse>;

public record GetOrganisationDetailsQueryResponse(OrganisationDetailsViewModel OrganisationDetailsViewModel);
