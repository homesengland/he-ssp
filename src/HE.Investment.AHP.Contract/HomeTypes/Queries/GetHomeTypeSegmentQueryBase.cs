using MediatR;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record GetHomeTypeSegmentQueryBase<TResult>(string ApplicationId, string HomeTypeId) : IRequest<TResult>
    where TResult : HomeTypeSegmentBase;
