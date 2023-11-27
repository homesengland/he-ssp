using HE.Investment.AHP.Domain.Common.ValueObjects;
using HE.Investment.AHP.Domain.Scheme.Commands;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.Scheme.CommandHandlers;

public class ChangeSchemeStakeholderDiscussionsCommandHandler : UpdateSchemeCommandHandler<ChangeSchemeStakeholderDiscussionsCommand>
{
    public ChangeSchemeStakeholderDiscussionsCommandHandler(ISchemeRepository repository)
        : base(repository)
    {
    }

    protected override void Update(SchemeEntity scheme, ChangeSchemeStakeholderDiscussionsCommand request)
    {
        var operationResult = new OperationResult();

        var discussion = operationResult.Aggregate(() => new StakeholderDiscussions(request.DiscussionReport));

        var file = request.FileToUpload != null
            ? operationResult.Aggregate(() =>
                StakeholderDiscussionsFile.ForUpload(
                    new FileName(request.FileToUpload.Name),
                    new FileSize(request.FileToUpload.Lenght),
                    request.FileToUpload.Content))
            : null;

        scheme.ChangeStakeholderDiscussions(discussion, file);
        operationResult.CheckErrors();
    }
}
