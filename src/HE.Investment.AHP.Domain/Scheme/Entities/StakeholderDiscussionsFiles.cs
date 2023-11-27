using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Common.Entities;
using HE.Investment.AHP.Domain.Common.FilePolicies;
using HE.Investment.AHP.Domain.Mock;
using DomainApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Scheme.Entities;

public class StakeholderDiscussionsFiles : Files<StakeholderDiscussionsFile>
{
    public StakeholderDiscussionsFiles(UploadedFile? uploadedFile = null)
        : base(uploadedFile == null ? null : new List<UploadedFile> { uploadedFile })
    {
    }

    protected override IFilePolicy<int> FilesCountPolicy => new SingleFileCountPolicy(nameof(StakeholderDiscussionsFiles));

    public async Task SaveChanges(DomainApplicationId applicationId, IFileService fileService, CancellationToken cancellationToken)
    {
        await SaveChanges(
            async file => await file.Upload(applicationId, fileService, cancellationToken),
            async fileId => await fileService.RemoveFile(applicationId, fileId, cancellationToken));
    }
}
