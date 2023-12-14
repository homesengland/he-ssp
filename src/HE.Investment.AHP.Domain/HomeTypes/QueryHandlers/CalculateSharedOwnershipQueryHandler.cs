using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

internal sealed class CalculateSharedOwnershipQueryHandler : BaseQueryHandler, IRequestHandler<CalculateSharedOwnershipQuery, OperationResult>
{
    private readonly IHomeTypeRepository _homeTypeRepository;

    public CalculateSharedOwnershipQueryHandler(IHomeTypeRepository homeTypeRepository, ILogger<CalculateSharedOwnershipQueryHandler> logger)
        : base(logger)
    {
        _homeTypeRepository = homeTypeRepository;
    }

    private IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.TenureDetails };

    private IEnumerable<Action<CalculateSharedOwnershipQuery, IHomeTypeEntity>> CalculateActions => new[]
    {
        (CalculateSharedOwnershipQuery request, IHomeTypeEntity homeType) => homeType.TenureDetails.ChangeMarketValue(request.MarketValue, true),
        (request, homeType) => homeType.TenureDetails.ChangeInitialSale(request.InitialSale, true),
        (request, homeType) => homeType.TenureDetails.ChangeExpectedFirstTranche(request.MarketValue, request.InitialSale),
        (request, homeType) => homeType.TenureDetails.ChangeProspectiveRent(request.ProspectiveRent, true),
        (request, homeType) =>
            homeType.TenureDetails.ChangeProspectiveRentAsPercentageOfTheUnsoldShare(
                request.MarketValue,
                request.ProspectiveRent,
                request.InitialSale),
    };

    public async Task<OperationResult> Handle(CalculateSharedOwnershipQuery request, CancellationToken cancellationToken)
    {
        var applicationId = new ApplicationId(request.ApplicationId);
        var homeType = await _homeTypeRepository.GetById(
            applicationId,
            new HomeTypeId(request.HomeTypeId),
            SegmentTypes,
            cancellationToken);

        var errors = PerformWithValidation(CalculateActions
            .Select<Action<CalculateSharedOwnershipQuery, IHomeTypeEntity>, Action>(x => () => x(request, homeType))
            .ToArray());

        var result = errors.Any() ? new OperationResult(errors) : OperationResult.Success();
        return result;
    }
}
