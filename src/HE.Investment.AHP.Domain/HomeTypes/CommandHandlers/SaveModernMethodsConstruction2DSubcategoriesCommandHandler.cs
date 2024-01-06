using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Account.Shared;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveModernMethodsConstruction2DSubcategoriesCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveModernMethodsConstruction2DSubcategoriesCommand>
{
    public SaveModernMethodsConstruction2DSubcategoriesCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IAccountUserContext accountUserContext,
        ILogger<SaveModernMethodsConstruction2DSubcategoriesCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.ModernMethodsConstruction };

    protected override IEnumerable<Action<SaveModernMethodsConstruction2DSubcategoriesCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (SaveModernMethodsConstruction2DSubcategoriesCommand request, IHomeTypeEntity homeType) =>
            homeType.ModernMethodsConstruction.ChangeModernMethodsConstruction2DSubcategories(request.ModernMethodsConstruction2DSubcategories),
    };
}
