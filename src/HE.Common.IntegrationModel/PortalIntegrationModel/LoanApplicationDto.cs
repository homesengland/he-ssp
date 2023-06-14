using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;

namespace HE.Common.IntegrationModel.PortalIntegrationModel
{
    public class LoanApplicationDto
    {
        //COMPANY
        public string companyPurpose { get; set; } //Purpose
        public string existingCompany { get; set; } //ExistingCompany
        public int? companyExperience { get; set; } //HomesBuilt
                                                    //Company.CompanyInfoFile

        //FUNDING
        public string projectGdv { get; set; } //GDV
        public string projectEstimatedTotalCost { get; set; } //TotalCosts
        public string projectAbnormalCosts { get; set; } //AbnormalCosts
        public string projectAbnormalCostsInformation { get; set; } //AbnormalCosts
        public string privateSectorApproach { get; set; } //PrivateSectorFunding
        public string privateSectorApproachInformation { get; set; } //PrivateSectorFunding
        public string additionalProjects { get; set; } //AdditionalProjects
        public string refinanceRepayment { get; set; } //Refinance
        public string refinanceRepaymentDetails { get; set; } //Refinance
                                                              //Complete

        //SECURITY
        public string outstandingLegalChargesOrDebt { get; set; } //ChargesDebtCompany
        public string debentureHolder { get; set; } //ChargesDebtCompany
        public string directorLoans { get; set; } //DirLoans
        public string confirmationDirectorLoansCanBeSubordinated { get; set; } //DirLoansSub
        public string reasonForDirectorLoanNotSubordinated { get; set; } //DirLoansSub

        //SITES
        public List<SiteDetailsDto> siteDetailsList { get; set; }

        //OTHER ATTRIBUTES
        public string id { get; set; }
        public string name { get; set; }
        public string numberOfSites { get; set; }
        public string companyStructureInformation { get; set; }
        public string costsForAdditionalProjects { get; set; }
        public string fundingReason { get; set; }
        public string fundingTypeForAdditionalProjects { get; set; }
        public string contactEmailAdress { get; set; }
        public Guid accountId { get; set; }
    }
}