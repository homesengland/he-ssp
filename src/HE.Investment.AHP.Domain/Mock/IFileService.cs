using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Common.ValueObjects;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Mock;

public interface IFileService
{
    Task<IReadOnlyCollection<UploadedFile>> GetByApplicationId(ApplicationId applicationId, CancellationToken cancellationToken);

    Task<UploadedFile> UploadFile(ApplicationId applicationId, FileName name, Stream content, CancellationToken cancellationToken);

    Task RemoveFile(ApplicationId applicationId, FileId fileId, CancellationToken cancellationToken);
}
