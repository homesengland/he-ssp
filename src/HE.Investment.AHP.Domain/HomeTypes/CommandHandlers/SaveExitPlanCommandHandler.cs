using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Extensions;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveExitPlanCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveExitPlanCommand>
{
    public SaveExitPlanCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IAccountUserContext accountUserContext,
        ILogger<SaveExitPlanCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.SupportedHousingInformation };

    protected override IEnumerable<Action<SaveExitPlanCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (SaveExitPlanCommand request, IHomeTypeEntity homeType) =>
        {
            var exitPlan = request.ExitPlan.IsNotProvided()
                ? null
                : new MoreInformation(request.ExitPlan!, "The exit plan or alternative use");
            homeType.SupportedHousingInformation.ChangeExitPlan(exitPlan);
        },
    };
}
