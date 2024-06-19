using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Extensions;
using HE.Investments.Consortium.Shared.UserContext;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveExitPlanCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveExitPlanCommand>
{
    public SaveExitPlanCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IConsortiumUserContext accountUserContext,
        ILogger<SaveExitPlanCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => [HomeTypeSegmentType.SupportedHousingInformation];

    protected override IEnumerable<Action<SaveExitPlanCommand, IHomeTypeEntity>> SaveActions =>
    [
        (SaveExitPlanCommand request, IHomeTypeEntity homeType) =>
        {
            var exitPlan = request.ExitPlan.IsNotProvided()
                ? null
                : new MoreInformation(request.ExitPlan!, "exit plan or alternative use");
            homeType.SupportedHousingInformation.ChangeExitPlan(exitPlan);
        },
    ];
}
