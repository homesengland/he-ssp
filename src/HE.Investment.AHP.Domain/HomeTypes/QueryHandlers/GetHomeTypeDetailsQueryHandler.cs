using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Account.Shared;
using MediatR;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

internal sealed class GetHomeTypeDetailsQueryHandler : IRequestHandler<GetHomeTypeDetailsQuery, HomeTypeDetails>
{
    private readonly IHomeTypeRepository _repository;

    private readonly IAccountUserContext _accountUserContext;

    public GetHomeTypeDetailsQueryHandler(IHomeTypeRepository repository, IAccountUserContext accountUserContext)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
    }

    public async Task<HomeTypeDetails> Handle(GetHomeTypeDetailsQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var homeType = await _repository.GetById(
            request.ApplicationId,
            request.HomeTypeId,
            account,
            new[] { HomeTypeSegmentType.HomeInformation },
            cancellationToken);

        return new HomeTypeDetails(
            homeType.Application.Name.Name,
            request.HomeTypeId,
            homeType.Name.Value,
            homeType.HomeInformation.NumberOfHomes?.Value,
            homeType.HousingType);
    }
}
