using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SavePeopleGroupForSpecificDesignFeaturesCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SavePeopleGroupForSpecificDesignFeaturesCommand>
{
    public SavePeopleGroupForSpecificDesignFeaturesCommandHandler(IHomeTypeRepository homeTypeRepository, ILogger<SaveMoveOnAccommodationCommandHandler> logger)
        : base(homeTypeRepository, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.HomeInformation };

    protected override IEnumerable<Action<SavePeopleGroupForSpecificDesignFeaturesCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (SavePeopleGroupForSpecificDesignFeaturesCommand request, IHomeTypeEntity entity) =>
            entity.HomeInformation.ChangePeopleGroupForSpecificDesignFeatures(request.PeopleGroupForSpecificDesignFeatures),
    };
}
