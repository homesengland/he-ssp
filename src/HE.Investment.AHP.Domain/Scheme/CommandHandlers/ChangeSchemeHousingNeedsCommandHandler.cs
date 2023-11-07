using HE.Investment.AHP.Domain.Scheme.Commands;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;

namespace HE.Investment.AHP.Domain.Scheme.CommandHandlers;

public class ChangeSchemeHousingNeedsCommandHandler : UpdateSchemeCommandHandler<ChangeSchemeHousingNeedsCommand>
{
    public ChangeSchemeHousingNeedsCommandHandler(ISchemeRepository repository)
        : base(repository)
    {
    }

    protected override void Update(SchemeEntity scheme, ChangeSchemeHousingNeedsCommand request)
    {
        scheme.ChangeHousingNeeds(new HousingNeeds(request.TypeAndTenureJustification, request.SchemeAndProposalJustification));
    }
}
