using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Account.Shared;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class RemoveDesignPlansFileCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<RemoveDesignPlansFileCommand>
{
    public RemoveDesignPlansFileCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IAccountUserContext accountUserContext,
        ILogger<RemoveDesignPlansFileCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => [HomeTypeSegmentType.DesignPlans];

    protected override IEnumerable<Action<RemoveDesignPlansFileCommand, IHomeTypeEntity>> SaveActions =>
    [
        (RemoveDesignPlansFileCommand request, IHomeTypeEntity homeType) => homeType.DesignPlans.MarkFileToRemove(request.FileId),
    ];
}
