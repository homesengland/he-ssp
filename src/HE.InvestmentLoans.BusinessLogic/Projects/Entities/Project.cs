using HE.InvestmentLoans.BusinessLogic.Projects.Enums;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Contract;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.InvestmentLoans.BusinessLogic.Projects.Entities;

public class Project : DomainEntity
{
    public Project()
    {
        Id = new ProjectId(Guid.NewGuid());
        IsNewlyCreated = true;

        Status = SectionStatus.NotStarted;
    }

    public Project(
        ProjectId id,
        SectionStatus status,
        ProjectName? name,
        StartDate? startDate,
        HomesCount? homesCount,
        HomesTypes? homesTypes,
        ProjectType? projectType,
        PlanningReferenceNumber? planningReferenceNumber,
        Coordinates? coordinates,
        LandRegistryTitleNumber? landRegistryTitleNumber,
        LandOwnership? landOwnership,
        AdditionalDetails? additionalDetails,
        PublicSectorGrantFundingStatus? grantFundingStatus,
        PublicSectorGrantFunding? publicSectorGrantFunding,
        ChargesDebt? chargesDebt,
        AffordableHomes? affordableHomes,
        ApplicationStatus loanApplicationStatus,
        PlanningPermissionStatus? planningPermissionStatus)
    {
        IsNewlyCreated = false;

        Id = id;
        Status = status;
        Name = name;
        StartDate = startDate;
        HomesCount = homesCount;
        HomesTypes = homesTypes;
        ProjectType = projectType;
        PlanningReferenceNumber = planningReferenceNumber;
        Coordinates = coordinates;
        LandRegistryTitleNumber = landRegistryTitleNumber;
        LandOwnership = landOwnership;
        AdditionalDetails = additionalDetails;
        GrantFundingStatus = grantFundingStatus;
        PublicSectorGrantFunding = publicSectorGrantFunding;
        ChargesDebt = chargesDebt;
        AffordableHomes = affordableHomes;

        IsNewlyCreated = false;
        LoanApplicationStatus = loanApplicationStatus;
        PlanningPermissionStatus = planningPermissionStatus;
    }

    public ProjectId Id { get; private set; }

    public ApplicationStatus LoanApplicationStatus { get; }

    public ProjectName? Name { get; private set; }

    public StartDate? StartDate { get; private set; }

    public HomesCount? HomesCount { get; private set; }

    public HomesTypes? HomesTypes { get; private set; }

    public ProjectType? ProjectType { get; private set; }

    public PlanningReferenceNumber? PlanningReferenceNumber { get; private set; }

    public PlanningPermissionStatus? PlanningPermissionStatus { get; private set; }

    public Coordinates? Coordinates { get; private set; }

    public LandRegistryTitleNumber? LandRegistryTitleNumber { get; private set; }

    public LandOwnership? LandOwnership { get; private set; }

    public AdditionalDetails? AdditionalDetails { get; private set; }

    public PublicSectorGrantFundingStatus? GrantFundingStatus { get; private set; }

    public PublicSectorGrantFunding? PublicSectorGrantFunding { get; private set; }

    public ChargesDebt? ChargesDebt { get; private set; }

    public AffordableHomes? AffordableHomes { get; private set; }

    public SectionStatus Status { get; private set; }

    public bool IsNewlyCreated { get; }

    public bool IsSoftDeleted { get; private set; }

    public void Delete()
    {
        IsSoftDeleted = true;
    }

    public void ChangeName(ProjectName? newName)
    {
        if (Name != newName)
        {
            UncompleteSection();
        }

        Name = newName;
    }

    public void ProvideStartDate(StartDate? startDate)
    {
        if (StartDate != startDate)
        {
            UncompleteSection();
        }

        StartDate = startDate;
    }

    public void ProvideHomesCount(HomesCount? homesCount)
    {
        if (HomesCount != homesCount)
        {
            UncompleteSection();
        }

        HomesCount = homesCount;
    }

    public void ProvideHomesTypes(HomesTypes? homesTypes)
    {
        if (HomesTypes != homesTypes)
        {
            UncompleteSection();
        }

        HomesTypes = homesTypes;
    }

    public void ProvideProjectType(ProjectType? projectType)
    {
        if (ProjectType != projectType)
        {
            UncompleteSection();
        }

        ProjectType = projectType;
    }

    public void ProvidePlanningReferenceNumber(PlanningReferenceNumber? planningReferenceNumber)
    {
        if (PlanningReferenceNumber == planningReferenceNumber)
        {
            return;
        }

        if (planningReferenceNumber.IsProvided() && PlanningReferenceNumber.IsProvided())
        {
            if (planningReferenceNumber!.Exists && planningReferenceNumber.Value.IsNotProvided() && PlanningReferenceNumber!.Exists)
            {
                PlanningReferenceNumber = new PlanningReferenceNumber(planningReferenceNumber.Exists, PlanningReferenceNumber.Value);
            }
            else
            {
                PlanningReferenceNumber = planningReferenceNumber;
            }
        }
        else
        {
            PlanningReferenceNumber = planningReferenceNumber;
        }

        UncompleteSection();
    }

    public void ProvideCoordinates(Coordinates? coordinates)
    {
        if (coordinates.IsProvided())
        {
            LandRegistryTitleNumber = null!;
        }

        if (Coordinates != coordinates)
        {
            UncompleteSection();
        }

        Coordinates = coordinates;
    }

    public void ProvideLandRegistryNumber(LandRegistryTitleNumber? landRegistryTitleNumber)
    {
        if (landRegistryTitleNumber.IsProvided())
        {
            Coordinates = null!;
        }

        if (LandRegistryTitleNumber != landRegistryTitleNumber)
        {
            UncompleteSection();
        }

        LandRegistryTitleNumber = landRegistryTitleNumber;
    }

    public void ProvidePlanningPermissionStatus(PlanningPermissionStatus? planningPermissionStatus)
    {
        if (PlanningReferenceNumber.IsNotProvided() || !PlanningReferenceNumber!.Exists)
        {
            throw new DomainException(
                $"Cannot provide planning permission status because project id: {Id}, has no planning reference number.",
                LoanApplicationErrorCodes.PlanningReferenceNumberNotExists);
        }

        if (PlanningPermissionStatus != planningPermissionStatus)
        {
            UncompleteSection();
        }

        PlanningPermissionStatus = planningPermissionStatus;
    }

    public void ProvideLandOwnership(LandOwnership? landOwnership)
    {
        if (LandOwnership != landOwnership)
        {
            UncompleteSection();
        }

        LandOwnership = landOwnership;

        if (LandOwnership is null || !LandOwnership.ApplicantHasFullOwnership)
        {
            AdditionalDetails = null;
        }

        UncompleteSection();
    }

    public void ProvideAdditionalData(AdditionalDetails? additionalDetails)
    {
        if (additionalDetails != AdditionalDetails)
        {
            UncompleteSection();
        }

        AdditionalDetails = additionalDetails;
    }

    public void ProvideChargesDebt(ChargesDebt? chargesDebt)
    {
        if (ChargesDebt != chargesDebt)
        {
            UncompleteSection();
        }

        ChargesDebt = chargesDebt;
    }

    public void ProvideAffordableHomes(AffordableHomes? affordableHomes)
    {
        if (AffordableHomes != affordableHomes)
        {
            UncompleteSection();
        }

        AffordableHomes = affordableHomes;
    }

    internal void ProvideGrantFundingStatus(PublicSectorGrantFundingStatus? grantFundingStatus)
    {
        if (GrantFundingStatus != grantFundingStatus)
        {
            UncompleteSection();
        }

        GrantFundingStatus = grantFundingStatus;

        if (grantFundingStatus != PublicSectorGrantFundingStatus.Received)
        {
            PublicSectorGrantFunding = null;
        }
    }

    internal void ProvideGrantFundingInformation(PublicSectorGrantFunding? publicSectorGrantFunding)
    {
        if (GrantFundingStatus != PublicSectorGrantFundingStatus.Received)
        {
            throw new DomainException(
                $"Cannot provide more information about grant funding that has not been received. Current status: {GrantFundingStatus}",
                LoanApplicationErrorCodes.GrantFundingNotExists);
        }

        if (PublicSectorGrantFunding != publicSectorGrantFunding)
        {
            UncompleteSection();
        }

        PublicSectorGrantFunding = publicSectorGrantFunding;
    }

    internal void CheckAnswers(YesNoAnswers answer)
    {
        switch (answer)
        {
            case YesNoAnswers.Yes:
                if (!CanBeCompleted())
                {
                    OperationResult.New().AddValidationError(nameof(CheckAnswers), ValidationErrorMessage.CheckAnswersOption).CheckErrors();
                }

                CompleteSection();
                break;
            case YesNoAnswers.No:
                UncompleteSection();
                break;
            case YesNoAnswers.Undefined:
                OperationResult.New()
                    .AddValidationError(nameof(CheckAnswers), ValidationErrorMessage.NoCheckAnswers)
                    .CheckErrors();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(answer), answer, null);
        }
    }

    private bool CanBeCompleted()
    {
        return StartDate.IsProvided() &&
            PlanningReferenceNumber.IsProvided() && (!PlanningReferenceNumber!.Exists || (PlanningReferenceNumber.Value.IsProvided() && PlanningPermissionStatus.IsProvided())) &&
            HomesCount.IsProvided() &&
            HomesTypes.IsProvided() && HomesTypes!.HomesTypesValue.Any() &&
            ProjectType.IsProvided() &&
            (Coordinates.IsProvided() || LandRegistryTitleNumber.IsProvided()) &&
            LandOwnership.IsProvided() && (!LandOwnership!.ApplicantHasFullOwnership || AdditionalDetails.IsProvided()) &&
            GrantFundingStatus.IsProvided() && (GrantFundingStatus != PublicSectorGrantFundingStatus.Received || PublicSectorGrantFunding.IsProvided()) &&
            ChargesDebt.IsProvided() &&
            AffordableHomes.IsProvided();
    }

    private void CompleteSection()
    {
        Status = SectionStatus.Completed;
    }

    private void UncompleteSection()
    {
        Status = SectionStatus.InProgress;
    }
}
