using DataverseModel;
using HE.Base.Services;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Drawing.Text;
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
            var loanApplicationFromPortal = JsonSerializer.Deserialize<LoanApplicationDto>(loanApplicationPayload);
            if (loanApplicationFromPortal.id != null && loanApplicationRepository.LoanWithGivenIdExists(loanApplicationFromPortal.id))
            {
                var loanToDelete = new invln_Loanapplication()
                {
                    Id = loanApplicationFromPortal.id,
                };
                loanApplicationRepository.Delete(loanToDelete);
            }
            Account account = PrepareAccount(loanApplicationFromPortal);
            var accountExists = accountRepository.AccountWithGivenIdExists(loanApplicationFromPortal.accountId);
            if (accountExists)
            {
                account.Id = loanApplicationFromPortal.accountId;
                accountRepository.Update(account);
            }
            else
            {
                account.Id = accountRepository.Create(account);
            }
            var loanApplicationToCreate = new invln_Loanapplication()
            {
                invln_Name = loanApplicationFromPortal.name,
                invln_NumberofSites = loanApplicationFromPortal.numberOfSites,
                invln_CompanyExperience = loanApplicationFromPortal.companyExperience,
                invln_CompanyPurpose = loanApplicationFromPortal.companyPurpose,
                invln_Companystructureinformation = loanApplicationFromPortal.companyStructureInformation,
                invln_Confirmationdirectorloanscanbesubordinated = loanApplicationFromPortal.confirmationDirectorLoansCanBeSubordinated,
                invln_Costsforadditionalprojects = loanApplicationFromPortal.costsForAdditionalProjects,
                invln_DebentureHolder = loanApplicationFromPortal.debentureHolder,
                invln_Directorloans = loanApplicationFromPortal.directorLoads,
                invln_ExistingCompany = loanApplicationFromPortal.existingCompany,
                invln_FundingReason = loanApplicationFromPortal.fundingReason,
                invln_Fundingtypeforadditionalprojects = loanApplicationFromPortal.fundingTypeForAdditionalProjects,
                invln_Outstandinglegalchargesordebt = loanApplicationFromPortal.outstandingLegalChargesOrDebt,
                invln_Privatesectorapproach = loanApplicationFromPortal.privateSectorApproach,
                invln_Privatesectorapproachinformation = loanApplicationFromPortal.privateSectorApproachInformation,
                invln_Projectabnormalcosts = loanApplicationFromPortal.projectAbnormalCosts,
                invln_Projectabnormalcostsinformation = loanApplicationFromPortal.projectAbnormalCostsInformation,
                invln_Projectestimatedtotalcost = loanApplicationFromPortal.projectEstimatedTotalCost,
                invln_ProjectGDV = loanApplicationFromPortal.projectGdv,
                invln_Reasonfordirectorloannotsubordinated = loanApplicationFromPortal.reasonForDirectorLoanNotSubordinated,
                invln_Refinancerepayment = loanApplicationFromPortal.refinanceRepayment,
                invln_Refinancerepaymentdetails = loanApplicationFromPortal.refinanceRepaymentDetails,
                invln_Account = account.ToEntityReference(),
                invln_Contact = account.PrimaryContactId,
            };
            var loanApplicationGuid = loanApplicationRepository.Create(loanApplicationToCreate);
            if(loanApplicationFromPortal.siteDetailsList.Count > 0)
            {
                foreach(var siteDetail in loanApplicationFromPortal.siteDetailsList)
                {
                    var siteDetailToCreate = new invln_SiteDetails()
                    {
                        invln_currentvalue = siteDetail.currentValue,
                        invln_Dateofpurchase = siteDetail.dateOfPurchase,
                        invln_Existinglegalcharges = siteDetail.existingLegalCharges,
                        invln_Existinglegalchargesinformation = siteDetail.existingLegalChargesInformation,
                        invln_Haveaplanningreferencenumber = siteDetail.haveAPlanningReferenceNumber,
                        invln_HowMuch = siteDetail.howMuch,
                        invln_Landregistrytitlenumber = siteDetail.landRegistryTitleNumber,
                        invln_Loanapplication = new Microsoft.Xrm.Sdk.EntityReference(invln_Loanapplication.EntityLogicalName, loanApplicationGuid),
                        invln_Name = siteDetail.Name,
                        invln_Nameofgrantfund = siteDetail.nameOfGrantFund,
                        invln_Numberofaffordablehomes = siteDetail.numberOfAffordableHomes,
                        invln_Numberofhomes = siteDetail.numberOfHomes,
                        invln_OtherTypeofhomes = siteDetail.otherTypeOfHomes,
                        invln_Planningreferencenumber = siteDetail.planningReferenceNumber,
                        invln_Publicsectorfunding = siteDetail.publicSectorFunding,
                        invln_Reason = siteDetail.reason,
                        invln_Sitecoordinates = siteDetail.siteCoordinates,
                        invln_Sitecost = siteDetail.siteCost,
                        invln_Sitename = siteDetail.siteName,
                        invln_Siteownership = siteDetail.siteOwnership,
                        invln_Typeofhomes = siteDetail.typeOfHomes,
                        invln_TypeofSite = siteDetail.typeOfSite,
                        invln_Valuationsource = siteDetail.valuationSource,
                        invln_Whoprovided = siteDetail.whoProvided,
                    };
                    siteDetailsRepository.Create(siteDetailToCreate);
                }
            }

            
            
        }

        private Account PrepareAccount(LoanApplicationDto loanApplicationFromPortal)
        {
            var retrievedContact = contactRepository.GetContactWithGivenEmail(loanApplicationFromPortal.contactEmailAdress);
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
