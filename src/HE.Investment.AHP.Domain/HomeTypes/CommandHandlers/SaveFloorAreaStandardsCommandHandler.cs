using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Account.Shared;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveFloorAreaStandardsCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveFloorAreaStandardsCommand>
{
    public SaveFloorAreaStandardsCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IAccountUserContext accountUserContext,
        ILogger<SaveFloorAreaStandardsCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.HomeInformation };

    protected override IEnumerable<Action<SaveFloorAreaStandardsCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (SaveFloorAreaStandardsCommand request, IHomeTypeEntity homeType) =>
            homeType.HomeInformation.ChangeNationallyDescribedSpaceStandards(request.NationallyDescribedSpaceStandards),
    };
}
