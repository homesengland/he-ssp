using System.Text.Json;
using HE.Investments.DocumentService.Models;

namespace HE.Investments.DocumentService.Services;

public class MockedDocumentService : IDocumentService
{
    private static readonly IDictionary<string, MockedFile> Files = new Dictionary<string, MockedFile>();

    public Task<IEnumerable<FileDetails<TMetadata>>> GetFilesAsync<TMetadata>(GetFilesQuery query, CancellationToken cancellationToken)
        where TMetadata : class
    {
        var fileKeys = GenerateFileKeys(query.ListTitle, query.FolderPaths);
        var files = Files.Where(file => fileKeys.Any(fileKey => file.Key.StartsWith(fileKey, StringComparison.InvariantCulture)))
            .Select(x => new FileDetails<TMetadata>(
                1,
                x.Value.FileName,
                string.Empty,
                0,
                "Unknown",
                x.Value.CreatedOn,
                JsonSerializer.Deserialize<TMetadata>(x.Value.Metadata)))
            .ToList();

        return Task.FromResult<IEnumerable<FileDetails<TMetadata>>>(files);
    }

    public Task UploadAsync<TMetadata>(
        FileLocation location,
        UploadFileData<TMetadata> file,
        bool overwrite = false,
        CancellationToken cancellationToken = default)
    {
        var fileKey = GenerateFileKey(location, file.Name);
        if (!Files.ContainsKey(fileKey) || overwrite)
        {
            Files[fileKey] = new MockedFile(file.Name, DateTime.UtcNow, JsonSerializer.Serialize(file.Metadata));
        }

        return Task.CompletedTask;
    }

    public Task DeleteAsync(FileLocation location, string fileName, CancellationToken cancellationToken)
    {
        var fileKey = GenerateFileKey(location, fileName);
        if (Files.ContainsKey(fileKey))
        {
            Files.Remove(fileKey);
        }

        return Task.CompletedTask;
    }

    public Task<DownloadFileData> DownloadAsync(FileLocation location, string fileName, CancellationToken cancellationToken)
    {
        return Task.FromResult(new DownloadFileData(fileName, new MemoryStream()));
    }

    public Task CreateFoldersAsync(string listTitle, List<string> folderPaths, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private static string GenerateFileKey(FileLocation location, string fileName)
    {
        return $"{location.ListTitle}/{location.FolderPath}/{fileName}";
    }

    private static IList<string> GenerateFileKeys(string listTitle, IEnumerable<string> folderPaths)
    {
        return folderPaths.Select(x => $"{listTitle}/{x}/").ToList();
    }

    private record MockedFile(string FileName, DateTime CreatedOn, string Metadata);
}
