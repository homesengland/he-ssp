using HE.Investment.AHP.Contract.Scheme.Commands;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investments.Consortium.Shared.UserContext;

namespace HE.Investment.AHP.Domain.Scheme.CommandHandlers;

public class ChangeSchemeHousingNeedsCommandHandler : UpdateSchemeCommandHandler<ChangeSchemeHousingNeedsCommand>
{
    public ChangeSchemeHousingNeedsCommandHandler(ISchemeRepository repository, IConsortiumUserContext accountUserContext)
        : base(repository, accountUserContext, false)
    {
    }

    protected override void Update(SchemeEntity scheme, ChangeSchemeHousingNeedsCommand request)
    {
        scheme.ChangeHousingNeeds(new HousingNeeds(request.MeetingLocalPriorities, request.MeetingLocalHousingNeed));
    }
}
