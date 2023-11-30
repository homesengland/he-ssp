using HE.Investment.AHP.Domain.Common.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Extensions;
using HE.Investments.DocumentService.Models;
using HE.Investments.DocumentService.Services;
using HE.Investments.Loans.Common.Utils;

namespace HE.Investment.AHP.Domain.Common.Services;

public class AhpFileService : IAhpFileService
{
    private readonly IDocumentService _documentService;

    private readonly IDateTimeProvider _dateTimeProvider;

    private readonly IAccountUserContext _userContext;

    public AhpFileService(IDocumentService documentService, IDateTimeProvider dateTimeProvider, IAccountUserContext userContext)
    {
        _documentService = documentService;
        _dateTimeProvider = dateTimeProvider;
        _userContext = userContext;
    }

    public async Task<IReadOnlyCollection<UploadedFile>> GetFiles(FileLocation fileLocation, CancellationToken cancellationToken)
    {
        var query = new GetFilesQuery(fileLocation.ListTitle, fileLocation.ListAlias, new List<string> { fileLocation.FolderPath });
        var files = await _documentService.GetFilesAsync<AhpFileMetadata>(query, cancellationToken);

        return files.Select(MapToUploadedFile).ToList();
    }

    public async Task<UploadedFile> UploadFile(FileName name, Stream content, FileLocation fileLocation, CancellationToken cancellationToken)
    {
        var fileId = FileId.New();
        var profileDetails = await _userContext.GetProfileDetails();
        var createdBy = $"{profileDetails.FirstName} {profileDetails.LastName}";
        var fileData = new UploadFileData<AhpFileMetadata>(name.Value, new AhpFileMetadata(fileId.Value, createdBy), content);

        await _documentService.UploadAsync(fileLocation, fileData, false, cancellationToken);

        return new UploadedFile(fileId, name, _dateTimeProvider.UtcNow, createdBy);
    }

    public async Task RemoveFile(FileId fileId, FileLocation fileLocation, CancellationToken cancellationToken)
    {
        var files = await GetFiles(fileLocation, cancellationToken);
        var file = files.FirstOrDefault(x => x.Id == fileId);
        if (file != null)
        {
            await _documentService.DeleteAsync(fileLocation, file.Name.Value, cancellationToken);
        }
    }

    private static UploadedFile MapToUploadedFile(FileDetails<AhpFileMetadata> file)
    {
        if (file.Metadata.IsNotProvided())
        {
            throw new InvalidOperationException("AHP File returned from Document Service is missing required metadata.");
        }

        return new UploadedFile(
            new FileId(file.Metadata!.FileId),
            new FileName(file.FileName),
            file.Modified,
            file.Metadata!.CreatedBy);
    }
}
