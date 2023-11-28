using System.Collections.Concurrent;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Common.ValueObjects;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.HomeTypes.Services;

public class DesignFileService : IDesignFileService
{
    // TODO: AB#65910 use http document service to store files
    private static readonly IDictionary<string, UploadedFile> Documents = new ConcurrentDictionary<string, UploadedFile>();

    public Task<IReadOnlyCollection<UploadedFile>> GetByHomeTypeId(ApplicationId applicationId, HomeTypeId homeTypeId, CancellationToken cancellationToken)
    {
        return Task.FromResult<IReadOnlyCollection<UploadedFile>>(Documents
            .Where(x => x.Key.StartsWith($"{applicationId}-{homeTypeId.Value}-", StringComparison.InvariantCulture))
            .Select(x => x.Value)
            .ToList());
    }

    public async Task<UploadedFile> UploadFile(ApplicationId applicationId, HomeTypeId homeTypeId, FileName name, Stream content, CancellationToken cancellationToken)
    {
        await Task.Delay(2000, cancellationToken);

        var fileId = new FileId(Guid.NewGuid().ToString());
        var uploadedFile = new UploadedFile(fileId, name, DateTime.Now, "Test User");

        Documents[$"{applicationId}-{homeTypeId.Value}-{fileId.Value}"] = uploadedFile;
        return uploadedFile;
    }

    public async Task RemoveFile(ApplicationId applicationId, HomeTypeId homeTypeId, FileId fileId, CancellationToken cancellationToken)
    {
        await Task.Delay(1000, cancellationToken);

        Documents.Remove($"{applicationId}-{homeTypeId.Value}-{fileId.Value}");
    }
}
