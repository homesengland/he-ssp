using HE.Base.Services;
using System;
using System.Collections.Generic;
using DataverseModel;
using HE.CRM.Common.Repositories.Interfaces;
using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.CRM.Plugins.Services.Accounts
{
    public class AccountService : CrmService, IAccountService
    {
        #region Fields

        private readonly IAccountRepository _accountRepository;
        private readonly IContactRepository _contactRepository;

        private readonly string youRequested = "You requested";
        private readonly string someoneElseRequested = "Someoneelse requested";
        private readonly string noRequest = "No request";

        #endregion

        #region Constructors

        public AccountService(CrmServiceArgs args) : base(args)
        {
            _accountRepository = CrmRepositoriesFactory.Get<IAccountRepository>();
            _contactRepository = CrmRepositoriesFactory.Get<IContactRepository>();
        }

        #endregion

        #region Public Methods

        public string GenerateRandomAccountSampleName()
        {
            return this.GenerateRandomNumber().ToString();
        }

        public string GetOrganisationChangeDetails(string accountId)
        {
            if(Guid.TryParse(accountId, out Guid accountGuid))
            {
                var account = _accountRepository.GetById(accountGuid);
                return youRequested;
            }
            return string.Empty;
        }

        public OrganizationDetailsDto GetOrganizationDetails(string accountid, string contactExternalId)
        {
            var organizationDetailsDto = new OrganizationDetailsDto();
            TracingService.Trace($"accountid : {accountid}");
            TracingService.Trace($"externalid : {contactExternalId}");
            if (Guid.TryParse(accountid, out var organizationId))
            {
                TracingService.Trace($"organizationid : {organizationId}");
                var account = _accountRepository.GetById(organizationId, new string[] {
                    nameof(Account.Name).ToLower(),
                    nameof(Account.he_CompaniesHouseNumber).ToLower(),
                    nameof(Account.Address1_Line1).ToLower(),
                    nameof(Account.Address1_Line2).ToLower(),
                    nameof(Account.Address1_Line3).ToLower(),
                    nameof(Account.Address1_City).ToLower(),
                    nameof(Account.Address1_PostalCode).ToLower(),
                    nameof(Account.Address1_Country).ToLower(),
                    nameof(Account.PrimaryContactId).ToLower(),
                });

                organizationDetailsDto.registeredCompanyName = account.Name;
                organizationDetailsDto.companyRegistrationNumber = account.he_CompaniesHouseNumber;
                organizationDetailsDto.addressLine1 = account.Address1_Line1;
                organizationDetailsDto.addressLine2 = account.Address1_Line2;
                organizationDetailsDto.addressLine3 = account.Address1_Line3;
                organizationDetailsDto.city = account.Address1_City;
                organizationDetailsDto.postalcode = account.Address1_PostalCode;
                organizationDetailsDto.country = account.Address1_Country;

                if (account.PrimaryContactId != null)
                {
                    TracingService.Trace($"primary contact : {account.PrimaryContactId}");
                    var contact = _contactRepository.GetById(account.PrimaryContactId, new string[]
                    {
                        nameof(Contact.FullName).ToLower(),
                        nameof(Account.EMailAddress1).ToLower(),
                        nameof(Account.Telephone1).ToLower(),
                    });

                    organizationDetailsDto.compayAdminContactName = contact.FullName;
                    organizationDetailsDto.compayAdminContactEmail = contact.EMailAddress1;
                    organizationDetailsDto.compayAdminContactTelephone = contact.Telephone1;
                }
            }

            return organizationDetailsDto;
        }

        public void OnCurrentCrrFieldUpdate(Account target, Account preImage)
        {
            if (target != null && preImage != null)
            {
                target.invln_PreviousCRR = preImage.invln_CurrentCRR;
                target.invln_DateofCRRassessment = DateTime.Now;
            }
        }

        #endregion

        #region Private Methods

        private int GenerateRandomNumber()
        {
            Random random = new Random();
            return random.Next(100000);
        }

        #endregion
    }
}
