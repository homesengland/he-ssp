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

    public string? PlanningReferenceNumberExists { get; set; }

    public string? PlanningReferenceNumber { get; set; }

    public string? LocationOption { get; set; }

    public string? LocationCoordinates { get; set; }

    public string? LocationLandRegistry { get; set; }

    public string? CheckAnswers { get; set; }
}
