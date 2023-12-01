using HE.Investment.AHP.Domain.Scheme.Commands;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.Repositories;

namespace HE.Investment.AHP.Domain.Scheme.CommandHandlers;

public class CompleteSchemeCommandHandler : UpdateSchemeCommandHandler<CompleteSchemeCommand>
{
    public CompleteSchemeCommandHandler(ISchemeRepository repository)
        : base(repository)
    {
    }

    protected override void Update(SchemeEntity scheme, CompleteSchemeCommand request)
    {
        scheme.Complete();
    }
}
