using HE.InvestmentLoans.BusinessLogic.LoanApplication.Workflow;
using HE.InvestmentLoans.Common.Utils;

namespace HE.InvestmentLoans.BusinessLogic.ViewModel;

public class SiteViewModel
{
    public SiteViewModel()
    {
        State = SiteWorkflow.State.Index;
        StateChanged = false;
    }

    public string? CheckAnswers { get; set; }

    public string? AffordableHomes { get; set; }

    public string? PlanningRef { get; set; }

    public string? PlanningRefEnter { get; set; }

    public string? SitePurchaseFrom { get; set; }

    public string? Ownership { get; set; }

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

    public string? LocationOption { get; set; }

    public string? LocationCoordinates { get; set; }

    public string? LocationLandRegistry { get; set; }

    public string? Location { get; set; }

    public string? PlanningStatus { get; set; }

    public string? GrantFundingName { get; set; }

    public string? GrantFundingSource { get; set; }

    public string? GrantFundingAmount { get; set; }

    public string? GrantFundingPurpose { get; set; }

    public string? Type { get; set; }

    public string? ChargesDebt { get; set; }

    public string? ChargesDebtInfo { get; set; }

    public SiteWorkflow.State State { get; set; }

    public SiteWorkflow.State PreviousState { get; set; }

    public string? Name { get; set; }

    public Guid? Id { get; set; }

    public bool StateChanged { get; set; }

    public string? HasEstimatedStartDate { get; set; }

    public string? EstimatedStartDay { get; set; }

    public string? EstimatedStartMonth { get; set; }

    public string? EstimatedStartYear { get; set; }

    public string? DeleteProject { get; set; }

    public void RemoveAlternativeRoutesData()
    {
        if (TypeHomes != null && !TypeHomes.Contains("other"))
        {
            TypeHomesOther = null;
        }

        if (PlanningRef == CommonResponse.No)
        {
            PlanningRefEnter = null;
            Location = null;
            PlanningStatus = null;
        }

        if (LocationOption == "coordinates")
        {
            LocationLandRegistry = null;
        }
        else if (LocationOption == "landRegistryTitleNumber")
        {
            LocationCoordinates = null;
        }

        if (Ownership == CommonResponse.No)
        {
            PurchaseDate = null;
            Cost = null;
            Value = null;
            Source = null;
        }

        if (GrantFunding != CommonResponse.Yes)
        {
            GrantFundingSource = null;
            GrantFundingAmount = null;
            GrantFundingName = null;
            GrantFundingPurpose = null;
        }

        if (ChargesDebt == CommonResponse.No)
        {
            ChargesDebtInfo = null;
        }
    }

    public bool AllInformationIsProvided()
    {
        return BasicInformationProvided() &&
            PlanningReferenceProvided() &&
            OwnershipInformationProvided() &&
            GrantFundingInformationProvided();
    }

    private bool BasicInformationProvided()
    {
        return !string.IsNullOrEmpty(Name) &&
                !string.IsNullOrEmpty(ManyHomes) &&
                !string.IsNullOrEmpty(HasEstimatedStartDate) &&
                TypeHomes != null && TypeHomes.Length > 0 &&
                !string.IsNullOrEmpty(Type) &&
                !string.IsNullOrEmpty(ChargesDebt) &&
                !string.IsNullOrEmpty(AffordableHomes);
    }

    private bool PlanningReferenceProvided()
    {
        if (string.IsNullOrEmpty(PlanningRef))
        {
            return false;
        }

        if (PlanningRef == CommonResponse.Yes && !PlanningInformationProvided())
        {
            return false;
        }

        return ProjectLocationProvided();
    }

    private bool PlanningInformationProvided()
    {
        return !string.IsNullOrEmpty(PlanningRefEnter) && !string.IsNullOrEmpty(PlanningStatus);
    }

    private bool ProjectLocationProvided()
    {
        if (string.IsNullOrEmpty(LocationOption))
        {
            return false;
        }

        if (LocationOption == "coordinates")
        {
            return !string.IsNullOrEmpty(LocationCoordinates);
        }
        else
        {
            return !string.IsNullOrEmpty(LocationLandRegistry);
        }
    }

    private bool OwnershipInformationProvided()
    {
        if (string.IsNullOrEmpty(Ownership))
        {
            return false;
        }

        if (Ownership == CommonResponse.No)
        {
            return true;
        }

        return !string.IsNullOrEmpty(PurchaseDay)
                && !string.IsNullOrEmpty(PurchaseMonth)
                && !string.IsNullOrEmpty(PurchaseYear)
                && !string.IsNullOrEmpty(Cost)
                && !string.IsNullOrEmpty(Value)
                && !string.IsNullOrEmpty(Source);
    }

    private bool GrantFundingInformationProvided()
    {
        if (string.IsNullOrEmpty(GrantFunding))
        {
            return false;
        }

        if (GrantFunding is CommonResponse.No or CommonResponse.DoNotKnow)
        {
            return true;
        }

        return !string.IsNullOrEmpty(GrantFundingName) &&
            !string.IsNullOrEmpty(GrantFundingSource) &&
            !string.IsNullOrEmpty(GrantFundingAmount) &&
            !string.IsNullOrEmpty(GrantFundingPurpose);
    }
}
