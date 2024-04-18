using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Mappers;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Account.Shared;
using MediatR;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

internal abstract class GetHomeTypeSegmentQueryHandlerBase<TQuery, TSegment, TResult> : IRequestHandler<TQuery, TResult>
    where TQuery : GetHomeTypeSegmentQueryBase<TResult>
    where TSegment : IHomeTypeSegmentEntity
    where TResult : HomeTypeSegmentBase
{
    private readonly IHomeTypeRepository _repository;

    private readonly IHomeTypeSegmentContractMapper<TSegment, TResult> _mapper;

    private readonly IAccountUserContext _accountUserContext;

    protected GetHomeTypeSegmentQueryHandlerBase(IHomeTypeRepository repository, IHomeTypeSegmentContractMapper<TSegment, TResult> mapper, IAccountUserContext accountUserContext)
    {
        _repository = repository;
        _mapper = mapper;
        _accountUserContext = accountUserContext;
    }

    protected virtual bool LoadFiles => false;

    public async Task<TResult> Handle(TQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var homeType = await _repository.GetById(
            request.ApplicationId,
            request.HomeTypeId,
            account,
            cancellationToken,
            loadFiles: LoadFiles);

        return _mapper.Map(homeType.Application.Name, homeType.Name, GetSegment(homeType));
    }

    protected abstract TSegment GetSegment(IHomeTypeEntity homeType);
}
