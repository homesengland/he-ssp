using HE.Investment.AHP.Domain.Scheme.Commands;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;

namespace HE.Investment.AHP.Domain.Scheme.CommandHandlers;

public class ChangeSchemeFundingCommandHandler : UpdateSchemeCommandHandler<ChangeSchemeFundingCommand>
{
    public ChangeSchemeFundingCommandHandler(ISchemeRepository repository)
        : base(repository)
    {
    }

    protected override void Update(SchemeEntity scheme, ChangeSchemeFundingCommand request)
    {
        scheme.ChangeFunding(new SchemeFunding(request.RequiredFunding, request.HousesToDeliver));
    }
}
