using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Mappers;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using MediatR;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

internal abstract class GetHomeTypeSegmentQueryHandlerBase<TQuery, TSegment, TResult> : IRequestHandler<TQuery, TResult>
    where TQuery : GetHomeTypeSegmentQueryBase<TResult>
    where TSegment : IHomeTypeSegmentEntity
    where TResult : HomeTypeSegmentBase
{
    private readonly IHomeTypeRepository _repository;

    private readonly IHomeTypeSegmentContractMapper<TSegment, TResult> _mapper;

    protected GetHomeTypeSegmentQueryHandlerBase(IHomeTypeRepository repository, IHomeTypeSegmentContractMapper<TSegment, TResult> mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    protected abstract IReadOnlyCollection<HomeTypeSegmentType> Segments { get; }

    public async Task<TResult> Handle(TQuery request, CancellationToken cancellationToken)
    {
        var applicationId = new Domain.Application.ValueObjects.ApplicationId(request.ApplicationId);
        var homeType = await _repository.GetById(
            applicationId,
            new HomeTypeId(request.HomeTypeId),
            Segments,
            cancellationToken);

        return _mapper.Map(homeType.Application.Name, homeType.Name, GetSegment(homeType));
    }

    protected abstract TSegment GetSegment(IHomeTypeEntity homeType);
}
