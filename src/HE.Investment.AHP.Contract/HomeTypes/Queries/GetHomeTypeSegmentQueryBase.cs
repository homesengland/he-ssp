using HE.Investment.AHP.Contract.Application;
using MediatR;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record GetHomeTypeSegmentQueryBase<TResult>(AhpApplicationId ApplicationId, string HomeTypeId) : IRequest<TResult>
    where TResult : HomeTypeSegmentBase;
