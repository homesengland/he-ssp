using MediatR;

namespace HE.Investment.AHP.Contract.Scheme.Queries;

public record GetApplicationSchemeQuery(string ApplicationId) : IRequest<Scheme>;
