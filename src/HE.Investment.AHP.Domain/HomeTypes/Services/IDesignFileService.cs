using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Common.ValueObjects;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.HomeTypes.Services;

public interface IDesignFileService
{
    Task<IReadOnlyCollection<UploadedFile>> GetByHomeTypeId(ApplicationId applicationId, HomeTypeId homeTypeId, CancellationToken cancellationToken);

    Task<UploadedFile> UploadFile(
        ApplicationId applicationId,
        HomeTypeId homeTypeId,
        FileName name,
        Stream content,
        CancellationToken cancellationToken);

    Task RemoveFile(ApplicationId applicationId, HomeTypeId homeTypeId, FileId fileId, CancellationToken cancellationToken);
}
