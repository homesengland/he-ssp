extern alias Org;

using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Errors;
using HE.Investments.Common.Extensions;
using HE.Investments.FrontDoor.Contract.Project.Events;
using HE.Investments.FrontDoor.Domain.Project.Repository;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Shared.Project;
using Org::HE.Investments.Organisation.LocalAuthorities.ValueObjects;
using ProjectGeographicFocus = HE.Investments.FrontDoor.Domain.Project.ValueObjects.ProjectGeographicFocus;

namespace HE.Investments.FrontDoor.Domain.Project;

public class ProjectEntity : DomainEntity
{
    private readonly ModificationTracker _modificationTracker = new();

    public ProjectEntity(
        FrontDoorProjectId id,
        ProjectName name,
        bool? isEnglandHousingDelivery = null,
        SupportActivities? supportActivityTypes = null,
        ProjectInfrastructure? infrastructureTypes = null,
        ProjectAffordableHomesAmount? affordableHomesAmount = null,
        OrganisationHomesBuilt? organisationHomesBuilt = null,
        IsSiteIdentified? isSiteIdentified = null,
        Regions? regions = null,
        HomesNumber? homesNumber = null,
        ProjectGeographicFocus? geographicFocus = null,
        IsSupportRequired? isSupportRequired = null,
        IsFundingRequired? isFundingRequired = null,
        RequiredFunding? requiredFunding = null,
        IsProfit? isProfit = null,
        ExpectedStartDate? expectedStartDate = null,
        LocalAuthorityId? localAuthorityId = null)
    {
        Id = id;
        Name = name;
        IsEnglandHousingDelivery = isEnglandHousingDelivery ?? true;
        SupportActivities = supportActivityTypes ?? SupportActivities.Empty();
        Infrastructure = infrastructureTypes ?? ProjectInfrastructure.Empty();
        AffordableHomesAmount = affordableHomesAmount ?? ProjectAffordableHomesAmount.Empty();
        OrganisationHomesBuilt = organisationHomesBuilt;
        IsSiteIdentified = isSiteIdentified;
        Regions = regions ?? Regions.Empty();
        HomesNumber = homesNumber;
        GeographicFocus = geographicFocus ?? ProjectGeographicFocus.Empty();
        IsSupportRequired = isSupportRequired;
        IsFundingRequired = isFundingRequired;
        RequiredFunding = requiredFunding ?? RequiredFunding.Empty;
        IsProfit = isProfit ?? IsProfit.Empty;
        ExpectedStartDate = expectedStartDate ?? ExpectedStartDate.Empty;
        LocalAuthorityId = localAuthorityId;
    }

    public FrontDoorProjectId Id { get; private set; }

    public bool IsEnglandHousingDelivery { get; private set; }

    public ProjectName Name { get; private set; }

    public SupportActivities SupportActivities { get; private set; }

    public ProjectAffordableHomesAmount AffordableHomesAmount { get; private set; }

    public ProjectInfrastructure Infrastructure { get; private set; }

    public OrganisationHomesBuilt? OrganisationHomesBuilt { get; private set; }

    public IsSiteIdentified? IsSiteIdentified { get; private set; }

    public Regions Regions { get; private set; }

    public HomesNumber? HomesNumber { get; private set; }

    public ProjectGeographicFocus GeographicFocus { get; private set; }

    public IsFundingRequired? IsFundingRequired { get; private set; }

    public IsSupportRequired? IsSupportRequired { get; private set; }

    public RequiredFunding RequiredFunding { get; private set; }

    public IsProfit IsProfit { get; private set; }

    public ExpectedStartDate ExpectedStartDate { get; private set; }

    public LocalAuthorityId? LocalAuthorityId { get; private set; }

    public static async Task<ProjectEntity> New(ProjectName projectName, IProjectNameExists projectNameExists, CancellationToken cancellationToken)
    {
        return new(FrontDoorProjectId.New(), await ValidateProjectNameUniqueness(projectName, projectNameExists, cancellationToken));
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

    public void ProvideInfrastructureTypes(ProjectInfrastructure infrastructure)
    {
        Infrastructure = _modificationTracker.Change(Infrastructure, infrastructure);
    }

    public void ProvideGeographicFocus(ProjectGeographicFocus geographicFocus)
    {
        GeographicFocus = _modificationTracker.Change(GeographicFocus, geographicFocus);
    }

    public void New(FrontDoorProjectId projectId)
    {
        SetId(projectId);
        Publish(new FrontDoorProjectHasBeenCreatedEvent(projectId, Name.Value));
    }

    public async Task ProvideName(ProjectName projectName, IProjectNameExists projectNameExists, CancellationToken cancellationToken)
    {
        if (Name == projectName)
        {
            return;
        }

        Name = _modificationTracker.Change(Name, await ValidateProjectNameUniqueness(projectName, projectNameExists, cancellationToken));
    }

    public void ProvideIsEnglandHousingDelivery(bool? isEnglandHousingDelivery)
    {
        IsEnglandHousingDelivery = _modificationTracker.Change(IsEnglandHousingDelivery, ValidateEnglandHousingDelivery(isEnglandHousingDelivery));
    }

    public void ProvideIsSiteIdentified(IsSiteIdentified isSiteIdentified)
    {
        IsSiteIdentified = _modificationTracker.Change(IsSiteIdentified, isSiteIdentified, null, IsSiteIdentifiedHasChanged);
    }

    public void ProvideOrganisationHomesBuilt(OrganisationHomesBuilt organisationHomesBuilt)
    {
        OrganisationHomesBuilt = _modificationTracker.Change(OrganisationHomesBuilt, organisationHomesBuilt);
    }

    public void ProvideRegions(Regions regions)
    {
        Regions = _modificationTracker.Change(Regions, regions);
    }

    public void ProvideHomesNumber(HomesNumber homesNumber)
    {
        HomesNumber = _modificationTracker.Change(HomesNumber, homesNumber);
    }

    public void ProvideIsSupportRequired(IsSupportRequired isSupportRequired)
    {
        IsSupportRequired = _modificationTracker.Change(IsSupportRequired, isSupportRequired);
    }

    public void ProvideIsFundingRequired(IsFundingRequired isFundingRequired)
    {
        IsFundingRequired = _modificationTracker.Change(IsFundingRequired, isFundingRequired, null, IsFundingRequiredHasChanged);
    }

    public void ProvideRequiredFunding(RequiredFunding requiredFunding)
    {
        RequiredFunding = _modificationTracker.Change(RequiredFunding, requiredFunding);
    }

    public void ProvideIsProfit(IsProfit isProfit)
    {
        IsProfit = _modificationTracker.Change(IsProfit, isProfit);
    }

    public void ProvideExpectedStartDate(ExpectedStartDate expectedStartDate)
    {
        ExpectedStartDate = _modificationTracker.Change(ExpectedStartDate, expectedStartDate);
    }

    public void ProvideLocalAuthority(LocalAuthorityId localAuthorityId)
    {
        LocalAuthorityId = _modificationTracker.Change(LocalAuthorityId, localAuthorityId);
    }

    private static async Task<ProjectName> ValidateProjectNameUniqueness(
        ProjectName projectName,
        IProjectNameExists projectNameExists,
        CancellationToken cancellationToken)
    {
        if (await projectNameExists.DoesExist(projectName, cancellationToken))
        {
            OperationResult.ThrowValidationError(nameof(Name), "This name has already been used on another project");
        }

        return projectName;
    }

    private void SetId(FrontDoorProjectId newId)
    {
        if (!Id.IsNew)
        {
            throw new DomainException("Id cannot be modified", CommonErrorCodes.IdCannotBeModified);
        }

        Id = newId;
    }

    private void IsFundingRequiredHasChanged(IsFundingRequired? isFundingRequired)
    {
        if (!isFundingRequired?.Value ?? false)
        {
            RequiredFunding = RequiredFunding.Empty;
            IsProfit = IsProfit.Empty;
        }
    }

    private void IsSiteIdentifiedHasChanged(IsSiteIdentified? isSiteIdentified)
    {
        if (isSiteIdentified?.Value ?? false)
        {
            ResetNonSiteQuestions();
        }
        else
        {
            Publish(new FrontDoorProjectSitesAreNotIdentifiedEvent(Id));
        }
    }

    private void ResetNonSiteQuestions()
    {
        GeographicFocus = ProjectGeographicFocus.Empty();
        LocalAuthorityId = null;
        Regions = Regions.Empty();
        HomesNumber = null;
    }

    private void SupportActivityTypesHaveChanged(SupportActivities newSupportActivityTypes)
    {
        if (!newSupportActivityTypes.IsTenureRequired())
        {
            AffordableHomesAmount = ProjectAffordableHomesAmount.Empty();
            OrganisationHomesBuilt = null;
        }

        if (!newSupportActivityTypes.IsInfrastructureRequired())
        {
            Infrastructure = ProjectInfrastructure.Empty();
        }
    }
}
