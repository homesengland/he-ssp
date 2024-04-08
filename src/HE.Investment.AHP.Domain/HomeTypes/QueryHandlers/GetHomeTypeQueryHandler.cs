using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.Application.Mappers;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Mappers;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Account.Shared;
using MediatR;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

internal sealed class GetHomeTypeQueryHandler : IRequestHandler<GetHomeTypeQuery, HomeType>
{
    private readonly IHomeTypeRepository _repository;

    private readonly IAccountUserContext _accountUserContext;

    public GetHomeTypeQueryHandler(IHomeTypeRepository repository, IAccountUserContext accountUserContext)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
    }

    public async Task<HomeType> Handle(GetHomeTypeQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var homeType = await _repository.GetById(
            request.ApplicationId,
            request.HomeTypeId,
            account,
            HomeTypeSegmentTypes.WorkflowConditionals,
            cancellationToken);

        return new HomeType(
            ApplicationBasicInfoMapper.Map(homeType.Application),
            request.HomeTypeId,
            homeType.Name.Value,
            homeType.HousingType,
            HomeTypeConditionalsMapper.Map(homeType));
    }
}
