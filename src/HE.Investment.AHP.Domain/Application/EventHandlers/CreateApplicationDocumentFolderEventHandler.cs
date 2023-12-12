using HE.Investment.AHP.Contract.Application.Events;
using HE.Investment.AHP.Domain.Documents;
using HE.Investment.AHP.Domain.Documents.Config;
using HE.Investment.AHP.Domain.Documents.Crm;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.DocumentService.Services;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Application.EventHandlers;

public class CreateApplicationDocumentFolderEventHandler : IEventHandler<ApplicationHasBeenCreatedEvent>
{
    private readonly IDocumentsCrmContext _documentsCrmContext;

    private readonly IDocumentService _documentService;

    private readonly IAhpDocumentSettings _documentSettings;

    public CreateApplicationDocumentFolderEventHandler(
        IDocumentsCrmContext documentsCrmContext,
        IDocumentService documentService,
        IAhpDocumentSettings documentSettings)
    {
        _documentsCrmContext = documentsCrmContext;
        _documentService = documentService;
        _documentSettings = documentSettings;
    }

    public async Task Handle(ApplicationHasBeenCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        var rootDirectory = await _documentsCrmContext.GetDocumentLocation(new ApplicationId(domainEvent.ApplicationId), cancellationToken);
        var applicationFolders = AhpFileFolders.ApplicationFolders(rootDirectory).ToList();

        await _documentService.CreateFoldersAsync(
            _documentSettings.ListTitle,
            applicationFolders,
            cancellationToken);
    }
}
