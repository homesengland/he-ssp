using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.Application.Mappers;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Consortium.Shared.UserContext;
using MediatR;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

internal sealed class GetHomeTypesQueryHandler : IRequestHandler<GetHomeTypesQuery, ApplicationHomeTypes>
{
    private readonly IHomeTypeRepository _repository;

    private readonly IConsortiumUserContext _accountUserContext;

    public GetHomeTypesQueryHandler(IHomeTypeRepository repository, IConsortiumUserContext accountUserContext)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
    }

    public async Task<ApplicationHomeTypes> Handle(GetHomeTypesQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var homeTypes = await _repository.GetByApplicationId(
            request.ApplicationId,
            account,
            cancellationToken);

        return new ApplicationHomeTypes(
            ApplicationBasicInfoMapper.Map(homeTypes.Application),
            homeTypes.HomeTypes.OrderByDescending(x => x.CreatedOn).Select(Map).ToList());
    }

    private static HomeTypeDetails Map(IHomeTypeEntity homeType)
    {
        return new HomeTypeDetails(
            homeType.Application.Name.Value,
            homeType.Id,
            homeType.Name.Value,
            homeType.HomeInformation.NumberOfHomes?.Value,
            homeType.HousingType);
    }
}
