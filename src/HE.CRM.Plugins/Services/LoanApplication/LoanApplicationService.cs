using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms.VisualStyles;

namespace HE.CRM.Plugins.Services.LoanApplication
{
    public class LoanApplicationService : CrmService, ILoanApplicationService
    {
        #region Fields

        private readonly ILoanApplicationRepository loanApplicationRepository;
        private readonly ISiteDetailsRepository siteDetailsRepository;
        private readonly IAccountRepository accountRepository;
        private readonly IContactRepository contactRepository;

        #endregion

        #region Constructors

        public LoanApplicationService(CrmServiceArgs args) : base(args)
        {
            loanApplicationRepository = CrmRepositoriesFactory.Get<ILoanApplicationRepository>();
            siteDetailsRepository = CrmRepositoriesFactory.Get<ISiteDetailsRepository>();
            accountRepository = CrmRepositoriesFactory.Get<IAccountRepository>();
            contactRepository = CrmRepositoriesFactory.Get<IContactRepository>();
        }

        #endregion

        #region Public Methods

        public string GetLoanApplicationsForAccountAndContact(string externalContactId, string accountId, string loanApplicationId = null)
        {
            List<LoanApplicationDto> entityCollection = new List<LoanApplicationDto>();
            if (Guid.TryParse(accountId, out Guid accountGuid))
            {
                var loanApplicationsForAccountAndContact = loanApplicationRepository.GetLoanApplicationsForGivenAccountAndContact(accountGuid, externalContactId, loanApplicationId);
                foreach (var element in loanApplicationsForAccountAndContact)
                {
                    List<SiteDetailsDto> siteDetailsDtoList = new List<SiteDetailsDto>();
                    var siteDetailsList = siteDetailsRepository.GetSiteDetailRelatedToLoanApplication(element.ToEntityReference());
                    if(siteDetailsList != null)
                    {
                        foreach(var siteDetail in siteDetailsList)
                        {
                            siteDetailsDtoList.Add(MapSiteDetailsToDto(siteDetail));
                        }
                    }
                    entityCollection.Add(MapLoanApplicationToDto(element, siteDetailsDtoList));
                }
            }
            return JsonSerializer.Serialize(entityCollection);
        }

        public string CreateRecordFromPortal(string contactExternalId, string accountId, string loanApplicationId, string loanApplicationPayload)
        {
            this.TracingService.Trace("PAYLOAD:" + loanApplicationPayload);
            LoanApplicationDto loanApplicationFromPortal = JsonSerializer.Deserialize<LoanApplicationDto>(loanApplicationPayload);
            Contact contact = contactRepository.GetContactViaExternalId(contactExternalId);

            int numberOfSites = 0;
            if (loanApplicationFromPortal.siteDetailsList != null && loanApplicationFromPortal.siteDetailsList.Count > 0)
            {
                numberOfSites = loanApplicationFromPortal.siteDetailsList.Count;
            }

            this.TracingService.Trace("Setting up invln_Loanapplication");
            var loanApplicationToCreate = MapLoanApplicationDtoToRegularEntity(loanApplicationFromPortal, numberOfSites, contact, accountId);
           
            Guid loanApplicationGuid = Guid.NewGuid();
            if (!string.IsNullOrEmpty(loanApplicationId) && Guid.TryParse(loanApplicationId, out Guid loanAppId))
            {
                this.TracingService.Trace("Update invln_Loanapplication");
                loanApplicationGuid = loanAppId;
                loanApplicationToCreate.Id = loanAppId;
                loanApplicationRepository.Update(loanApplicationToCreate);
               // siteDetailsRepository.DeleteSiteDetailsRelatedToLoanApplication(new EntityReference(invln_Loanapplication.EntityLogicalName, loanAppId));
            }
            else
            {
                this.TracingService.Trace("Create invln_Loanapplication");
                loanApplicationGuid = loanApplicationRepository.Create(loanApplicationToCreate);
            }

            if (loanApplicationFromPortal.siteDetailsList != null && loanApplicationFromPortal.siteDetailsList.Count > 0)
            {
                this.TracingService.Trace($"siteDetailsList.Count {loanApplicationFromPortal.siteDetailsList.Count}");
                foreach (var siteDetail in loanApplicationFromPortal.siteDetailsList)
                {
                    this.TracingService.Trace("loop begin");
                    var siteDetailToCreate = MapSiteDetailsDtoToRegularEntity(siteDetail, loanApplicationGuid);
                    this.TracingService.Trace("create");
                    if(siteDetail.siteDetailsId != null)
                    {
                        siteDetailsRepository.Update(siteDetailToCreate);
                    }
                    else
                    {
                        siteDetailsRepository.Create(siteDetailToCreate);
                    }
                    this.TracingService.Trace("after create record");
                }
            }

            return loanApplicationGuid.ToString();
        }

        private invln_Loanapplication MapLoanApplicationDtoToRegularEntity(LoanApplicationDto loanApplicationDto, int numberOfSites, Contact contact, 
            string accountId)
        {
            var loanApplication = new invln_Loanapplication()
            {
                invln_NumberofSites = numberOfSites,
                invln_FundingReason = MapFundingReason(loanApplicationDto.fundingReason),
                invln_ExternalStatus = MapApplicationStatus(loanApplicationDto.loanApplicationStatus),

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
                Id = Guid.TryParse(loanApplicationDto.loanApplicationId, out Guid loanApplicationId) ? loanApplicationId : Guid.NewGuid(),
            };
            return loanApplication;
        }

        private invln_SiteDetails MapSiteDetailsDtoToRegularEntity(SiteDetailsDto siteDetail, Guid loanApplicationGuid)
        {
            var siteDetailToReturn = new invln_SiteDetails()
            {
                invln_currentvalue = ParseDecimalToMoney(siteDetail.currentValue),
                invln_Dateofpurchase = siteDetail.dateOfPurchase,
                invln_Existinglegalcharges = ParseBool(siteDetail.existingLegalCharges),
                invln_Existinglegalchargesinformation = siteDetail.existingLegalChargesInformation,
                invln_Haveaplanningreferencenumber = ParseBool(siteDetail.haveAPlanningReferenceNumber),
                invln_HowMuch = ParseDecimalToMoney(siteDetail.howMuch),
                invln_Landregistrytitlenumber = siteDetail.landRegistryTitleNumber,
                invln_Loanapplication = new EntityReference(invln_Loanapplication.EntityLogicalName, loanApplicationGuid),
                invln_Name = siteDetail.Name,
                invln_Nameofgrantfund = siteDetail.nameOfGrantFund,
                invln_Numberofhomes = ParseInt(siteDetail.numberOfHomes),
                invln_OtherTypeofhomes = siteDetail.otherTypeOfHomes,
                invln_Planningreferencenumber = siteDetail.planningReferenceNumber,
                invln_Publicsectorfunding = MapPublicSectorFunding(siteDetail.publicSectorFunding),
                invln_Reason = siteDetail.reason,
                invln_Sitecoordinates = siteDetail.siteCoordinates,
                invln_Sitecost = ParseDecimalToMoney(siteDetail.siteCost),
                invln_Sitename = siteDetail.siteName,
                invln_Siteownership = ParseBool(siteDetail.siteOwnership),
                invln_Typeofhomes = MapTypeOfHomes(siteDetail.typeOfHomes),
                invln_TypeofSite = MapTypeOfSite(siteDetail.typeOfSite),
                invln_Valuationsource = MapValuationSource(siteDetail.valuationSource),
                invln_Whoprovided = siteDetail.whoProvided,
                Id = Guid.TryParse(siteDetail.siteDetailsId, out Guid detailId) ? detailId : Guid.NewGuid(),
            };
            return siteDetailToReturn;
        }

        private SiteDetailsDto MapSiteDetailsToDto(invln_SiteDetails siteDetails)
        {
            var siteDetailToReturn = new SiteDetailsDto()
            {
               siteDetailsId = siteDetails.invln_SiteDetailsId?.ToString(),
               currentValue = (siteDetails.invln_currentvalue?.Value)?.ToString(),
               dateOfPurchase = siteDetails.invln_Dateofpurchase,
               existingLegalCharges = siteDetails.invln_Existinglegalcharges?.ToString(),
               existingLegalChargesInformation = siteDetails.invln_Existinglegalchargesinformation,
               haveAPlanningReferenceNumber = siteDetails.invln_Haveaplanningreferencenumber?.ToString(),
               howMuch = (siteDetails.invln_HowMuch?.Value)?.ToString(),
               landRegistryTitleNumber = siteDetails.invln_Landregistrytitlenumber,
               Name = siteDetails.invln_Name,
               nameOfGrantFund = siteDetails.invln_Nameofgrantfund,
               numberOfHomes = siteDetails.invln_Numberofhomes?.ToString(),
               otherTypeOfHomes = siteDetails.invln_OtherTypeofhomes,
               planningReferenceNumber = siteDetails.invln_Planningreferencenumber,
               publicSectorFunding = MapPublicSectorFundingOptionSetToString(siteDetails.invln_Publicsectorfunding),
               reason = siteDetails.invln_Reason,
               siteCoordinates = siteDetails.invln_Sitecoordinates,
               siteCost = siteDetails.invln_Sitecost?.Value.ToString(),
               siteName = siteDetails.invln_Sitename,
               siteOwnership = siteDetails.invln_Siteownership?.ToString(),
               typeOfHomes = MapTypeOfHomesOptionSetToString(siteDetails.invln_Typeofhomes),
               typeOfSite = MapTypeOfSiteOptionSetToString(siteDetails.invln_TypeofSite),
               valuationSource = MapValuationSourceOptionSetToString(siteDetails.invln_Valuationsource),
               whoProvided = siteDetails.invln_Whoprovided,
            };
            return siteDetailToReturn;
        }

        private LoanApplicationDto MapLoanApplicationToDto(invln_Loanapplication loanApplication, List<SiteDetailsDto> siteDetailsDtoList)
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
            };
            return loanApplicationDto;
        }

        private string MapFundingReasonOptionSetToString(OptionSetValue fundingReason)
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

        private OptionSetValue MapApplicationStatus(string applicationStatus)
        {
            switch (applicationStatus?.ToLower())
            {
                case "draft":
                    return new OptionSetValue((int)invln_ExternalStatus.Draft);
                case "submitted":
                    return new OptionSetValue((int)invln_ExternalStatus.Submitted);
            }

            return null;
        }

        private string MapApplicationStatusFromDtoToRegular(OptionSetValue applicationStatus)
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
            }
            return null;
        }

        private OptionSetValueCollection MapTypeOfHomes(string[] typeOfHomes)
        {
            if(typeOfHomes.Length > 0)
            {
                var collection = new OptionSetValueCollection();
                foreach(var home in typeOfHomes)
                {
                    switch (home?.ToLower())
                    {
                        case "apartmentsorflats":
                            collection.Add(new OptionSetValue((int)invln_Typeofhomes.Apartmentsorflats));
                            break;
                        case "bungalows":
                            collection.Add(new OptionSetValue((int)invln_Typeofhomes.Bungalows));
                            break;
                        case "extracareorassistedliving":
                            collection.Add(new OptionSetValue((int)invln_Typeofhomes.Extracareorassisted));
                            break;
                        case "houses":
                            collection.Add(new OptionSetValue((int)invln_Typeofhomes.Houses));
                            break;
                        case "other":
                            collection.Add(new OptionSetValue((int)invln_Typeofhomes.Other));
                            break;
                    }
                }
                return collection;
            }
            return null;
        }

        private string[] MapTypeOfHomesOptionSetToString(OptionSetValueCollection typeOfHomes)
        {
            if (typeOfHomes.Count > 0)
            {
                List<string> collection = new List<string>();
                foreach (var home in typeOfHomes)
                {
                    switch (home?.Value)
                    {
                        case (int)invln_Typeofhomes.Apartmentsorflats:
                            collection.Add("apartmentsorflats");
                            break;
                        case (int)invln_Typeofhomes.Bungalows:
                            collection.Add("bungalows");
                            break;
                        case (int)invln_Typeofhomes.Extracareorassisted:
                            collection.Add("extracareorassistedliving");
                            break;
                        case (int)invln_Typeofhomes.Houses:
                            collection.Add("houses");
                            break;
                        case (int)invln_Typeofhomes.Other:
                            collection.Add("other");
                            break;
                    }
                }
                return collection.ToArray();
            }
            return null;
        }

        private OptionSetValue MapPublicSectorFunding(string publicSectorFunding)
        {
            switch (publicSectorFunding?.ToLower())
            {
                case "no":
                    return new OptionSetValue((int)invln_Publicsectorfunding.No);
                case "donotknow":
                    return new OptionSetValue((int)invln_Publicsectorfunding.Donotknow);
                case "yes":
                    return new OptionSetValue((int)invln_Publicsectorfunding.Yes);
            }

            return null;
        }

        private string MapPublicSectorFundingOptionSetToString(OptionSetValue publicSectorFunding)
        {
            switch (publicSectorFunding?.Value)
            {
                case (int)invln_Publicsectorfunding.No:
                    return "no";
                case (int)invln_Publicsectorfunding.Donotknow:
                    return "donotknow";
                case (int)invln_Publicsectorfunding.Yes:
                    return "yes";
            }

            return null;
        }

        private OptionSetValue MapValuationSource(string valuationSource)
        {
            switch (valuationSource?.ToLower())
            {
                case "customerestimate":
                    return new OptionSetValue((int)invln_Valuationsource.Customerestimate);
                case "ricsredbookvaluation":
                    return new OptionSetValue((int)invln_Valuationsource.RICSRedBookvaluation);
                case "estateagentestimate":
                    return new OptionSetValue((int)invln_Valuationsource.Estateagentestimate);
            }

            return null;
        }

        private string MapValuationSourceOptionSetToString(OptionSetValue valuationSource)
        {
            switch (valuationSource?.Value)
            {
                case (int)invln_Valuationsource.Customerestimate:
                    return "customerestimate";
                case (int)invln_Valuationsource.RICSRedBookvaluation:
                    return "ricsredbookvaluation";
                case (int)invln_Valuationsource.Estateagentestimate:
                    return "estateagentestimate";
            }

            return null;
        }

        private OptionSetValue MapTypeOfSite(string typeOfSite)
        {
            switch (typeOfSite?.ToLower())
            {
                case "greenfield":
                    return new OptionSetValue((int)invln_TypeofSite.Greenfield);
                case "brownfield":
                    return new OptionSetValue((int)invln_TypeofSite.Brownfield);
            }

            return null;
        }

        private string MapTypeOfSiteOptionSetToString(OptionSetValue typeOfSite)
        {
            switch (typeOfSite?.Value)
            {
                case (int)invln_TypeofSite.Greenfield:
                    return "greenfield";
                case (int)invln_TypeofSite.Brownfield:
                    return "brownfield";
            }

            return null;
        }

        private OptionSetValue MapFundingReason(string fundingReason)
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

        private OptionSetValue MapRefinancePayment(string refinancePayment)
        {
            switch(refinancePayment?.ToLower())
            {
                case "refinance":
                    return new OptionSetValue((int)invln_refinancerepayment.Refinance);
                case "repay":
                    return new OptionSetValue((int)invln_refinancerepayment.Repay);
            }

            return null;
        }

        private string MapRefinancePaymentOptionSetToString(OptionSetValue refinancePayment)
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
        private bool? ParseBool(string boolToParse)
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

        private int? ParseInt(string intToParse)
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

        private Money ParseDecimalToMoney(string decimalToParse)
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

        private Account PrepareAccount(LoanApplicationDto loanApplicationFromPortal)
        {
            var retrievedContact = contactRepository.GetFirstContactWithGivenEmail(loanApplicationFromPortal.contactEmailAdress);
            Account preparedAccount;
            if (retrievedContact != null)
            {
                preparedAccount = new Account()
                {
                    PrimaryContactId = retrievedContact.ToEntityReference(),
                };
            }
            else
            {
                var contactToCreate = new Contact()
                {
                    EMailAddress1 = loanApplicationFromPortal.contactEmailAdress,
                    FirstName = "custom api",
                    LastName = "test jb",
                };
                var createdContactGuid = contactRepository.Create(contactToCreate);
                preparedAccount = new Account()
                {
                    PrimaryContactId = new Microsoft.Xrm.Sdk.EntityReference(Contact.EntityLogicalName, createdContactGuid),
                };
            }
            return preparedAccount;
        }

        #endregion
    }
}
