using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Consortium.Shared.UserContext;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveHappiDesignPrinciplesCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveHappiDesignPrinciplesCommand>
{
    public SaveHappiDesignPrinciplesCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IConsortiumUserContext accountUserContext,
        ILogger<SaveHappiDesignPrinciplesCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => [HomeTypeSegmentType.DesignPlans];

    protected override IEnumerable<Action<SaveHappiDesignPrinciplesCommand, IHomeTypeEntity>> SaveActions =>
    [
        (SaveHappiDesignPrinciplesCommand request, IHomeTypeEntity homeType) => homeType.DesignPlans.ChangeDesignPrinciples(request.DesignPrinciples),
    ];
}
