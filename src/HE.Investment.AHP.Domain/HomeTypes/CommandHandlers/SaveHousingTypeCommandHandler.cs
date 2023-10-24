using HE.Investment.AHP.BusinessLogic.HomeTypes.Commands;
using HE.Investment.AHP.BusinessLogic.HomeTypes.Entities;
using HE.Investment.AHP.BusinessLogic.HomeTypes.ValueObjects;
using HE.InvestmentLoans.Common.Validation;
using MediatR;

namespace HE.Investment.AHP.BusinessLogic.HomeTypes.CommandHandlers;

public class SaveHousingTypeCommandHandler : IRequestHandler<SaveHousingTypeCommand, OperationResult<HomeTypeId>>
{
    private readonly IHomeTypeRepository _repository;

    public SaveHousingTypeCommandHandler(IHomeTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult<HomeTypeId>> Handle(SaveHousingTypeCommand request, CancellationToken cancellationToken)
    {
        var homeType = await GetOrCreateHomeTypeEntity(request.FinancialSchemeId, request.HomeTypeId, cancellationToken);

        homeType.ChangeName(request.HomeTypeName);
        homeType.HousingTypeSectionEntity.ChangeHousingType(request.HousingType);

        await _repository.Save(request.FinancialSchemeId, homeType, HomeTypeSectionType.HousingType, cancellationToken);

        return new OperationResult<HomeTypeId>(homeType.Id!);
    }

    private async Task<HomeTypeEntity> GetOrCreateHomeTypeEntity(string financialSchemeId, string? homeTypeId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(homeTypeId))
        {
            return new HomeTypeEntity(homeTypeId, new HousingTypeSectionEntity());
        }

        return await _repository.Get(new HomeTypeId(homeTypeId!), HomeTypeSectionType.HousingType, cancellationToken);
    }
}
