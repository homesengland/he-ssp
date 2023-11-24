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

        var files = new List<StakeholderDiscussionsFile>();
        foreach (var file in request.FilesToUpload)
        {
            files.Add(operationResult.Aggregate(() => StakeholderDiscussionsFile.ForUpload(new FileName(file.Name), new FileSize(file.Lenght), file.Content)));
        }

        scheme.ChangeStakeholderDiscussions(operationResult.Aggregate(() => new StakeholderDiscussions(request.DiscussionReport)), files);
        operationResult.CheckErrors();
    }
}
