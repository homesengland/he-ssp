using HE.Investments.DocumentService.Models;

namespace HE.Investments.DocumentService.Services;

public interface IDocumentService
{
    Task<IEnumerable<FileDetails<TMetadata>>> GetFilesAsync<TMetadata>(GetFilesQuery query, CancellationToken cancellationToken)
        where TMetadata : class;

    Task UploadAsync<TMetadata>(
        FileLocation location,
        UploadFileData<TMetadata> file,
        bool overwrite,
        CancellationToken cancellationToken);

    Task DeleteAsync(FileLocation location, string fileName, CancellationToken cancellationToken);

    Task<DownloadFileData> DownloadAsync(FileLocation location, string fileName, CancellationToken cancellationToken);

    Task CreateFoldersAsync(string listTitle, List<string> folderPaths, CancellationToken cancellationToken);
}
