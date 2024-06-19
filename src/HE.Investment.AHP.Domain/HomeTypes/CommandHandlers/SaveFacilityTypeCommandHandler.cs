using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Consortium.Shared.UserContext;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveFacilityTypeCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveFacilityTypeCommand>
{
    public SaveFacilityTypeCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IConsortiumUserContext accountUserContext,
        ILogger<SaveFacilityTypeCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => [HomeTypeSegmentType.HomeInformation];

    protected override IEnumerable<Action<SaveFacilityTypeCommand, IHomeTypeEntity>> SaveActions =>
    [
        (SaveFacilityTypeCommand request, IHomeTypeEntity entity) =>
            entity.HomeInformation.ChangeFacilityType(request.FacilityType),
    ];
}
