using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveHappiDesignPrinciplesCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveHappiDesignPrinciplesCommand>
{
    public SaveHappiDesignPrinciplesCommandHandler(IHomeTypeRepository homeTypeRepository, ILogger<SaveHappiDesignPrinciplesCommandHandler> logger)
        : base(homeTypeRepository, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.DesignPlans };

    protected override IEnumerable<Action<SaveHappiDesignPrinciplesCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (SaveHappiDesignPrinciplesCommand request, IHomeTypeEntity homeType) =>
            homeType.DesignPlans.ChangeDesignPrinciples(request.DesignPrinciples),
    };
}
