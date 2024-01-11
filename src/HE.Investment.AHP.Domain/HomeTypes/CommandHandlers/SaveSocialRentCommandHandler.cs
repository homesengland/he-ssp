using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Account.Shared;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveSocialRentCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveSocialRentCommand>
{
    public SaveSocialRentCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IAccountUserContext accountUserContext,
        ILogger<SaveSocialRentCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.TenureDetails };

    protected override IEnumerable<Action<SaveSocialRentCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (SaveSocialRentCommand request, IHomeTypeEntity homeType) => homeType.TenureDetails.ChangeMarketValue(request.MarketValue),
        (request, homeType) => homeType.TenureDetails.ChangeProspectiveRent(request.ProspectiveRent),
    };
}
