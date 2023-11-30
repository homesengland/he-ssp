using HE.Investment.AHP.Domain.Common.ValueObjects;
using HE.Investments.DocumentService.Models;

namespace HE.Investment.AHP.Domain.Common.Services;

public interface IAhpFileService
{
    Task<IReadOnlyCollection<UploadedFile>> GetFiles(FileLocation fileLocation, CancellationToken cancellationToken);

    Task<UploadedFile> UploadFile(FileName name, Stream content, FileLocation fileLocation, CancellationToken cancellationToken);

    Task RemoveFile(FileId fileId, FileLocation fileLocation, CancellationToken cancellationToken);
}
