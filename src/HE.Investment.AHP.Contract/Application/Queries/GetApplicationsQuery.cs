using MediatR;

namespace HE.Investment.AHP.Contract.Application.Queries;

public record GetApplicationsQuery : IRequest<IList<Application>>;
