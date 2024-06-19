using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Consortium.Shared.UserContext;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveDisabledPeopleHousingTypeCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveDisabledPeopleHousingTypeCommand>
{
    public SaveDisabledPeopleHousingTypeCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IConsortiumUserContext accountUserContext,
        ILogger<SaveDisabledPeopleHousingTypeCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => [HomeTypeSegmentType.DisabledAndVulnerablePeople];

    protected override IEnumerable<Action<SaveDisabledPeopleHousingTypeCommand, IHomeTypeEntity>> SaveActions =>
    [
        (SaveDisabledPeopleHousingTypeCommand request, IHomeTypeEntity homeType) =>
            homeType.DisabledPeopleHomeTypeDetails.ChangeHousingType(request.HousingType),
    ];
}
