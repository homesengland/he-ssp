using HE.Investments.Organisation.ValueObjects;
using MediatR;

namespace HE.Investments.Organisation.Contract.Queries;

public record GetOrganisationDetailsQuery(OrganisationIdentifier OrganisationIdentifier) : IRequest<OrganisationDetails>;
