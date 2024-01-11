using HE.Investment.AHP.Contract.Scheme.Commands;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investments.Account.Shared;

namespace HE.Investment.AHP.Domain.Scheme.CommandHandlers;

public class ChangeSchemeAffordabilityCommandHandler : UpdateSchemeCommandHandler<ChangeSchemeAffordabilityCommand>
{
    public ChangeSchemeAffordabilityCommandHandler(ISchemeRepository repository, IAccountUserContext accountUserContext)
        : base(repository, accountUserContext, false)
    {
    }

    protected override void Update(SchemeEntity scheme, ChangeSchemeAffordabilityCommand request)
    {
        scheme.ChangeAffordabilityEvidence(new AffordabilityEvidence(request.AffordabilityEvidence));
    }
}
