using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Consortium.Shared.UserContext;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveModernMethodsConstructionCategoriesCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveModernMethodsConstructionCategoriesCommand>
{
    public SaveModernMethodsConstructionCategoriesCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IConsortiumUserContext accountUserContext,
        ILogger<SaveModernMethodsConstructionCategoriesCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => [HomeTypeSegmentType.ModernMethodsConstruction];

    protected override IEnumerable<Action<SaveModernMethodsConstructionCategoriesCommand, IHomeTypeEntity>> SaveActions =>
    [
        (request, homeType) => homeType.ModernMethodsConstruction.ChangeModernMethodsConstructionCategories(request.ModernMethodsConstructionCategories),
    ];
}
