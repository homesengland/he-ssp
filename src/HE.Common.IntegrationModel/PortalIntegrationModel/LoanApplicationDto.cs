#pragma warning disable IDE0005 // Using directive is unnecessary.
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
#pragma warning restore IDE0005 // Using directive is unnecessary.

namespace HE.Common.IntegrationModel.PortalIntegrationModel
{
    public class LoanApplicationDto
    {
        //COMPANY
        public bool? companyPurpose { get; set; } //Purpose

        public string existingCompany { get; set; } //ExistingCompany

        public int? companyExperience { get; set; } //HomesBuilt
                                                    //Company.CompanyInfoFile

        //FUNDING
        public string projectGdv { get; set; } //GDV

        public string projectEstimatedTotalCost { get; set; } //TotalCosts

        public bool? projectAbnormalCosts { get; set; } //AbnormalCosts

        public string projectAbnormalCostsInformation { get; set; } //AbnormalCosts

        public bool? privateSectorApproach { get; set; } //PrivateSectorFunding

        public string privateSectorApproachInformation { get; set; } //PrivateSectorFunding

        public bool? additionalProjects { get; set; } //AdditionalProjects

        public string refinanceRepayment { get; set; } //Refinance

        public string refinanceRepaymentDetails { get; set; } //Refinance
                                                              //Complete

        //SECURITY
        public bool? outstandingLegalChargesOrDebt { get; set; } //ChargesDebtCompany

        public string debentureHolder { get; set; } //ChargesDebtCompany

        public bool? directorLoans { get; set; } //DirLoans

        public bool? confirmationDirectorLoansCanBeSubordinated { get; set; } //DirLoansSub

        public string reasonForDirectorLoanNotSubordinated { get; set; } //DirLoansSub

        //SITES
        public List<SiteDetailsDto> siteDetailsList { get; set; }

        //SECTIONS STATUSES

        public int? CompanyStructureAndExperienceCompletionStatus { get; set; }

        public int? FundingDetailsCompletionStatus { get; set; }

        public int? SecurityDetailsCompletionStatus { get; set; }

        public int? SiteDetailsCompletionStatus { get; set; }

        //OTHER ATTRIBUTES
        public UserAccountDto LoanApplicationContact { get; set; }

        public DateTime? LastModificationOn { get; set; }

        public int? loanApplicationExternalStatus { get; set; } //LOAN APPLICATION EXTERNAL STATUS IN INT VALUE


        //TO DELETE IN SOME TIME???
        public string loanApplicationId { get; set; }

        public string name { get; set; }

        public string numberOfSites { get; set; }

        public string companyStructureInformation { get; set; }

        public string costsForAdditionalProjects { get; set; }

        public string fundingReason { get; set; }

        public string fundingTypeForAdditionalProjects { get; set; }

        public string contactEmailAdress { get; set; }

        public Guid accountId { get; set; }

        public string loanApplicationStatus { get; set; }

        public string externalId { get; set; }
    }
}
