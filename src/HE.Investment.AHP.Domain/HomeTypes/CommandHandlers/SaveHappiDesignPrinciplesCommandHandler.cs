using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Account.Shared;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveHappiDesignPrinciplesCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveHappiDesignPrinciplesCommand>
{
    public SaveHappiDesignPrinciplesCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IAccountUserContext accountUserContext,
        ILogger<SaveHappiDesignPrinciplesCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.DesignPlans };

    protected override IEnumerable<Action<SaveHappiDesignPrinciplesCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (SaveHappiDesignPrinciplesCommand request, IHomeTypeEntity homeType) => homeType.DesignPlans.ChangeDesignPrinciples(request.DesignPrinciples),
    };
}
