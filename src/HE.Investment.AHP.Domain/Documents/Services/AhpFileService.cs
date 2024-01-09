using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Common.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Utils;
using HE.Investments.DocumentService.Models;
using HE.Investments.DocumentService.Services;

namespace HE.Investment.AHP.Domain.Documents.Services;

public class AhpFileService<TFileParams> : IAhpFileService<TFileParams>
{
    private readonly IDocumentService _documentService;

    private readonly IDateTimeProvider _dateTimeProvider;

    private readonly IAccountUserContext _userContext;

    private readonly IAhpFileLocationProvider<TFileParams> _fileLocationProvider;

    public AhpFileService(
        IDocumentService documentService,
        IDateTimeProvider dateTimeProvider,
        IAccountUserContext userContext,
        IAhpFileLocationProvider<TFileParams> fileLocationProvider)
    {
        _documentService = documentService;
        _dateTimeProvider = dateTimeProvider;
        _userContext = userContext;
        _fileLocationProvider = fileLocationProvider;
    }

    public async Task<IReadOnlyCollection<UploadedFile>> GetFiles(TFileParams fileParams, CancellationToken cancellationToken)
    {
        var fileLocation = await _fileLocationProvider.GetFileLocation(fileParams, cancellationToken);
        return await GetFiles(fileLocation, cancellationToken);
    }

    public async Task<UploadedFile> UploadFile(FileName name, Stream content, TFileParams fileParams, CancellationToken cancellationToken)
    {
        var fileId = FileId.New();
        var profileDetails = await _userContext.GetProfileDetails();
        var createdBy = $"{profileDetails.FirstName} {profileDetails.LastName}";
        var fileData = new UploadFileData<AhpFileMetadata>(name.Value, new AhpFileMetadata(fileId.Value, createdBy), content);
        var fileLocation = await _fileLocationProvider.GetFileLocation(fileParams, cancellationToken);

        await _documentService.UploadAsync(fileLocation, fileData, false, cancellationToken);

        return new UploadedFile(fileId, name, _dateTimeProvider.UtcNow, createdBy);
    }

    public async Task RemoveFile(FileId fileId, TFileParams fileParams, CancellationToken cancellationToken)
    {
        var fileLocation = await _fileLocationProvider.GetFileLocation(fileParams, cancellationToken);
        var files = await GetFiles(fileLocation, cancellationToken);
        var file = files.FirstOrDefault(x => x.Id == fileId);
        if (file != null)
        {
            await _documentService.DeleteAsync(fileLocation, file.Name.Value, cancellationToken);
        }
    }

    public async Task<DownloadFileData> DownloadFile(FileId fileId, TFileParams fileParams, CancellationToken cancellationToken)
    {
        var fileLocation = await _fileLocationProvider.GetFileLocation(fileParams, cancellationToken);
        var files = await GetFiles(fileLocation, cancellationToken);
        var file = files.FirstOrDefault(x => x.Id == fileId) ?? throw new NotFoundException("File", fileId);

        return await _documentService.DownloadAsync(fileLocation, file.Name.Value, cancellationToken);
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

    private async Task<IReadOnlyCollection<UploadedFile>> GetFiles(FileLocation fileLocation, CancellationToken cancellationToken)
    {
        var query = new GetFilesQuery(fileLocation.ListTitle, fileLocation.ListAlias, new List<string> { fileLocation.FolderPath });
        var files = await _documentService.GetFilesAsync<AhpFileMetadata>(query, cancellationToken);

        return files.Select(MapToUploadedFile).ToList();
    }
}
