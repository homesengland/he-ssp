using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common;
using MediatR;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

internal sealed class GetHomeInformationQueryHandler : IRequestHandler<GetHomeInformationQuery, HomeInformation>
{
    private readonly IHomeTypeRepository _repository;

    public GetHomeInformationQueryHandler(IHomeTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<HomeInformation> Handle(GetHomeInformationQuery request, CancellationToken cancellationToken)
    {
        var homeType = await _repository.GetById(
            new Domain.Application.ValueObjects.ApplicationId(request.ApplicationId),
            new HomeTypeId(request.HomeTypeId),
            new[] { HomeTypeSegmentType.HomeInformation },
            cancellationToken);
        var homeInformation = homeType.HomeInformation;

        return new HomeInformation(
            homeType.Name?.Value ?? Check.IfCanBeNull,
            homeInformation.NumberOfHomes?.Value,
            homeInformation.NumberOfBedrooms?.Value,
            homeInformation.MaximumOccupancy?.Value,
            homeInformation.NumberOfStoreys?.Value);
    }
}
