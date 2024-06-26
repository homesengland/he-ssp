using HE.Investment.AHP.Contract.Scheme.Commands;
using HE.Investment.AHP.Domain.Common.ValueObjects;
using HE.Investment.AHP.Domain.Documents.Config;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Consortium.Shared.UserContext;

namespace HE.Investment.AHP.Domain.Scheme.CommandHandlers;

public class ChangeSchemeStakeholderDiscussionsCommandHandler : UpdateSchemeCommandHandler<ChangeSchemeStakeholderDiscussionsCommand>
{
    private readonly IAhpDocumentSettings _documentSettings;

    public ChangeSchemeStakeholderDiscussionsCommandHandler(ISchemeRepository repository, IConsortiumUserContext accountUserContext, IAhpDocumentSettings documentSettings)
        : base(repository, accountUserContext, true)
    {
        _documentSettings = documentSettings;
    }

    protected override void Update(SchemeEntity scheme, ChangeSchemeStakeholderDiscussionsCommand request)
    {
        var operationResult = new OperationResult();

        var details = operationResult.Aggregate(() => new StakeholderDiscussionsDetails(request.DiscussionReport));

        var file = request.FileToUpload != null ?
            operationResult.Aggregate(() =>
                            new LocalAuthoritySupportFile(
                                new FileName(request.FileToUpload.Name),
                                new FileSize(request.FileToUpload.Lenght),
                                request.FileToUpload.Content,
                                _documentSettings))
                : null;

        scheme.ChangeStakeholderDiscussions(details, file);
        operationResult.CheckErrors();
    }
}
