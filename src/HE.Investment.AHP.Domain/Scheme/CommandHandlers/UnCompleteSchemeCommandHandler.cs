using HE.Investment.AHP.Domain.Scheme.Commands;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.Repositories;

namespace HE.Investment.AHP.Domain.Scheme.CommandHandlers;

public class UnCompleteSchemeCommandHandler : UpdateSchemeCommandHandler<UnCompleteSchemeCommand>
{
    public UnCompleteSchemeCommandHandler(ISchemeRepository repository)
        : base(repository, false)
    {
    }

    protected override void Update(SchemeEntity scheme, UnCompleteSchemeCommand request)
    {
        scheme.UnComplete();
    }
}
