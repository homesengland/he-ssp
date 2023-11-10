using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using MediatR;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

internal sealed class GetHomeTypeDetailsQueryHandler : IRequestHandler<GetHomeTypeDetailsQuery, HomeTypeDetails>
{
    private readonly IHomeTypeRepository _repository;

    public GetHomeTypeDetailsQueryHandler(IHomeTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<HomeTypeDetails> Handle(GetHomeTypeDetailsQuery request, CancellationToken cancellationToken)
    {
        var homeType = await _repository.GetById(
            new Domain.Application.ValueObjects.ApplicationId(request.ApplicationId),
            new HomeTypeId(request.HomeTypeId),
            new[] { HomeTypeSegmentType.HomeInformation },
            cancellationToken);

        return new HomeTypeDetails(
            request.HomeTypeId,
            homeType.Name?.Value,
            homeType.HomeInformation.NumberOfHomes?.Value,
            homeType.HousingType);
    }
}
