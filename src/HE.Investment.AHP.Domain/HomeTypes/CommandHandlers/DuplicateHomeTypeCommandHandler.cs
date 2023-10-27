using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.InvestmentLoans.Common.Validation;
using MediatR;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class DuplicateHomeTypeCommandHandler : IRequestHandler<DuplicateHomeTypeCommand, OperationResult<HomeTypeId>>
{
    private readonly IHomeTypesRepository _homeTypesRepository;

    private readonly IHomeTypeRepository _homeTypeRepository;

    public DuplicateHomeTypeCommandHandler(IHomeTypesRepository homeTypesRepository, IHomeTypeRepository homeTypeRepository)
    {
        _homeTypesRepository = homeTypesRepository;
        _homeTypeRepository = homeTypeRepository;
    }

    public async Task<OperationResult<HomeTypeId>> Handle(DuplicateHomeTypeCommand request, CancellationToken cancellationToken)
    {
        var homeTypeId = new HomeTypeId(request.HomeTypeId);
        var homeTypes = await _homeTypesRepository.GetByApplicationId(request.ApplicationId, cancellationToken);
        var homeType = await _homeTypeRepository.GetById(
            request.ApplicationId,
            homeTypeId,
            HomeTypeSegmentTypes.All,
            cancellationToken);

        var duplicatedName = homeTypes.DuplicateName(homeTypeId);
        var duplicatedHomeType = homeType.Duplicate(duplicatedName);

        await _homeTypeRepository.Save(request.ApplicationId, duplicatedHomeType, HomeTypeSegmentTypes.All, cancellationToken);

        return OperationResult.Success(duplicatedHomeType.Id!);
    }
}
