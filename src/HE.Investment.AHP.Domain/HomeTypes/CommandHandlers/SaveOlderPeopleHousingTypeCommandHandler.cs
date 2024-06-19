using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Consortium.Shared.UserContext;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveOlderPeopleHousingTypeCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveOlderPeopleHousingTypeCommand>
{
    public SaveOlderPeopleHousingTypeCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IConsortiumUserContext accountUserContext,
        ILogger<SaveOlderPeopleHousingTypeCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => [HomeTypeSegmentType.OlderPeople];

    protected override IEnumerable<Action<SaveOlderPeopleHousingTypeCommand, IHomeTypeEntity>> SaveActions =>
    [
        (SaveOlderPeopleHousingTypeCommand request, IHomeTypeEntity homeType) => homeType.OlderPeopleHomeTypeDetails.ChangeHousingType(request.HousingType),
    ];
}
