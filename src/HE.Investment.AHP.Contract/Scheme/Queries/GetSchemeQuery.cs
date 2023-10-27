using MediatR;

namespace HE.Investment.AHP.Contract.Scheme.Queries;

public record GetSchemeQuery(string SchemeId) : IRequest<Scheme>;
