using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Consortium.Shared.UserContext;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SavePeopleGroupForSpecificDesignFeaturesCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SavePeopleGroupForSpecificDesignFeaturesCommand>
{
    public SavePeopleGroupForSpecificDesignFeaturesCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IConsortiumUserContext accountUserContext,
        ILogger<SaveMoveOnAccommodationCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => [HomeTypeSegmentType.HomeInformation];

    protected override IEnumerable<Action<SavePeopleGroupForSpecificDesignFeaturesCommand, IHomeTypeEntity>> SaveActions =>
    [
        (SavePeopleGroupForSpecificDesignFeaturesCommand request, IHomeTypeEntity entity) =>
            entity.HomeInformation.ChangePeopleGroupForSpecificDesignFeatures(request.PeopleGroupForSpecificDesignFeatures),
    ];
}
