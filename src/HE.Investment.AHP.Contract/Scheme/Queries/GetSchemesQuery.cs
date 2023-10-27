using MediatR;

namespace HE.Investment.AHP.Contract.Scheme.Queries;

public record GetSchemesQuery() : IRequest<IList<Scheme>>;
