using MediatR;

namespace HE.Investments.AHP.Consortium.Contract.Queries;

public record GetConsortiumDetailsQuery(ConsortiumId ConsortiumId) : IRequest<ConsortiumDetails>;
