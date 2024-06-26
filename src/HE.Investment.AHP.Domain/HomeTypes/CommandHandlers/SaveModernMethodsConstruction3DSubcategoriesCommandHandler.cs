using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Consortium.Shared.UserContext;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveModernMethodsConstruction3DSubcategoriesCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveModernMethodsConstruction3DSubcategoriesCommand>
{
    public SaveModernMethodsConstruction3DSubcategoriesCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IConsortiumUserContext accountUserContext,
        ILogger<SaveModernMethodsConstruction3DSubcategoriesCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => [HomeTypeSegmentType.ModernMethodsConstruction];

    protected override IEnumerable<Action<SaveModernMethodsConstruction3DSubcategoriesCommand, IHomeTypeEntity>> SaveActions =>
    [
        (SaveModernMethodsConstruction3DSubcategoriesCommand request, IHomeTypeEntity homeType) =>
            homeType.ModernMethodsConstruction.ChangeModernMethodsConstruction3DSubcategories(request.ModernMethodsConstruction3DSubcategories),
    ];
}
