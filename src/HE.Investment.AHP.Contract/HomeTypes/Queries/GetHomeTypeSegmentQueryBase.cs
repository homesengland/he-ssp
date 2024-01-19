using HE.Investment.AHP.Contract.Application;
using MediatR;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record GetHomeTypeSegmentQueryBase<TResult>(AhpApplicationId ApplicationId, HomeTypeId HomeTypeId) : IRequest<TResult>
    where TResult : HomeTypeSegmentBase;
