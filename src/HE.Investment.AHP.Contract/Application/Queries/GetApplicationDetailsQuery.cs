using MediatR;

namespace HE.Investment.AHP.Contract.Application.Queries;

public record GetApplicationDetailsQuery(AhpApplicationId ApplicationId) : IRequest<ApplicationExtendedDetails>;
