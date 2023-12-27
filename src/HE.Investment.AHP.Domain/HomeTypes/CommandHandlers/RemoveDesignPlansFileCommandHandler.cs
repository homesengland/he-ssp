using HE.Investment.AHP.Domain.Common.ValueObjects;
using HE.Investment.AHP.Domain.HomeTypes.Commands;
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

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.DesignPlans };

    protected override IEnumerable<Action<RemoveDesignPlansFileCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (RemoveDesignPlansFileCommand request, IHomeTypeEntity homeType) => homeType.DesignPlans.MarkFileToRemove(new FileId(request.FileId)),
    };
}
