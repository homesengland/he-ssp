using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Extensions;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveTypologyLocationAndDesignCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveTypologyLocationAndDesignCommand>
{
    public SaveTypologyLocationAndDesignCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IAccountUserContext accountUserContext,
        ILogger<SaveTypologyLocationAndDesignCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.SupportedHousingInformation };

    protected override IEnumerable<Action<SaveTypologyLocationAndDesignCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (SaveTypologyLocationAndDesignCommand request, IHomeTypeEntity homeType) =>
        {
            var typologyLocationAndDesign = request.TypologyLocationAndDesign.IsNotProvided()
                ? null
                : new MoreInformation(request.TypologyLocationAndDesign!, "The typology, location and design of these homes");
            homeType.SupportedHousingInformation.ChangeTypologyLocationAndDesign(typologyLocationAndDesign);
        },
    };
}
