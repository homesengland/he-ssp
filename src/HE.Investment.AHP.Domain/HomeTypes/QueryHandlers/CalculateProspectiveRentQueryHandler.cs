using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

internal sealed class CalculateProspectiveRentQueryHandler : BaseQueryHandler, IRequestHandler<CalculateProspectiveRentQuery, OperationResult>
{
    private readonly IHomeTypeRepository _homeTypeRepository;

    public CalculateProspectiveRentQueryHandler(IHomeTypeRepository homeTypeRepository, ILogger<CalculateProspectiveRentQueryHandler> logger)
        : base(logger)
    {
        _homeTypeRepository = homeTypeRepository;
    }

    private IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.TenureDetails };

    private IEnumerable<Action<CalculateProspectiveRentQuery, IHomeTypeEntity>> CalculateActions => new[]
    {
        (CalculateProspectiveRentQuery request, IHomeTypeEntity homeType) => homeType.TenureDetails.ChangeMarketValue(request.MarketValue, true),
        (request, homeType) => homeType.TenureDetails.ChangeMarketRent(request.MarketRent, true),
        (request, homeType) => homeType.TenureDetails.ChangeProspectiveRent(request.ProspectiveRent, true),
        (request, homeType) => homeType.TenureDetails.ChangeTargetRentExceedMarketRent(request.TargetRentExceedMarketRent, true),
        (request, homeType) => homeType.TenureDetails.ChangeProspectiveRentAsPercentageOfMarketRent(request.MarketRent, request.ProspectiveRent),
    };

    public async Task<OperationResult> Handle(CalculateProspectiveRentQuery request, CancellationToken cancellationToken)
    {
        var applicationId = new ApplicationId(request.ApplicationId);
        var homeType = await _homeTypeRepository.GetById(
            applicationId,
            new HomeTypeId(request.HomeTypeId),
            SegmentTypes,
            cancellationToken);

        var errors = PerformWithValidation(CalculateActions.Select<Action<CalculateProspectiveRentQuery, IHomeTypeEntity>, Action>(x => () => x(request, homeType)).ToArray());

        var result = errors.Any() ? new OperationResult(errors) : OperationResult.Success();
        return result;
    }
}
