using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.InvestmentLoans.Common.Validation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveHousingTypeCommandHandler : HomeTypeCommandHandlerBase, IRequestHandler<SaveHousingTypeCommand, OperationResult<HomeTypeId?>>
{
    private readonly IHomeTypesRepository _homeTypesRepository;

    private readonly IHomeTypeRepository _homeTypeRepository;

    public SaveHousingTypeCommandHandler(
        IHomeTypesRepository homeTypesRepository,
        IHomeTypeRepository homeTypeRepository,
        ILogger<SaveHousingTypeCommand> logger)
        : base(logger)
    {
        _homeTypesRepository = homeTypesRepository;
        _homeTypeRepository = homeTypeRepository;
    }

    public async Task<OperationResult<HomeTypeId?>> Handle(SaveHousingTypeCommand request, CancellationToken cancellationToken)
    {
        var homeTypes = await _homeTypesRepository.GetBySchemeId(request.SchemeId, cancellationToken);
        var homeType = await GetOrCreateHomeTypeEntity(request.SchemeId, request.HomeTypeId, cancellationToken);

        var validationErrors = PerformWithValidation(
            () => homeTypes.ValidateNameUniqueness(homeType.Id, request.HomeTypeName),
            () => homeType.ChangeName(request.HomeTypeName),
            () => homeType.HousingTypeSectionEntity.ChangeHousingType(request.HousingType));
        if (validationErrors.Any())
        {
            return new OperationResult<HomeTypeId?>(validationErrors, null);
        }

        await _homeTypeRepository.Save(request.SchemeId, homeType, new[] { HomeTypeSectionType.HousingType }, cancellationToken);

        return new OperationResult<HomeTypeId?>(homeType.Id!);
    }

    private async Task<HomeTypeEntity> GetOrCreateHomeTypeEntity(string schemeId, string? homeTypeId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(homeTypeId))
        {
            return new HomeTypeEntity(homeTypeId, new HousingTypeSectionEntity());
        }

        return await _homeTypeRepository.GetById(schemeId, new HomeTypeId(homeTypeId), new[] { HomeTypeSectionType.HousingType }, cancellationToken);
    }
}
