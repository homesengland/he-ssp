using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Account.Shared;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveDisabledPeopleHousingTypeCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveDisabledPeopleHousingTypeCommand>
{
    public SaveDisabledPeopleHousingTypeCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IAccountUserContext accountUserContext,
        ILogger<SaveDisabledPeopleHousingTypeCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.DisabledAndVulnerablePeople };

    protected override IEnumerable<Action<SaveDisabledPeopleHousingTypeCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (SaveDisabledPeopleHousingTypeCommand request, IHomeTypeEntity homeType) =>
            homeType.DisabledPeopleHomeTypeDetails.ChangeHousingType(request.HousingType),
    };
}
