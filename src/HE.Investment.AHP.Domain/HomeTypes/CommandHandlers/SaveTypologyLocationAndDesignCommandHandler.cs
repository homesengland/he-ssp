using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveTypologyLocationAndDesignCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveTypologyLocationAndDesignCommand>
{
    public SaveTypologyLocationAndDesignCommandHandler(IHomeTypeRepository homeTypeRepository, ILogger<SaveTypologyLocationAndDesignCommandHandler> logger)
        : base(homeTypeRepository, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.SupportedHousingInformation };

    protected override IEnumerable<Action<SaveTypologyLocationAndDesignCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (SaveTypologyLocationAndDesignCommand request, IHomeTypeEntity homeType) =>
        {
            var typologyLocationAndDesign = string.IsNullOrEmpty(request.TypologyLocationAndDesign)
                ? null
                : new MoreInformation(request.TypologyLocationAndDesign, "The typology, location and design of these homes");
            homeType.SupportedHousingInformation.ChangeTypologyLocationAndDesign(typologyLocationAndDesign);
        },
    };
}
