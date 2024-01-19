using HE.Investment.AHP.Contract.Application;
using MediatR;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record GetHomeTypeDetailsQuery(AhpApplicationId ApplicationId, HomeTypeId HomeTypeId) : IRequest<HomeTypeDetails>;
