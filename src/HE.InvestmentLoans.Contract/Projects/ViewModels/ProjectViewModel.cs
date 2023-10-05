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

    public string? ChargesDebt { get; set; }

    public string? ChargesDebtInfo { get; set; }

    public string? AffordableHomes { get; set; }
}
