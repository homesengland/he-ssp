using MediatR;

namespace HE.Investment.AHP.Contract.Application.Queries;

public record GetApplicationQuery(string ApplicationId) : IRequest<Application>;
