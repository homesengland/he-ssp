using HE.Investment.AHP.Contract.Scheme.Commands;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investments.Consortium.Shared.UserContext;

namespace HE.Investment.AHP.Domain.Scheme.CommandHandlers;

public class RemoveStakeholderDiscussionsFileCommandHandler : UpdateSchemeCommandHandler<RemoveStakeholderDiscussionsFileCommand>
{
    public RemoveStakeholderDiscussionsFileCommandHandler(ISchemeRepository repository, IConsortiumUserContext accountUserContext)
        : base(repository, accountUserContext, true)
    {
    }

    protected override void Update(SchemeEntity scheme, RemoveStakeholderDiscussionsFileCommand request)
    {
        scheme.RemoveStakeholderDiscussionsFile(request.FileId);
    }
}
