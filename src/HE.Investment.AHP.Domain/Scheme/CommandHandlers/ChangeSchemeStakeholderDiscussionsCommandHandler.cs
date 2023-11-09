using HE.Investment.AHP.Domain.Scheme.Commands;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.Repositories;

namespace HE.Investment.AHP.Domain.Scheme.CommandHandlers;

public class ChangeSchemeStakeholderDiscussionsCommandHandler : UpdateSchemeCommandHandler<ChangeSchemeStakeholderDiscussionsCommand>
{
    public ChangeSchemeStakeholderDiscussionsCommandHandler(ISchemeRepository repository)
        : base(repository)
    {
    }

    protected override void Update(SchemeEntity scheme, ChangeSchemeStakeholderDiscussionsCommand request)
    {
        scheme.ChangeStakeholderDiscussions(new(request.DiscussionReport));
    }
}
