using HE.Xrm.ServiceClientExample.Model;
using HE.Xrm.ServiceClientExample.Model.EntitiesDto;
using Microsoft.Xrm.Sdk;

public class LoanApplicationDto
{
    public Guid id { get; set; }
    public string name { get; set; }

    public int? numberOfSites { get; set; }

    public int? companyExperience { get; set; }
    public bool? companyPurpose { get; set; }
    public string companyStructureInformation { get; set; }

    public bool confirmationDirectorLoansCanBeSubordinated { get; set; }

    public Money costsForAdditionalProjects { get; set; }

    public string debentureHolder { get; set; }
    public bool? directorLoads { get; set; }
    public bool? existingCompany { get; set; }

    public OptionSetValue fundingReason { get; set; }

    public OptionSetValue fundingTypeForAdditionalProjects { get; set; }

    public bool? outstandingLegalChargesOrDebt { get; set; }

    public bool? privateSectorApproach { get; set; }

    public string privateSectorApproachInformation { get; set; }
    public bool? projectAbnormalCosts { get; set; }

    public string projectAbnormalCostsInformation { get; set; }

    public Money projectEstimatedTotalCost { get; set; }

    public Money projectGdv { get; set; }

    public string reasonForDirectorLoanNotSubordinated { get; set; }

    public OptionSetValue refinanceRepayment { get; set; }

    public string refinanceRepaymentDetails { get; set; }
    public List<SiteDetailsDto> siteDetailsList { get; set; }
    public string contactEmailAdress { get; set; }
    public Guid accountId { get; set; }
}