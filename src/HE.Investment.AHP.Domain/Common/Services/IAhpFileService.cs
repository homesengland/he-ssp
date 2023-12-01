using HE.Investment.AHP.Domain.Common.ValueObjects;
using HE.Investments.DocumentService.Models;

namespace HE.Investment.AHP.Domain.Common.Services;

public interface IAhpFileService<in TFileParams>
{
    Task<IReadOnlyCollection<UploadedFile>> GetFiles(TFileParams fileParams, CancellationToken cancellationToken);

    Task<UploadedFile> UploadFile(FileName name, Stream content, TFileParams fileParams, CancellationToken cancellationToken);

    Task RemoveFile(FileId fileId, TFileParams fileParams, CancellationToken cancellationToken);

    Task<DownloadFileData> DownloadFile(FileId fileId, TFileParams fileParams, CancellationToken cancellationToken);
}
