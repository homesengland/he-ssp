using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

internal sealed class CalculateAffordableRentQueryHandler : BaseQueryHandler, IRequestHandler<CalculateAffordableRentQuery, OperationResult>
{
    private readonly IHomeTypeRepository _homeTypeRepository;

    public CalculateAffordableRentQueryHandler(IHomeTypeRepository homeTypeRepository, ILogger<CalculateAffordableRentQueryHandler> logger)
        : base(logger)
    {
        _homeTypeRepository = homeTypeRepository;
    }

    private IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.TenureDetails };

    private IEnumerable<Action<CalculateAffordableRentQuery, IHomeTypeEntity>> CalculateActions => new[]
    {
        (CalculateAffordableRentQuery request, IHomeTypeEntity homeType) => homeType.TenureDetails.ChangeHomeMarketValue(request.HomeMarketValue, true),
        (request, homeType) => homeType.TenureDetails.ChangeHomeWeeklyRent(request.HomeWeeklyRent, true),
        (request, homeType) => homeType.TenureDetails.ChangeAffordableWeeklyRent(request.AffordableWeeklyRent, true),
        (request, homeType) => homeType.TenureDetails.ChangeTargetRentExceedMarketRent(request.TargetRentExceedMarketRent, true),
        (request, homeType) => homeType.TenureDetails.CalculateAffordableRentAsPercentageOfMarketRent(request.HomeWeeklyRent, request.AffordableWeeklyRent),
    };

    public async Task<OperationResult> Handle(CalculateAffordableRentQuery request, CancellationToken cancellationToken)
    {
        var applicationId = new ApplicationId(request.ApplicationId);
        var homeType = await _homeTypeRepository.GetById(
            applicationId,
            new HomeTypeId(request.HomeTypeId),
            SegmentTypes,
            cancellationToken);

        var errors = PerformWithValidation(CalculateActions.Select<Action<CalculateAffordableRentQuery, IHomeTypeEntity>, Action>(x => () => x(request, homeType)).ToArray());

        var result = errors.Any() ? new OperationResult(errors) : OperationResult.Success();
        return result;
    }
}
