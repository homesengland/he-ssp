using HE.Investment.AHP.Contract.Application;
using MediatR;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record GetFullHomeTypeQuery(AhpApplicationId ApplicationId, HomeTypeId HomeTypeId) : IRequest<FullHomeType>;
