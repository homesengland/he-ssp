using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Account.Shared;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveFacilityTypeCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveFacilityTypeCommand>
{
    public SaveFacilityTypeCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IAccountUserContext accountUserContext,
        ILogger<SaveFacilityTypeCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.HomeInformation };

    protected override IEnumerable<Action<SaveFacilityTypeCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (SaveFacilityTypeCommand request, IHomeTypeEntity entity) =>
            entity.HomeInformation.ChangeFacilityType(request.FacilityType),
    };
}
