using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes.Events;
using HE.Investment.AHP.Domain.Documents;
using HE.Investment.AHP.Domain.Documents.Config;
using HE.Investment.AHP.Domain.Documents.Crm;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.DocumentService.Services;

namespace HE.Investment.AHP.Domain.HomeTypes.EventHandlers;

public class CreateHomeTypeDocumentFolderEventHandler : IEventHandler<HomeTypeHasBeenCreatedEvent>
{
    private readonly IDocumentsCrmContext _documentsCrmContext;

    private readonly IDocumentService _documentService;

    private readonly IAhpDocumentSettings _documentSettings;

    public CreateHomeTypeDocumentFolderEventHandler(
        IDocumentsCrmContext documentsCrmContext,
        IDocumentService documentService,
        IAhpDocumentSettings documentSettings)
    {
        _documentsCrmContext = documentsCrmContext;
        _documentService = documentService;
        _documentSettings = documentSettings;
    }

    public async Task Handle(HomeTypeHasBeenCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        var rootDirectory = await _documentsCrmContext.GetDocumentLocation(domainEvent.ApplicationId, cancellationToken);
        var homeTypeFolders = AhpFileFolders.HomeTypeFolders(rootDirectory, domainEvent.HomeTypeId).ToList();

        await _documentService.CreateFoldersAsync(
            _documentSettings.ListTitle,
            homeTypeFolders,
            cancellationToken);
    }
}
