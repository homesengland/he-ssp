using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.InvestmentLoans.Common.Validation;
using MediatR;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class DuplicateHousingTypeCommandHandler : IRequestHandler<DuplicateHousingTypeCommand, OperationResult<HomeTypeId>>
{
    private readonly IHomeTypesRepository _homeTypesRepository;

    private readonly IHomeTypeRepository _homeTypeRepository;

    public DuplicateHousingTypeCommandHandler(IHomeTypesRepository homeTypesRepository, IHomeTypeRepository homeTypeRepository)
    {
        _homeTypesRepository = homeTypesRepository;
        _homeTypeRepository = homeTypeRepository;
    }

    public async Task<OperationResult<HomeTypeId>> Handle(DuplicateHousingTypeCommand request, CancellationToken cancellationToken)
    {
        var homeTypeId = new HomeTypeId(request.HomeTypeId);
        var homeTypes = await _homeTypesRepository.GetBySchemeId(request.SchemeId, cancellationToken);
        var homeType = await _homeTypeRepository.GetById(
            request.SchemeId,
            homeTypeId,
            HomeTypeSectionTypes.All, // TODO: use in other places
            cancellationToken);

        var duplicatedName = homeTypes.DuplicateName(homeTypeId);
        var duplicatedHomeType = homeType.Duplicate(duplicatedName);

        await _homeTypeRepository.Save(request.SchemeId, duplicatedHomeType, HomeTypeSectionTypes.All, cancellationToken);

        return OperationResult.Success(duplicatedHomeType.Id!);
    }
}
