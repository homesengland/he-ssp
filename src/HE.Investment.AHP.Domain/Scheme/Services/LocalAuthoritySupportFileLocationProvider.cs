using HE.Investment.AHP.Domain.Documents;
using HE.Investment.AHP.Domain.Documents.Config;
using HE.Investment.AHP.Domain.Documents.Crm;
using HE.Investment.AHP.Domain.Documents.Services;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investments.DocumentService.Models;

namespace HE.Investment.AHP.Domain.Scheme.Services;

public class LocalAuthoritySupportFileLocationProvider : IAhpFileLocationProvider<LocalAuthoritySupportFileParams>
{
    private readonly IDocumentsCrmContext _documentsCrmContext;

    private readonly IAhpDocumentSettings _documentSettings;

    public LocalAuthoritySupportFileLocationProvider(IDocumentsCrmContext documentsCrmContext, IAhpDocumentSettings documentSettings)
    {
        _documentsCrmContext = documentsCrmContext;
        _documentSettings = documentSettings;
    }

    public async Task<FileLocation> GetFileLocation(LocalAuthoritySupportFileParams fileParams, CancellationToken cancellationToken)
    {
        var rootDirectory = await _documentsCrmContext.GetDocumentLocation(fileParams.ApplicationId, cancellationToken);
        var folderPath = AhpFileFolders.LocalAuthoritySupportFilesFolder(rootDirectory);

        return new FileLocation(_documentSettings.ListTitle, _documentSettings.ListAlias, folderPath);
    }
}
