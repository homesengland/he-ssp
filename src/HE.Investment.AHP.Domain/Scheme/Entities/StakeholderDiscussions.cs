using HE.Investment.AHP.Domain.Common.ValueObjects;
using HE.Investment.AHP.Domain.Documents.Services;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investments.Common.Domain;
using DomainApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Scheme.Entities;

public class StakeholderDiscussions
{
    private readonly ModificationTracker _modificationTracker = new();

    public StakeholderDiscussions(StakeholderDiscussionsDetails stakeholderDiscussionsDetails, LocalAuthoritySupportFileContainer localAuthoritySupportFileContainer)
    {
        StakeholderDiscussionsDetails = stakeholderDiscussionsDetails;
        LocalAuthoritySupportFileContainer = localAuthoritySupportFileContainer;
    }

    public StakeholderDiscussionsDetails StakeholderDiscussionsDetails { get; private set; }

    public LocalAuthoritySupportFileContainer LocalAuthoritySupportFileContainer { get; }

    public bool IsModified => _modificationTracker.IsModified || LocalAuthoritySupportFileContainer.IsModified;

    public void ChangeStakeholderDiscussionsDetails(StakeholderDiscussionsDetails details)
    {
        StakeholderDiscussionsDetails = _modificationTracker.Change(StakeholderDiscussionsDetails, details);
    }

    public void ChangeLocalAuthoritySupportFile(LocalAuthoritySupportFile localAuthoritySupportFile)
    {
        LocalAuthoritySupportFileContainer.Add(localAuthoritySupportFile);
    }

    public void MarkFileToRemove(FileId fileId)
    {
        LocalAuthoritySupportFileContainer.MarkFileToRemove(fileId);
    }

    public void CheckIsComplete()
    {
        StakeholderDiscussionsDetails.CheckIsComplete();
    }

    public async Task SaveChanges(
        DomainApplicationId applicationId,
        IAhpFileService<LocalAuthoritySupportFileParams> fileService,
        CancellationToken cancellationToken)
    {
        await LocalAuthoritySupportFileContainer.SaveChanges(applicationId, fileService, cancellationToken);
    }
}
