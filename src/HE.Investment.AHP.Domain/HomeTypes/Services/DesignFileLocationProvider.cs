using HE.Investment.AHP.Domain.Documents;
using HE.Investment.AHP.Domain.Documents.Config;
using HE.Investment.AHP.Domain.Documents.Crm;
using HE.Investment.AHP.Domain.Documents.Services;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.DocumentService.Models;

namespace HE.Investment.AHP.Domain.HomeTypes.Services;

public class DesignFileLocationProvider : IAhpFileLocationProvider<DesignFileParams>
{
    private readonly IDocumentsCrmContext _documentsCrmContext;

    private readonly IAhpDocumentSettings _documentSettings;

    public DesignFileLocationProvider(IDocumentsCrmContext documentsCrmContext, IAhpDocumentSettings documentSettings)
    {
        _documentsCrmContext = documentsCrmContext;
        _documentSettings = documentSettings;
    }

    public async Task<FileLocation> GetFileLocation(DesignFileParams fileParams, CancellationToken cancellationToken)
    {
        var rootDirectory = await _documentsCrmContext.GetDocumentLocation(fileParams.ApplicationId, cancellationToken);
        var folderPath = AhpFileFolders.DesignFilesFolder(rootDirectory, fileParams.HomeTypeId);

        return new FileLocation(_documentSettings.ListTitle, _documentSettings.ListAlias, folderPath);
    }
}
