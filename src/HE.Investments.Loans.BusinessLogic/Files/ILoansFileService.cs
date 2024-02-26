using HE.Investments.Common.Contract;
using HE.Investments.DocumentService.Models;
using HE.Investments.Loans.Contract.Documents;

namespace HE.Investments.Loans.BusinessLogic.Files;

public interface ILoansFileService<in TFileParams>
{
    Task<IReadOnlyCollection<UploadedFile>> GetFiles(TFileParams fileParams, CancellationToken cancellationToken);

    Task<UploadedFile> UploadFile(string name, Stream content, TFileParams fileParams, CancellationToken cancellationToken);

    Task<UploadedFile?> RemoveFile(FileId fileId, TFileParams fileParams, CancellationToken cancellationToken);

    Task<DownloadFileData> DownloadFile(FileId fileId, TFileParams fileParams, CancellationToken cancellationToken);
}
