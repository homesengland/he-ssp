using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using MediatR;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

internal sealed class GetSupportedHousingInformationQueryHandler : IRequestHandler<GetSupportedHousingInformationQuery, SupportedHousingInformation>
{
    private readonly IHomeTypeRepository _repository;

    public GetSupportedHousingInformationQueryHandler(IHomeTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<SupportedHousingInformation> Handle(GetSupportedHousingInformationQuery request, CancellationToken cancellationToken)
    {
        var homeType = await _repository.GetById(
            new Domain.Application.ValueObjects.ApplicationId(request.ApplicationId),
            new HomeTypeId(request.HomeTypeId),
            new[] { HomeTypeSegmentType.SupportedHousingInformation },
            cancellationToken);

        return new SupportedHousingInformation(
            homeType.Name?.Value,
            homeType.SupportedHousingInformation.LocalCommissioningBodiesConsulted,
            homeType.SupportedHousingInformation.ShortStayAccommodation,
            homeType.SupportedHousingInformation.RevenueFundingType);
    }
}
