using MediatR;

namespace HE.Investment.AHP.Contract.Application.Queries;

public record GetApplicationQuery(AhpApplicationId ApplicationId) : IRequest<Application>;
