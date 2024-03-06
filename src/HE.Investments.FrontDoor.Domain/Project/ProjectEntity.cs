using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Errors;
using HE.Investments.Common.Extensions;
using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Domain.Project.Repository;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;

namespace HE.Investments.FrontDoor.Domain.Project;

public class ProjectEntity : DomainEntity
{
    private readonly ModificationTracker _modificationTracker = new();

    public ProjectEntity(
        FrontDoorProjectId id,
        ProjectName name,
        bool? isEnglandHousingDelivery = null,
        SupportActivities? supportActivityTypes = null,
        ProjectAffordableHomesAmount? affordableHomesAmount = null,
        IsSiteIdentified? isSiteIdentified = null)
    {
        Id = id;
        Name = name;
        IsEnglandHousingDelivery = isEnglandHousingDelivery ?? true;
        SupportActivities = supportActivityTypes ?? SupportActivities.Empty();
        AffordableHomesAmount = affordableHomesAmount ?? ProjectAffordableHomesAmount.Empty();
        IsSiteIdentified = isSiteIdentified;
    }

    public FrontDoorProjectId Id { get; private set; }

    public bool IsEnglandHousingDelivery { get; private set; }

    public ProjectName Name { get; private set; }

    public SupportActivities SupportActivities { get; private set; }

    public ProjectAffordableHomesAmount AffordableHomesAmount { get; private set; }

    public IsSiteIdentified? IsSiteIdentified { get; private set; }

    public static async Task<ProjectEntity> New(ProjectName projectName, IProjectNameExists projectNameExists, CancellationToken cancellationToken)
    {
        return new(FrontDoorProjectId.New(), await ValidateProjectName(projectName, projectNameExists, null, cancellationToken));
    }

    public static bool ValidateEnglandHousingDelivery(bool? isEnglandHousingDelivery)
    {
        if (isEnglandHousingDelivery.IsNotProvided())
        {
            OperationResult.ThrowValidationError(nameof(isEnglandHousingDelivery), "Select yes if your project is supporting housing delivery in England");
        }

        return isEnglandHousingDelivery!.Value;
    }

    public void ProvideSupportActivityTypes(SupportActivities supportActivityTypes)
    {
        SupportActivities = _modificationTracker.Change(SupportActivities, supportActivityTypes, null, SupportActivityTypesHaveChanged);
    }

    public void ProvideAffordableHomesAmount(ProjectAffordableHomesAmount affordableHomesAmount)
    {
        AffordableHomesAmount = _modificationTracker.Change(AffordableHomesAmount, affordableHomesAmount);
    }

    public void SetId(FrontDoorProjectId newId)
    {
        if (!Id.IsNew)
        {
            throw new DomainException("Id cannot be modified", CommonErrorCodes.IdCannotBeModified);
        }

        Id = newId;
    }

    public async Task ProvideName(ProjectName projectName, IProjectNameExists projectNameExists, CancellationToken cancellationToken)
    {
        Name = _modificationTracker.Change(Name, await ValidateProjectName(projectName, projectNameExists, Id, cancellationToken));
    }

    public void ProvideIsEnglandHousingDelivery(bool? isEnglandHousingDelivery)
    {
        IsEnglandHousingDelivery = _modificationTracker.Change(IsEnglandHousingDelivery, ValidateEnglandHousingDelivery(isEnglandHousingDelivery));
    }

    public void ProvideIsSiteIdentified(IsSiteIdentified isSiteIdentified)
    {
        IsSiteIdentified = _modificationTracker.Change(IsSiteIdentified, isSiteIdentified);
    }

    private static async Task<ProjectName> ValidateProjectName(
        ProjectName projectName,
        IProjectNameExists projectNameExists,
        FrontDoorProjectId? projectId,
        CancellationToken cancellationToken)
    {
        if (await projectNameExists.DoesExist(projectName, projectId, cancellationToken))
        {
            OperationResult.ThrowValidationError(nameof(Name), "This name has already been used on another project");
        }

        return projectName;
    }

    private void SupportActivityTypesHaveChanged(SupportActivities newSupportActivityTypes)
    {
        if (!newSupportActivityTypes.IsTenureRequired())
        {
            AffordableHomesAmount = ProjectAffordableHomesAmount.Empty();
        }

        if (!newSupportActivityTypes.IsInfrastructureRequired())
        {
            // TODO: Wipe answers #91004: Assess infrastructure delivery (portal user)
        }
    }
}
