using HE.Investment.AHP.Domain.Scheme.Commands;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;

namespace HE.Investment.AHP.Domain.Scheme.CommandHandlers;

public class ChangeSchemeFundingCommandHandler : UpdateSchemeCommandHandler<ChangeSchemeFundingCommand>
{
    public ChangeSchemeFundingCommandHandler(ISchemeRepository repository)
        : base(repository, false)
    {
    }

    protected override void Update(SchemeEntity scheme, ChangeSchemeFundingCommand request)
    {
        var funding = new SchemeFunding(request.RequiredFunding, request.HousesToDeliver);
        scheme.ChangeFunding(funding);
    }
}
