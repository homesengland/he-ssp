using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
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
        (SaveTypologyLocationAndDesignCommand request, IHomeTypeEntity homeType) => homeType.SupportedHousingInformation.ChangeTypologyLocationAndDesign(request.TypologyLocationAndDesign),
    };
}
