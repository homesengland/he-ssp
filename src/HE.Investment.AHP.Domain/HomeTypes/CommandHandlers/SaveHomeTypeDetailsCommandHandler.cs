using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.InvestmentLoans.Common.Validation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveHomeTypeDetailsCommandHandler : HomeTypeCommandHandlerBase, IRequestHandler<SaveHomeTypeDetailsCommand, OperationResult<HomeTypeId?>>
{
    private readonly IHomeTypesRepository _homeTypesRepository;

    private readonly IHomeTypeRepository _homeTypeRepository;

    public SaveHomeTypeDetailsCommandHandler(
        IHomeTypesRepository homeTypesRepository,
        IHomeTypeRepository homeTypeRepository,
        ILogger<SaveHomeTypeDetailsCommand> logger)
        : base(logger)
    {
        _homeTypesRepository = homeTypesRepository;
        _homeTypeRepository = homeTypeRepository;
    }

    public async Task<OperationResult<HomeTypeId?>> Handle(SaveHomeTypeDetailsCommand request, CancellationToken cancellationToken)
    {
        var homeTypes = await _homeTypesRepository.GetByApplicationId(request.ApplicationId, cancellationToken);
        var homeType = await GetOrCreateHomeTypeEntity(request.ApplicationId, request.HomeTypeId, cancellationToken);

        var validationErrors = PerformWithValidation(
            () => homeTypes.ValidateNameUniqueness(homeType.Id, request.HomeTypeName),
            () => homeType.ChangeDetails(request.HomeTypeName, request.HousingType));
        if (validationErrors.Any())
        {
            return new OperationResult<HomeTypeId?>(validationErrors, null);
        }

        await _homeTypeRepository.Save(request.ApplicationId, homeType, Array.Empty<HomeTypeSegmentType>(), cancellationToken);

        return new OperationResult<HomeTypeId?>(homeType.Id!);
    }

    private async Task<HomeTypeEntity> GetOrCreateHomeTypeEntity(string applicationId, string? homeTypeId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(homeTypeId))
        {
            return new HomeTypeEntity();
        }

        return await _homeTypeRepository.GetById(applicationId, new HomeTypeId(homeTypeId), Array.Empty<HomeTypeSegmentType>(), cancellationToken);
    }
}
