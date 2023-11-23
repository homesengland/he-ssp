using HE.Investment.AHP.Domain.Common.ValueObjects;
using HE.Investment.AHP.Domain.Scheme.Commands;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.Repositories;

namespace HE.Investment.AHP.Domain.Scheme.CommandHandlers;

public class RemoveStakeholderDiscussionsFileCommandHandler : UpdateSchemeCommandHandler<RemoveStakeholderDiscussionsFileCommand>
{
    public RemoveStakeholderDiscussionsFileCommandHandler(ISchemeRepository repository)
        : base(repository)
    {
    }

    protected override void Update(SchemeEntity scheme, RemoveStakeholderDiscussionsFileCommand request)
    {
        scheme.StakeholderDiscussionsFiles.MarkFileToRemove(new FileId(request.FileId));
    }
}
