using DataverseModel;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;

namespace HE.CRM.Common.DtoMapping
{
    public class LoanApplicationDtoMapper
    {
        public static invln_Loanapplication MapLoanApplicationDtoToRegularEntity(LoanApplicationDto loanApplicationDto, Contact contact, string accountId)
        {
            var loanApplication = new invln_Loanapplication()
            {
                invln_NumberofSites = ParseInt(loanApplicationDto.numberOfSites),
                invln_FundingReason = MapFundingReason(loanApplicationDto.fundingReason),
                invln_ExternalStatus = MapApplicationExternalStatus(loanApplicationDto.loanApplicationStatus),

                //COMPANY
                invln_CompanyPurpose = ParseBool(loanApplicationDto.companyPurpose), //Purpose
                invln_Companystructureinformation = loanApplicationDto.existingCompany, //ExistingCompany
                invln_CompanyExperience = loanApplicationDto.companyExperience, //HomesBuilt
                                                                                //Company.CompanyInfoFile

                //FUNDING
                invln_ProjectGDV = ParseDecimalToMoney(loanApplicationDto.projectGdv), //GDV
                invln_Projectestimatedtotalcost = ParseDecimalToMoney(loanApplicationDto.projectEstimatedTotalCost), //TotalCosts
                invln_Projectabnormalcosts = ParseBool(loanApplicationDto.projectAbnormalCosts), //AbnormalCosts
                invln_Projectabnormalcostsinformation = loanApplicationDto.projectAbnormalCostsInformation, //AbnormalCosts
                invln_Privatesectorapproach = ParseBool(loanApplicationDto.privateSectorApproach), //PrivateSectorFunding
                invln_Privatesectorapproachinformation = loanApplicationDto.privateSectorApproachInformation, //PrivateSectorFunding
                invln_Additionalprojects = ParseBool(loanApplicationDto.additionalProjects), //AdditionalProjects
                invln_Refinancerepayment = MapRefinancePayment(loanApplicationDto.refinanceRepayment), //Refinance
                invln_Refinancerepaymentdetails = loanApplicationDto.refinanceRepaymentDetails, //Refinance


                //SECURITY
                invln_Outstandinglegalchargesordebt = ParseBool(loanApplicationDto.outstandingLegalChargesOrDebt), //ChargesDebtCompany
                invln_DebentureHolder = loanApplicationDto.debentureHolder, //ChargesDebtCompany
                invln_Directorloans = ParseBool(loanApplicationDto.directorLoans), //DirLoans
                invln_Confirmationdirectorloanscanbesubordinated = ParseBool(loanApplicationDto.confirmationDirectorLoansCanBeSubordinated), //DirLoansSub
                invln_Reasonfordirectorloannotsubordinated = loanApplicationDto.reasonForDirectorLoanNotSubordinated, //DirLoansSub

                //OTHER maybe not related
                invln_Name = loanApplicationDto.name,
                invln_Account = Guid.TryParse(accountId, out Guid accountid) == true ? new EntityReference("account", accountid) : null, //pusty account?
                invln_Contact = contact != null ? contact.ToEntityReference() : null,
            };
            if (Guid.TryParse(loanApplicationDto.loanApplicationId, out Guid loanApplicationId))
            {
                loanApplication.Id = loanApplicationId;
            }
            return loanApplication;
        }


        public static LoanApplicationDto MapLoanApplicationToDto(invln_Loanapplication loanApplication, List<SiteDetailsDto> siteDetailsDtoList, string externalContactId)
        {
            var loanApplicationDto = new LoanApplicationDto()
            {
                fundingReason = MapFundingReasonOptionSetToString(loanApplication.invln_FundingReason),
                numberOfSites = loanApplication.invln_NumberofSites?.ToString(),
                loanApplicationStatus = MapApplicationStatusFromDtoToRegular(loanApplication.invln_ExternalStatus),
                //company
                companyPurpose = loanApplication.invln_CompanyPurpose?.ToString(),
                existingCompany = loanApplication.invln_Companystructureinformation?.ToString(),
                companyExperience = loanApplication.invln_CompanyExperience,

                //funding
                projectGdv = ((int)loanApplication.invln_ProjectGDV?.Value).ToString(),
                projectEstimatedTotalCost = ((int)loanApplication.invln_Projectestimatedtotalcost?.Value).ToString(),
                projectAbnormalCosts = loanApplication.invln_Projectabnormalcosts?.ToString(),
                projectAbnormalCostsInformation = loanApplication.invln_Projectabnormalcostsinformation?.ToString(),
                privateSectorApproach = loanApplication.invln_Privatesectorapproach?.ToString(),
                privateSectorApproachInformation = loanApplication.invln_Privatesectorapproachinformation?.ToString(),
                additionalProjects = loanApplication.invln_Additionalprojects?.ToString(),
                refinanceRepayment = MapRefinancePaymentOptionSetToString(loanApplication.invln_Refinancerepayment),
                refinanceRepaymentDetails = loanApplication.invln_Refinancerepaymentdetails?.ToString(),

                //security
                outstandingLegalChargesOrDebt = loanApplication.invln_Outstandinglegalchargesordebt?.ToString(),
                debentureHolder = loanApplication.invln_DebentureHolder?.ToString(),
                directorLoans = loanApplication.invln_Directorloans?.ToString(),
                confirmationDirectorLoansCanBeSubordinated = loanApplication.invln_Confirmationdirectorloanscanbesubordinated?.ToString(),
                reasonForDirectorLoanNotSubordinated = loanApplication.invln_Reasonfordirectorloannotsubordinated?.ToString(),

                name = loanApplication.invln_Name,
                accountId = loanApplication.invln_Account.Id,
                loanApplicationId = loanApplication.invln_LoanapplicationId.ToString(),
                siteDetailsList = siteDetailsDtoList,
                externalId = externalContactId,
                LastModificationOn = loanApplication.ModifiedOn,
            };
            return loanApplicationDto;
        }

        public static string MapFundingReasonOptionSetToString(OptionSetValue fundingReason)
        {
            if (fundingReason == null)
            {
                return null;
            }
            switch (fundingReason.Value)
            {
                case (int)invln_FundingReason.Buildinginfrastructureonly:
                    return "buildinginfrastructureonly";
                case (int)invln_FundingReason.Buildingnewhomes:
                    return "buildingnewhomes";
                case (int)invln_FundingReason.Other:
                    return "other";
                default:
                    return null;
            }
        }

        public static OptionSetValue MapApplicationExternalStatus(string applicationStatus)
        {
            switch (applicationStatus?.ToLower())
            {
                case "draft":
                    return new OptionSetValue((int)invln_ExternalStatus.Draft);
                case "submitted":
                    return new OptionSetValue((int)invln_ExternalStatus.Submitted);
                case "under review":
                    return new OptionSetValue((int)invln_ExternalStatus.Underreview);
                case "in due diligence":
                    return new OptionSetValue((int)invln_ExternalStatus.Induediligence);
                case "contract signed subject to cp":
                    return new OptionSetValue((int)invln_ExternalStatus.ContractSignedsubjecttoCP);
                case "cps satisfied":
                    return new OptionSetValue((int)invln_ExternalStatus.CPssatisfied);
                case "loan available":
                    return new OptionSetValue((int)invln_ExternalStatus.Loanavailable);
                case "hold requested":
                    return new OptionSetValue((int)invln_ExternalStatus.Holdrequested);
                case "on hold":
                    return new OptionSetValue((int)invln_ExternalStatus.Onhold);
                case "referred back to applicant":
                    return new OptionSetValue((int)invln_ExternalStatus.Referredbacktoapplicant);
                case "n/a":
                    return new OptionSetValue((int)invln_ExternalStatus.NA);
                case "withdrawn":
                    return new OptionSetValue((int)invln_ExternalStatus.Withdrawn);
                case "not approved":
                    return new OptionSetValue((int)invln_ExternalStatus.Notapproved);
                case "application declined":
                    return new OptionSetValue((int)invln_ExternalStatus.Applicationdeclined);
                case "approved subject to contract":
                    return new OptionSetValue((int)invln_ExternalStatus.Approvedsubjecttocontract);
            }

            return null;
        }

        public static string MapApplicationStatusFromDtoToRegular(OptionSetValue applicationStatus)
        {
            if (applicationStatus == null)
            {
                return null;
            }
            switch (applicationStatus.Value)
            {
                case (int)invln_ExternalStatus.Draft:
                    return "draft";
                case (int)invln_ExternalStatus.Submitted:
                    return "submitted";
                case (int)invln_ExternalStatus.Underreview:
                    return "under review";
                case (int)invln_ExternalStatus.Induediligence:
                    return "in due diligence";
                case (int)invln_ExternalStatus.ContractSignedsubjecttoCP:
                    return "contract signed subject to cp";
                case (int)invln_ExternalStatus.CPssatisfied:
                    return "cps satisfied";
                case (int)invln_ExternalStatus.Loanavailable:
                    return "loan available";
                case (int)invln_ExternalStatus.Holdrequested:
                    return "hold requested";
                case (int)invln_ExternalStatus.Onhold:
                    return "on hold";
                case (int)invln_ExternalStatus.Referredbacktoapplicant:
                    return "referred back to applicant";
                case (int)invln_ExternalStatus.NA:
                    return "n/a";
                case (int)invln_ExternalStatus.Withdrawn:
                    return "withdrawn";
                case (int)invln_ExternalStatus.Notapproved:
                    return "not approved";
                case (int)invln_ExternalStatus.Applicationdeclined:
                    return "application declined";
                case (int)invln_ExternalStatus.Approvedsubjecttocontract:
                    return "approved subject to contract";
            }
            return null;
        }

        public static OptionSetValue MapFundingReason(string fundingReason)
        {
            switch (fundingReason?.ToLower())
            {
                case "buildinginfrastructureonly":
                    return new OptionSetValue((int)invln_FundingReason.Buildinginfrastructureonly);
                case "buildingnewhomes":
                    return new OptionSetValue((int)invln_FundingReason.Buildingnewhomes);
                case "other":
                    return new OptionSetValue((int)invln_FundingReason.Other);
            }

            return null;
        }

        public static OptionSetValue MapRefinancePayment(string refinancePayment)
        {
            switch (refinancePayment?.ToLower())
            {
                case "refinance":
                    return new OptionSetValue((int)invln_refinancerepayment.Refinance);
                case "repay":
                    return new OptionSetValue((int)invln_refinancerepayment.Repay);
            }

            return null;
        }

        public static string MapRefinancePaymentOptionSetToString(OptionSetValue refinancePayment)
        {
            if (refinancePayment == null)
            {
                return null;
            }
            switch (refinancePayment.Value)
            {
                case (int)invln_refinancerepayment.Refinance:
                    return "refinance";
                case (int)invln_refinancerepayment.Repay:
                    return "repay";
            }

            return null;
        }
        private static bool? ParseBool(string boolToParse)
        {
            switch (boolToParse?.ToLower())
            {
                case "yes":
                    return true;
                case "no":
                    return false;
            }

            if (bool.TryParse(boolToParse, out bool boolValue))
            {
                return boolValue;
            }
            else
            {
                return null;
            }
        }

        private static int? ParseInt(string intToParse)
        {
            if (int.TryParse(intToParse, out int intValue))
            {
                return intValue;
            }
            else
            {
                return null;
            }
        }

        private static Money ParseDecimalToMoney(string decimalToParse)
        {
            if (decimal.TryParse(decimalToParse, out decimal decimalValue))
            {
                return new Money(decimalValue);
            }
            else
            {
                return null;
            }
        }

    }
}
