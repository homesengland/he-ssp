using HE.Investment.AHP.Domain.Scheme.Commands;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;

namespace HE.Investment.AHP.Domain.Scheme.CommandHandlers;

public class ChangeSchemeAffordabilityCommandHandler : UpdateSchemeCommandHandler<ChangeSchemeAffordabilityCommand>
{
    public ChangeSchemeAffordabilityCommandHandler(ISchemeRepository repository)
        : base(repository, false)
    {
    }

    protected override void Update(SchemeEntity scheme, ChangeSchemeAffordabilityCommand request)
    {
        scheme.ChangeAffordabilityEvidence(new AffordabilityEvidence(request.AffordabilityEvidence));
    }
}
