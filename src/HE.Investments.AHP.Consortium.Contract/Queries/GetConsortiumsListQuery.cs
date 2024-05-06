using MediatR;

namespace HE.Investments.AHP.Consortium.Contract.Queries;

public record GetConsortiumsListQuery : IRequest<ConsortiumsList>;
