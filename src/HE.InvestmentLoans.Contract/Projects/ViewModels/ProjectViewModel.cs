using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.Contract.Application.ValueObjects;

namespace HE.InvestmentLoans.Contract.Projects.ViewModels;
public class ProjectViewModel
{
    public Guid ApplicationId { get; set; }

    public Guid ProjectId { get; set; }

    public string? Name { get; set; }

    public string? HasEstimatedStartDate { get; set; }

    public string? EstimatedStartDay { get; set; }

    public string? EstimatedStartMonth { get; set; }

    public string? EstimatedStartYear { get; set; }

    public string? HomesCount { get; set; }

    public string[]? HomeTypes { get; set; }

    public string? OtherHomeTypes { get; set; }

    public string? ProjectType { get; set; }

    public string? PlanningReferenceNumberExists { get; set; }

    public string? PlanningReferenceNumber { get; set; }

    public string? PlanningPermissionStatus { get; set; }

    public string? LocationOption { get; set; }

    public string? LocationCoordinates { get; set; }

    public string? LocationLandRegistry { get; set; }

    public string? Ownership { get; set; }

    public string? PurchaseDay { get; set; }

    public string? PurchaseMonth { get; set; }

    public string? PurchaseYear { get; set; }

    public string? Cost { get; set; }

    public string? Value { get; set; }

    public string? Source { get; set; }

    public string? GrantFundingStatus { get; set; }

    public string? GrantFundingProviderName { get; set; }

    public string? GrantFundingName { get; set; }

    public string? GrantFundingAmount { get; set; }

    public string? GrantFundingPurpose { get; set; }

    public string? ChargesDebt { get; set; }

    public string? ChargesDebtInfo { get; set; }

    public string? AffordableHomes { get; set; }

    public string? CheckAnswers { get; set; }

  public string DeleteProject { get; set; }
}
