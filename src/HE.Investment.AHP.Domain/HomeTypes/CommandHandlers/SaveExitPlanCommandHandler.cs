using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveExitPlanCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveExitPlanCommand>
{
    public SaveExitPlanCommandHandler(IHomeTypeRepository homeTypeRepository, ILogger<SaveExitPlanCommandHandler> logger)
        : base(homeTypeRepository, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.SupportedHousingInformation };

    protected override IEnumerable<Action<SaveExitPlanCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (SaveExitPlanCommand request, IHomeTypeEntity homeType) => homeType.SupportedHousingInformation.ChangeExitPlan(request.ExitPlan),
    };
}
