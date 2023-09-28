using HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Workflow;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
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

    public Project(ProjectId id, ProjectName name, StartDate startDate)
    {
        Id = id;
        Name = name;
        StartDate = startDate;

        IsNewlyCreated = false;
    }

    public ProjectId Id { get; private set; }

    public ProjectName Name { get; private set; }

    public StartDate StartDate { get; private set; }

    public bool IsNewlyCreated { get; private set; }

    public string? CheckAnswers { get; set; }

    public string? NameLegacy { get; set; }

    public string? AffordableHomes { get; set; }

    public bool? PlanningRef { get; set; }

    public string? PlanningRefEnter { get; set; }

    public string? SitePurchaseFrom { get; set; }

    public bool? Ownership { get; set; }

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
}
