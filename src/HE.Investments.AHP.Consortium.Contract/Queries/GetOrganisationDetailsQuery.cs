using HE.Investments.Common.Contract;
using MediatR;

namespace HE.Investments.AHP.Consortium.Contract.Queries;

public record GetOrganisationDetailsQuery(OrganisationId OrganisationId) : IRequest<OrganisationDetails>;
