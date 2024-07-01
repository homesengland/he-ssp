using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Consortium.Shared.UserContext;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class RemoveDesignPlansFileCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<RemoveDesignPlansFileCommand>
{
    public RemoveDesignPlansFileCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IConsortiumUserContext accountUserContext,
        ILogger<RemoveDesignPlansFileCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger, true)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => [HomeTypeSegmentType.DesignPlans];

    protected override IEnumerable<Action<RemoveDesignPlansFileCommand, IHomeTypeEntity>> SaveActions =>
    [
        (request, homeType) => homeType.DesignPlans.MarkFileToRemove(request.FileId),
    ];
}
