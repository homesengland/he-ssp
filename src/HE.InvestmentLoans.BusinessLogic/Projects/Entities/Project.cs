using HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Workflow;
using HE.InvestmentLoans.BusinessLogic.Projects.Enums;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract;
using HE.InvestmentLoans.Contract.Application.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Projects.Entities;

public class Project
{
    public Project()
    {
        Id = new ProjectId(Guid.NewGuid());
        StateChanged = false;

        IsNewlyCreated = true;
    }

    public Project(
        ProjectId id,
        ProjectName? name,
        StartDate? startDate,
        PlanningReferenceNumber? planningReferenceNumber,
        Coordinates? coordinates,
        LandRegistryTitleNumber? landRegistryTitleNumber,
        LandOwnership? landOwnership,
        AdditionalDetails? additionalDetails)
    {
        IsNewlyCreated = false;

        Id = id;
        Name = name;
        StartDate = startDate;
        PlanningReferenceNumber = planningReferenceNumber;
        Coordinates = coordinates;
        LandRegistryTitleNumber = landRegistryTitleNumber;
        LandOwnership = landOwnership;
        AdditionalDetails = additionalDetails;
    }

    public ProjectId Id { get; private set; }

    public ProjectName? Name { get; private set; }

    public StartDate? StartDate { get; private set; }

    public PlanningReferenceNumber? PlanningReferenceNumber { get; private set; }

    public PlanningPermissionStatus? PlanningPermissionStatus { get; private set; }

    public Coordinates? Coordinates { get; private set; }

    public LandRegistryTitleNumber? LandRegistryTitleNumber { get; private set; }

    public LandOwnership? LandOwnership { get; private set; }

    public AdditionalDetails? AdditionalDetails { get; private set; }

    public bool IsNewlyCreated { get; private set; }

    public string? CheckAnswers { get; set; }

    public string? NameLegacy { get; set; }

    public string? AffordableHomes { get; set; }

    public bool? PlanningRef { get; set; }

    public string? PlanningRefEnter { get; set; }

    public string? SitePurchaseFrom { get; set; }

    public string? ManyHomes { get; set; }

    public string? GrantFunding { get; set; }

    public string? TitleNumber { get; set; }

    public string[]? TypeHomes { get; set; }

    public string? TypeHomesOther { get; set; }

    public string? HomesToBuild { get; set; }

    public string? PurchaseDay { get; set; }

    public string? PurchaseMonth { get; set; }

    public string? PurchaseYear { get; set; }

    public DateTime? PurchaseDate { get; set; }

    public string? Cost { get; set; }

    public string? Value { get; set; }

    public string? Source { get; set; }

    public string? PlanningStatus { get; set; }

    public string? GrantFundingName { get; set; }

    public string? GrantFundingSource { get; set; }

    public string? GrantFundingAmount { get; set; }

    public string? GrantFundingPurpose { get; set; }

    public string? Type { get; set; }

    public bool? ChargesDebt { get; set; }

    public string? ChargesDebtInfo { get; set; }

    public bool IsSoftDeleted { get; private set; }

    public bool StateChanged { get; set; }

    public void ChangeName(ProjectName newName)
    {
        Name = newName;
    }

    public void ProvideStartDate(StartDate startDate)
    {
        StartDate = startDate;
    }

    public void MarkAsDeleted()
    {
        IsSoftDeleted = true;
    }

    public void ProvidePlanningReferenceNumber(PlanningReferenceNumber planningReferenceNumber)
    {
        PlanningReferenceNumber = planningReferenceNumber;
    }

    public void ProvideCoordinates(Coordinates coordinates)
    {
        if (coordinates.IsProvided())
        {
            LandRegistryTitleNumber = null!;
        }

        Coordinates = coordinates;
    }

    public void ProvideLandRegistryNumber(LandRegistryTitleNumber landRegistryTitleNumber)
    {
        if (landRegistryTitleNumber.IsProvided())
        {
            Coordinates = null!;
        }

        LandRegistryTitleNumber = landRegistryTitleNumber;
    }

    public void ProvidePlanningPermissionStatus(PlanningPermissionStatus? planningPermissionStatus)
    {
        if (PlanningReferenceNumber.IsNotProvided() || !PlanningReferenceNumber!.Exists)
        {
            throw new DomainException($"Cannot provide planning permission status because project id: {Id}, has no planning reference number.", LoanApplicationErrorCodes.PlanningReferenceNumberNotExists);
        }

        PlanningPermissionStatus = planningPermissionStatus;
    }

    public void ProvideLandOwnership(LandOwnership landOwnership)
    {
        LandOwnership = landOwnership;

        if (!LandOwnership.ApplicantHasFullOwnership)
        {
            AdditionalDetails = null!;
        }
    }

    public void ProvideAdditionalData(AdditionalDetails additionalDetails)
    {
        AdditionalDetails = additionalDetails;
    }
}
