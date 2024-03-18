extern alias Org;

using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Errors;
using HE.Investments.FrontDoor.Contract.Site;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Domain.Site.ValueObjects;
using HE.Investments.FrontDoor.Shared.Project;
using Org::HE.Investments.Organisation.LocalAuthorities.ValueObjects;

namespace HE.Investments.FrontDoor.Domain.Site;

public class ProjectSiteEntity
{
    private readonly ModificationTracker _modificationTracker = new();

    public ProjectSiteEntity(
        FrontDoorProjectId projectId,
        FrontDoorSiteId id,
        SiteName name,
        DateTime? createdOn = null,
        HomesNumber? homesNumber = null,
        PlanningStatus? planningStatus = null,
        LocalAuthorityId? localAuthorityId = null)
    {
        Id = id;
        ProjectId = projectId;
        Name = name;
        CreatedOn = createdOn;
        HomesNumber = homesNumber;
        LocalAuthorityId = localAuthorityId;
        PlanningStatus = planningStatus ?? PlanningStatus.Empty();
    }

    public FrontDoorSiteId Id { get; private set; }

    public FrontDoorProjectId ProjectId { get; }

    public SiteName Name { get; private set; }

    public HomesNumber? HomesNumber { get; private set; }

    public DateTime? CreatedOn { get; }

    public PlanningStatus PlanningStatus { get; private set; }

    public LocalAuthorityId? LocalAuthorityId { get; set; }

    public void ProvideName(SiteName siteName)
    {
        Name = _modificationTracker.Change(Name, siteName);
    }

    public void SetId(FrontDoorSiteId newId)
    {
        if (!Id.IsNew)
        {
            throw new DomainException("Id cannot be modified", CommonErrorCodes.IdCannotBeModified);
        }

        Id = newId;
    }

    public void ProvidePlanningStatus(PlanningStatus planningStatus)
    {
        PlanningStatus = _modificationTracker.Change(PlanningStatus, planningStatus);
    }

    public void ProvideHomesNumber(HomesNumber homesNumber)
    {
        HomesNumber = _modificationTracker.Change(HomesNumber, homesNumber);
    }

    public void ProvideLocalAuthority(LocalAuthorityId localAuthorityId)
    {
        LocalAuthorityId = _modificationTracker.Change(LocalAuthorityId, localAuthorityId);
    }
}
