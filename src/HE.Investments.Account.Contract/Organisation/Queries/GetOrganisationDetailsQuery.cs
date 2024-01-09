using MediatR;

namespace HE.Investments.Account.Contract.Organisation.Queries;

public class GetOrganisationDetailsQuery : IRequest<GetOrganisationDetailsQueryResponse>;

public record GetOrganisationDetailsQueryResponse(OrganisationDetailsViewModel OrganisationDetailsViewModel);
