using HE.Investments.DocumentService.Models;

namespace HE.Investments.DocumentService.Services;

public class MockedDocumentService : IDocumentService
{
    public Task<IEnumerable<FileDetails<TMetadata>>> GetFilesAsync<TMetadata>(GetFilesQuery query, CancellationToken cancellationToken)
        where TMetadata : class
    {
        return Task.FromResult(Enumerable.Empty<FileDetails<TMetadata>>());
    }

    public Task UploadAsync<TMetadata>(
        FileLocation location,
        UploadFileData<TMetadata> file,
        bool overwrite = false,
        CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task DeleteAsync(FileLocation location, string fileName, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task<DownloadFileData> DownloadAsync(FileLocation location, string fileName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task CreateFoldersAsync(string listTitle, List<string> folderPaths, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
