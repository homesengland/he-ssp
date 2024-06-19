using HE.Investment.AHP.Contract.Scheme.Commands;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investments.Consortium.Shared.UserContext;

namespace HE.Investment.AHP.Domain.Scheme.CommandHandlers;

public class CompleteSchemeCommandHandler : UpdateSchemeCommandHandler<CompleteSchemeCommand>
{
    public CompleteSchemeCommandHandler(ISchemeRepository repository, IConsortiumUserContext accountUserContext)
        : base(repository, accountUserContext, false)
    {
    }

    protected override void Update(SchemeEntity scheme, CompleteSchemeCommand request)
    {
        scheme.Complete();
    }
}
