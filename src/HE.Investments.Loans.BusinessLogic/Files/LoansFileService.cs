using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Utils;
using HE.Investments.DocumentService.Models;
using HE.Investments.DocumentService.Services;
using HE.Investments.Loans.Contract.Documents;

namespace HE.Investments.Loans.BusinessLogic.Files;

public class LoansFileService<TFileParams> : ILoansFileService<TFileParams>
{
    private readonly IDocumentService _documentService;

    private readonly IDateTimeProvider _dateTimeProvider;

    private readonly IAccountUserContext _userContext;

    private readonly ILoansFileLocationProvider<TFileParams> _fileLocationProvider;

    public LoansFileService(
        IDocumentService documentService,
        IDateTimeProvider dateTimeProvider,
        IAccountUserContext userContext,
        ILoansFileLocationProvider<TFileParams> fileLocationProvider)
    {
        _documentService = documentService;
        _dateTimeProvider = dateTimeProvider;
        _userContext = userContext;
        _fileLocationProvider = fileLocationProvider;
    }

    public async Task<IReadOnlyCollection<UploadedFile>> GetFiles(TFileParams fileParams, CancellationToken cancellationToken)
    {
        var fileLocation = await _fileLocationProvider.GetFilesLocationAsync(fileParams, cancellationToken);
        return await GetFiles(fileLocation, cancellationToken);
    }

    public async Task<UploadedFile> UploadFile(string name, Stream content, TFileParams fileParams, CancellationToken cancellationToken)
    {
        var fileId = FileId.GenerateNew();
        var profileDetails = await _userContext.GetProfileDetails();
        var createdBy = $"{profileDetails.FirstName} {profileDetails.LastName}";
        var fileData = new UploadFileData<LoansFileMetadata>(name, new LoansFileMetadata(fileId.Value, createdBy), content);
        var fileLocation = await _fileLocationProvider.GetFilesLocationAsync(fileParams, cancellationToken);

        await _documentService.UploadAsync(fileLocation, fileData, false, cancellationToken);

        return new UploadedFile(fileId, name, _dateTimeProvider.UtcNow, createdBy);
    }

    public async Task<UploadedFile?> RemoveFile(FileId fileId, TFileParams fileParams, CancellationToken cancellationToken)
    {
        var fileLocation = await _fileLocationProvider.GetFilesLocationAsync(fileParams, cancellationToken);
        var files = await GetFiles(fileLocation, cancellationToken);
        var file = files.FirstOrDefault(x => x.Id == fileId);
        if (file != null)
        {
            await _documentService.DeleteAsync(fileLocation, file.Name, cancellationToken);
        }

        return file;
    }

    public async Task<DownloadFileData> DownloadFile(FileId fileId, TFileParams fileParams, CancellationToken cancellationToken)
    {
        var fileLocation = await _fileLocationProvider.GetFilesLocationAsync(fileParams, cancellationToken);
        var files = await GetFiles(fileLocation, cancellationToken);
        var file = files.FirstOrDefault(x => x.Id == fileId) ?? throw new NotFoundException("File", fileId);

        return await _documentService.DownloadAsync(fileLocation, file.Name, cancellationToken);
    }

    private static UploadedFile MapToUploadedFile(FileDetails<LoansFileMetadata> file)
    {
        return new UploadedFile(
            string.IsNullOrEmpty(file.Metadata?.FileId) ? null : FileId.From(file.Metadata!.FileId),
            file.FileName,
            file.Modified,
            file.Metadata?.Creator ?? string.Empty);
    }

    private async Task<IReadOnlyCollection<UploadedFile>> GetFiles(FileLocation fileLocation, CancellationToken cancellationToken)
    {
        var query = new GetFilesQuery(fileLocation.ListTitle, fileLocation.ListAlias, [fileLocation.FolderPath]);
        var files = await _documentService.GetFilesAsync<LoansFileMetadata>(query, cancellationToken);

        return files.Select(MapToUploadedFile).ToList();
    }
}
