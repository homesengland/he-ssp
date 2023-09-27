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

                //COMPANY
                invln_CompanyPurpose = loanApplicationDto.companyPurpose, //Purpose
                invln_Companystructureinformation = loanApplicationDto.existingCompany, //ExistingCompany
                invln_CompanyExperience = loanApplicationDto.companyExperience, //HomesBuilt
                                                                                //Company.CompanyInfoFile

                //FUNDING
                invln_Projectabnormalcosts = loanApplicationDto.projectAbnormalCosts, //AbnormalCosts
                invln_Projectabnormalcostsinformation = loanApplicationDto.projectAbnormalCostsInformation, //AbnormalCosts
                invln_Privatesectorapproach = loanApplicationDto.privateSectorApproach, //PrivateSectorFunding
                invln_Privatesectorapproachinformation = loanApplicationDto.privateSectorApproachInformation, //PrivateSectorFunding
                invln_Additionalprojects = loanApplicationDto.additionalProjects, //AdditionalProjects
                invln_Refinancerepayment = MapRefinancePayment(loanApplicationDto.refinanceRepayment), //Refinance
                invln_Refinancerepaymentdetails = loanApplicationDto.refinanceRepaymentDetails, //Refinance


                //SECURITY
                invln_Outstandinglegalchargesordebt = loanApplicationDto.outstandingLegalChargesOrDebt, //ChargesDebtCompany
                invln_DebentureHolder = loanApplicationDto.debentureHolder, //ChargesDebtCompany
                invln_Directorloans = loanApplicationDto.directorLoans, //DirLoans
                invln_Confirmationdirectorloanscanbesubordinated = loanApplicationDto.confirmationDirectorLoansCanBeSubordinated, //DirLoansSub
                invln_Reasonfordirectorloannotsubordinated = loanApplicationDto.reasonForDirectorLoanNotSubordinated, //DirLoansSub

                //SECTIONS STATUSES
                invln_companystructureandexperiencecompletionst = MapNullableIntToOptionSetValue(loanApplicationDto.CompanyStructureAndExperienceCompletionStatus),
                invln_fundingdetailscompletionstatus = MapNullableIntToOptionSetValue(loanApplicationDto.FundingDetailsCompletionStatus),
                invln_securitydetailscompletionstatus = MapNullableIntToOptionSetValue(loanApplicationDto.SecurityDetailsCompletionStatus),
                invln_sitedetailscompletionstatus = MapNullableIntToOptionSetValue(loanApplicationDto.SiteDetailsCompletionStatus),

                //OTHER ATTRIBUTES
                //CHANGE IN STATUS ONLY VIA STATUS CHANGE ENDPOINT

                //OTHER maybe not related
                invln_source = loanApplicationDto.source,
                invln_Account = Guid.TryParse(accountId, out Guid accountid) == true ? new EntityReference(Account.EntityLogicalName, accountid) : null, //pusty account?
                invln_ProjectGDV = loanApplicationDto.projectGdv.HasValue ? new Money(loanApplicationDto.projectGdv.Value) : null,
                invln_Projectestimatedtotalcost = loanApplicationDto.projectEstimatedTotalCost.HasValue ? new Money(loanApplicationDto.projectEstimatedTotalCost.Value) : null,
                invln_planningpermissionstatus = loanApplicationDto.planningPermissionStatus,
            };

            if (loanApplicationDto.loanApplicationExternalStatus.HasValue)
            {
                loanApplication.invln_ExternalStatus = new OptionSetValue(loanApplicationDto.loanApplicationExternalStatus.Value);
            }
            if (contact != null)
            {
                loanApplication.invln_Contact = contact.ToEntityReference();
            }

            if (Guid.TryParse(loanApplicationDto.loanApplicationId, out Guid loanApplicationId))
            {
                loanApplication.Id = loanApplicationId;
            }
            return loanApplication;
        }


        public static LoanApplicationDto MapLoanApplicationToDto(invln_Loanapplication loanApplication, List<SiteDetailsDto> siteDetailsDtoList, string externalContactId, Contact contact = null)
        {
            var loanApplicationDto = new LoanApplicationDto()
            {
                fundingReason = MapFundingReasonOptionSetToString(loanApplication.invln_FundingReason),
                numberOfSites = loanApplication.invln_NumberofSites?.ToString(),

                //company
                companyPurpose = loanApplication.invln_CompanyPurpose,
                existingCompany = loanApplication.invln_Companystructureinformation?.ToString(),
                companyExperience = loanApplication.invln_CompanyExperience,

                //funding
                projectAbnormalCosts = loanApplication.invln_Projectabnormalcosts,
                projectAbnormalCostsInformation = loanApplication.invln_Projectabnormalcostsinformation?.ToString(),
                privateSectorApproach = loanApplication.invln_Privatesectorapproach,
                privateSectorApproachInformation = loanApplication.invln_Privatesectorapproachinformation?.ToString(),
                additionalProjects = loanApplication.invln_Additionalprojects,
                refinanceRepayment = MapRefinancePaymentOptionSetToString(loanApplication.invln_Refinancerepayment),
                refinanceRepaymentDetails = loanApplication.invln_Refinancerepaymentdetails?.ToString(),

                //security
                outstandingLegalChargesOrDebt = loanApplication.invln_Outstandinglegalchargesordebt,
                debentureHolder = loanApplication.invln_DebentureHolder?.ToString(),
                directorLoans = loanApplication.invln_Directorloans,
                confirmationDirectorLoansCanBeSubordinated = loanApplication.invln_Confirmationdirectorloanscanbesubordinated,
                reasonForDirectorLoanNotSubordinated = loanApplication.invln_Reasonfordirectorloannotsubordinated?.ToString(),

                //SITE DETAILS
                siteDetailsList = siteDetailsDtoList,

                //SECTIONS COMPLETION STATUS
                CompanyStructureAndExperienceCompletionStatus = loanApplication.invln_companystructureandexperiencecompletionst?.Value,
                FundingDetailsCompletionStatus = loanApplication.invln_fundingdetailscompletionstatus?.Value,
                SecurityDetailsCompletionStatus = loanApplication.invln_securitydetailscompletionstatus?.Value,
                SiteDetailsCompletionStatus = loanApplication.invln_sitedetailscompletionstatus?.Value,

                //OTHRER ATTRIBUTES
                LastModificationOn = loanApplication.ModifiedOn,
                loanApplicationExternalStatus = loanApplication.invln_ExternalStatus?.Value,

                name = loanApplication.invln_Name,
                accountId = loanApplication.invln_Account.Id,
                loanApplicationId = loanApplication.invln_LoanapplicationId.ToString(),
                externalId = externalContactId,
                source = loanApplication.invln_source,
                planningPermissionStatus = loanApplication.invln_planningpermissionstatus
            };

            if (loanApplication.invln_ProjectGDV != null)
            {
                loanApplicationDto.projectGdv = loanApplication.invln_ProjectGDV.Value;
            }

            if (loanApplication.invln_Projectestimatedtotalcost != null)
            {
                loanApplicationDto.projectEstimatedTotalCost = loanApplication.invln_Projectestimatedtotalcost.Value;
            }
            if (contact != null)
            {
                loanApplicationDto.LoanApplicationContact = new UserAccountDto()
                {
                    ContactEmail = contact.EMailAddress1,
                    ContactFirstName = contact.FirstName,
                    ContactLastName = contact.LastName,
                    ContactExternalId = contact.invln_externalid,
                    ContactTelephoneNumber = contact.Telephone1,
                    AccountId = loanApplication.invln_Account.Id
                };
            }

            return loanApplicationDto;
        }

        public static OptionSetValue MapNullableIntToOptionSetValue(int? valueToMap)
        {
            if(valueToMap.HasValue)
            {
                return new OptionSetValue(valueToMap.Value);
            }
            return null;
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
                    return new OptionSetValue((int)invln_ExternalStatus.ApplicationSubmitted);
                case "under review":
                    return new OptionSetValue((int)invln_ExternalStatus.UnderReview);
                case "in due diligence":
                    return new OptionSetValue((int)invln_ExternalStatus.Induediligence);
                case "contract signed subject to cp":
                    return new OptionSetValue((int)invln_ExternalStatus.ContractSignedSubjecttoCP);
                case "cps satisfied":
                    return new OptionSetValue((int)invln_ExternalStatus.CPsSatisfied);
                case "loan available":
                    return new OptionSetValue((int)invln_ExternalStatus.LoanAvailable);
                case "hold requested":
                    return new OptionSetValue((int)invln_ExternalStatus.HoldRequested);
                case "on hold":
                    return new OptionSetValue((int)invln_ExternalStatus.OnHold);
                case "referred back to applicant":
                    return new OptionSetValue((int)invln_ExternalStatus.ReferredBacktoApplicant);
                case "inactive":
                    return new OptionSetValue((int)invln_ExternalStatus.NA);
                case "withdrawn":
                    return new OptionSetValue((int)invln_ExternalStatus.Withdrawn);
                case "not approved":
                    return new OptionSetValue((int)invln_ExternalStatus.SentforApproval);
                case "application declined":
                    return new OptionSetValue((int)invln_ExternalStatus.ApplicationDeclined);
                case "approved subject to contract":
                    return new OptionSetValue((int)invln_ExternalStatus.ApprovedSubjecttoContract);
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
                case (int)invln_ExternalStatus.ApplicationSubmitted:
                    return "submitted";
                case (int)invln_ExternalStatus.UnderReview:
                    return "under review";
                case (int)invln_ExternalStatus.Induediligence:
                    return "in due diligence";
                case (int)invln_ExternalStatus.ContractSignedSubjecttoCP:
                    return "contract signed subject to cp";
                case (int)invln_ExternalStatus.CPsSatisfied:
                    return "cps satisfied";
                case (int)invln_ExternalStatus.LoanAvailable:
                    return "loan available";
                case (int)invln_ExternalStatus.HoldRequested:
                    return "hold requested";
                case (int)invln_ExternalStatus.OnHold:
                    return "on hold";
                case (int)invln_ExternalStatus.ReferredBacktoApplicant:
                    return "referred back to applicant";
                case (int)invln_ExternalStatus.NA:
                    return "inactive";
                case (int)invln_ExternalStatus.Withdrawn:
                    return "withdrawn";
                case (int)invln_ExternalStatus.SentforApproval:
                    return "not approved";
                case (int)invln_ExternalStatus.ApplicationDeclined:
                    return "application declined";
                case (int)invln_ExternalStatus.ApprovedSubjecttoContract:
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
