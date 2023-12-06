using HE.Investment.AHP.Domain.Common.ValueObjects;
using HE.Investment.AHP.Domain.Scheme.Commands;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.Repositories;

namespace HE.Investment.AHP.Domain.Scheme.CommandHandlers;

public class RemoveStakeholderDiscussionsFileCommandHandler : UpdateSchemeCommandHandler<RemoveStakeholderDiscussionsFileCommand>
{
    public RemoveStakeholderDiscussionsFileCommandHandler(ISchemeRepository repository)
        : base(repository, true)
    {
    }

    protected override void Update(SchemeEntity scheme, RemoveStakeholderDiscussionsFileCommand request)
    {
        scheme.RemoveStakeholderDiscussionsFile(new FileId(request.FileId));
    }
}
