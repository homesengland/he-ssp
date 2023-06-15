using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.Repositories.Implementations;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text.Json;

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


        public void CreateRecordFromPortal(string loanApplicationPayload)
        {
            this.TracingService.Trace("PAYLOAD:" + loanApplicationPayload);
            LoanApplicationDto loanApplicationFromPortal = JsonSerializer.Deserialize<LoanApplicationDto>(loanApplicationPayload);
            List<Contact> relatedContacts = contactRepository.GetAllContactsWithGivenEmail(loanApplicationFromPortal.contactEmailAdress);
            EntityReference contactForPortalUser = null;

            foreach (var contact in relatedContacts)
            {
                var loanApplications = loanApplicationRepository.GetContactLoans(contact.ToEntityReference());
                foreach(var loanApp in loanApplications)
                {
                    this.TracingService.Trace("Delete existing Loan Application");
                    var loanToDelete = new invln_Loanapplication()
                    {
                        Id = loanApp.Id,
                    };

                    loanApplicationRepository.Delete(loanToDelete);
                }
            }

            if(relatedContacts.Count == 0)
            {
                this.TracingService.Trace("Create contact");
                var createdContact = contactRepository.Create(new Contact()
                {
                    EMailAddress1 = loanApplicationFromPortal.contactEmailAdress
                });

                contactForPortalUser = new EntityReference(Contact.EntityLogicalName.ToLower(), createdContact);
            }
            else
            {
                this.TracingService.Trace("Assign existing contact");
                contactForPortalUser = relatedContacts.First().ToEntityReference();
            }

            /*Account account = PrepareAccount(loanApplicationFromPortal);
            var accountExists = accountRepository.AccountWithGivenIdExists(loanApplicationFromPortal.accountId);
            if (accountExists)
            {
                //Update existing account
                account.Id = loanApplicationFromPortal.accountId;
                accountRepository.Update(account);
            }
            else
            {
                //Create new account
                account.Id = accountRepository.Create(account);
            }*/

            this.TracingService.Trace("Setting up invln_Loanapplication");
            var loanApplicationToCreate = new invln_Loanapplication()
            {
                invln_NumberofSites = ParseInt(loanApplicationFromPortal.numberOfSites),
                invln_FundingReason = MapFundingReason(loanApplicationFromPortal.fundingReason),

                //COMPANY
                invln_CompanyPurpose = ParseBool(loanApplicationFromPortal.companyPurpose), //Purpose
                invln_Companystructureinformation = loanApplicationFromPortal.existingCompany, //ExistingCompany
                invln_CompanyExperience = loanApplicationFromPortal.companyExperience, //HomesBuilt
                                                                                       //Company.CompanyInfoFile

                //FUNDING
                invln_ProjectGDV = ParseDecimalToMoney(loanApplicationFromPortal.projectGdv), //GDV
                invln_Projectestimatedtotalcost = ParseDecimalToMoney(loanApplicationFromPortal.projectEstimatedTotalCost), //TotalCosts
                invln_Projectabnormalcosts = ParseBool(loanApplicationFromPortal.projectAbnormalCosts), //AbnormalCosts
                invln_Projectabnormalcostsinformation = loanApplicationFromPortal.projectAbnormalCostsInformation, //AbnormalCosts
                invln_Privatesectorapproach = ParseBool(loanApplicationFromPortal.privateSectorApproach), //PrivateSectorFunding
                invln_Privatesectorapproachinformation = loanApplicationFromPortal.privateSectorApproachInformation, //PrivateSectorFunding
                invln_Additionalprojects = ParseBool(loanApplicationFromPortal.additionalProjects), //AdditionalProjects
                //invln_Refinancerepayment = loanApplicationFromPortal.refinanceRepayment, //Refinance
                invln_Refinancerepaymentdetails = loanApplicationFromPortal.refinanceRepaymentDetails, //Refinance


                //SECURITY
                invln_Outstandinglegalchargesordebt = ParseBool(loanApplicationFromPortal.outstandingLegalChargesOrDebt), //ChargesDebtCompany
                invln_DebentureHolder = loanApplicationFromPortal.debentureHolder, //ChargesDebtCompany
                invln_Directorloans = ParseBool(loanApplicationFromPortal.directorLoans), //DirLoans
                invln_Confirmationdirectorloanscanbesubordinated = ParseBool(loanApplicationFromPortal.confirmationDirectorLoansCanBeSubordinated), //DirLoansSub
                invln_Reasonfordirectorloannotsubordinated = loanApplicationFromPortal.reasonForDirectorLoanNotSubordinated, //DirLoansSub

                //OTHER maybe not related
                //invln_Name = loanApplicationFromPortal.name,
                //invln_Account = account.ToEntityReference(),
                invln_Contact = contactForPortalUser,
            };

            this.TracingService.Trace("Create invln_Loanapplication");
            var loanApplicationGuid = loanApplicationRepository.Create(loanApplicationToCreate);
            //if (loanApplicationFromPortal.siteDetailsList.Count > 0)
            //{
            //    foreach (var siteDetail in loanApplicationFromPortal.siteDetailsList)
            //    {
            //        var siteDetailToCreate = new invln_SiteDetails()
            //        {
            //            invln_currentvalue = ParseDecimalToMoney(siteDetail.currentValue),
            //            invln_Dateofpurchase = siteDetail.dateOfPurchase,
            //            invln_Existinglegalcharges = ParseBool(siteDetail.existingLegalCharges),
            //            invln_Existinglegalchargesinformation = siteDetail.existingLegalChargesInformation,
            //            invln_Haveaplanningreferencenumber = ParseBool(siteDetail.haveAPlanningReferenceNumber),
            //            invln_HowMuch = ParseDecimalToMoney(siteDetail.howMuch),
            //            invln_Landregistrytitlenumber = siteDetail.landRegistryTitleNumber,
            //            invln_Loanapplication = new Microsoft.Xrm.Sdk.EntityReference(invln_Loanapplication.EntityLogicalName, loanApplicationGuid),
            //            invln_Name = siteDetail.Name,
            //            invln_Nameofgrantfund = siteDetail.nameOfGrantFund,
            //            //invln_Numberofaffordablehomes = ParseInt(siteDetail.numberOfAffordableHomes),
            //            invln_Numberofhomes = ParseInt(siteDetail.numberOfHomes),
            //            invln_OtherTypeofhomes = siteDetail.otherTypeOfHomes,
            //            invln_Planningreferencenumber = siteDetail.planningReferenceNumber,
            //            //invln_Publicsectorfunding = siteDetail.publicSectorFunding,
            //            invln_Reason = siteDetail.reason,
            //            invln_Sitecoordinates = siteDetail.siteCoordinates,
            //            invln_Sitecost = ParseDecimalToMoney(siteDetail.siteCost),
            //            invln_Sitename = siteDetail.siteName,
            //            invln_Siteownership = ParseBool(siteDetail.siteOwnership),
            //            //invln_Typeofhomes = siteDetail.typeOfHomes,
            //            //invln_TypeofSite = siteDetail.typeOfSite,
            //            //invln_Valuationsource = siteDetail.valuationSource,
            //            invln_Whoprovided = siteDetail.whoProvided,
            //        };
            //        siteDetailsRepository.Create(siteDetailToCreate);
            //    }
            //}
        }

        private OptionSetValue MapFundingReason(string fundingReason)
        {
            switch (fundingReason)
            {
                case "Buildinginfrastructureonly":
                    return new OptionSetValue((int)invln_FundingReason.Buildinginfrastructureonly);
                case "Buildingnewhomes":
                    return new OptionSetValue((int)invln_FundingReason.Buildingnewhomes);
                case "Other":
                    return new OptionSetValue((int)invln_FundingReason.Other);
            }

            return null;
        }
        private bool? ParseBool(string boolToParse)
        {
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
