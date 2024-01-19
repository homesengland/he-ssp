using HE.Investment.AHP.Contract.Scheme.Commands;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Domain.Scheme.CommandHandlers;

public class RemoveStakeholderDiscussionsFileCommandHandler : UpdateSchemeCommandHandler<RemoveStakeholderDiscussionsFileCommand>
{
    public RemoveStakeholderDiscussionsFileCommandHandler(ISchemeRepository repository, IAccountUserContext accountUserContext)
        : base(repository, accountUserContext, true)
    {
    }

    protected override void Update(SchemeEntity scheme, RemoveStakeholderDiscussionsFileCommand request)
    {
        scheme.RemoveStakeholderDiscussionsFile(request.FileId);
    }
}
