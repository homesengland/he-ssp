using HE.Investments.Common.Contract;
using MediatR;

namespace HE.Investments.AHP.Consortium.Contract.Queries;

public record GetOrganisationConsortiumQuery(OrganisationId OrganisationId, ProgrammeId ProgrammeId) : IRequest<ConsortiumBasicDetails?>;
