using System.Collections.Concurrent;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Common.ValueObjects;
using DomainApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Mock;

public class FileService : IFileService
{
    // TODO: AB#65910 use http document service to store files
    private static readonly IDictionary<string, UploadedFile> Documents = new ConcurrentDictionary<string, UploadedFile>();

    public Task<IReadOnlyCollection<UploadedFile>> GetByApplicationId(DomainApplicationId applicationId, CancellationToken cancellationToken)
    {
        return Task.FromResult<IReadOnlyCollection<UploadedFile>>(Documents
            .Where(x => x.Key.StartsWith($"{applicationId}-", StringComparison.InvariantCulture))
            .Select(x => x.Value)
            .ToList());
    }

    public Task<UploadedFile> UploadFile(DomainApplicationId applicationId, FileName name, Stream content, CancellationToken cancellationToken)
    {
        var fileId = new FileId(Guid.NewGuid().ToString());
        var uploadedFile = new UploadedFile(fileId, name, DateTime.Now, "Test User");

        Documents[$"{applicationId}-{fileId.Value}"] = uploadedFile;
        return Task.FromResult(uploadedFile);
    }

    public Task RemoveFile(DomainApplicationId applicationId, FileId fileId, CancellationToken cancellationToken)
    {
        Documents.Remove($"{applicationId}-{fileId.Value}");
        return Task.CompletedTask;
    }
}
