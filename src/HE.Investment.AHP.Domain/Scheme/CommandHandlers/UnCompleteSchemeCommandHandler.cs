using HE.Investment.AHP.Contract.Scheme.Commands;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investments.Account.Shared;

namespace HE.Investment.AHP.Domain.Scheme.CommandHandlers;

public class UnCompleteSchemeCommandHandler : UpdateSchemeCommandHandler<UnCompleteSchemeCommand>
{
    public UnCompleteSchemeCommandHandler(ISchemeRepository repository, IAccountUserContext accountUserContext)
        : base(repository, accountUserContext, false)
    {
    }

    protected override void Update(SchemeEntity scheme, UnCompleteSchemeCommand request)
    {
        scheme.SetInProgress();
    }
}
