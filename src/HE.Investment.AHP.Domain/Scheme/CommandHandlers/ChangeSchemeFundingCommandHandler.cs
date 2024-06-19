using HE.Investment.AHP.Contract.Scheme.Commands;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investments.Consortium.Shared.UserContext;

namespace HE.Investment.AHP.Domain.Scheme.CommandHandlers;

public class ChangeSchemeFundingCommandHandler : UpdateSchemeCommandHandler<ChangeSchemeFundingCommand>
{
    public ChangeSchemeFundingCommandHandler(ISchemeRepository repository, IConsortiumUserContext accountUserContext)
        : base(repository, accountUserContext, false)
    {
    }

    protected override void Update(SchemeEntity scheme, ChangeSchemeFundingCommand request)
    {
        var funding = new SchemeFunding(request.RequiredFunding, request.HousesToDeliver);
        scheme.ProvideFunding(funding);
    }
}
