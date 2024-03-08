using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Errors;
using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Contract.Site;
using HE.Investments.FrontDoor.Domain.Site.ValueObjects;

namespace HE.Investments.FrontDoor.Domain.Site;

public class ProjectSiteEntity
{
    private readonly ModificationTracker _modificationTracker = new();

    public ProjectSiteEntity(FrontDoorSiteId id, FrontDoorProjectId projectId, SiteName name, DateTime? createdOn = null)
    {
        Id = id;
        ProjectId = projectId;
        Name = name;
        CreatedOn = createdOn;
    }

    public FrontDoorSiteId Id { get; private set; }

    public FrontDoorProjectId ProjectId { get; }

    public SiteName Name { get; private set; }

    public DateTime? CreatedOn { get; }

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
}
