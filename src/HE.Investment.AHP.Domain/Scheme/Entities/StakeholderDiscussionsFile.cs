using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Common.Entities;
using HE.Investment.AHP.Domain.Common.FilePolicies;
using HE.Investment.AHP.Domain.Common.ValueObjects;
using HE.Investment.AHP.Domain.Mock;
using HE.Investments.Common.Extensions;
using DomainApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Scheme.Entities;

public class StakeholderDiscussionsFile : FileEntity
{
    private StakeholderDiscussionsFile(FileName name, FileSize size, Stream content)
        : base(name, size, content, new ValidFileExtensionPolicy(nameof(StakeholderDiscussionsFiles)), new FileSizePolicy(nameof(StakeholderDiscussionsFiles)))
    {
    }

    public static StakeholderDiscussionsFile ForUpload(FileName name, FileSize size, Stream content) => new(name, size, content);

    public async Task<UploadedFile> Upload(
        DomainApplicationId applicationId,
        IFileService fileService,
        CancellationToken cancellationToken)
    {
        if (Id.IsProvided())
        {
            throw new InvalidOperationException($"File {Name} is already uploaded with Id {Id}.");
        }

        var uploadedFile = await fileService.UploadFile(applicationId, Name, Content, cancellationToken);
        Id = uploadedFile.Id;

        return uploadedFile;
    }
}
