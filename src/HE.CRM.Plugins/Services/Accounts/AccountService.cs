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

        public OrganizationDetailsDto GetOrganizationDetails(string accountid, string contactExternalId)
        {
            var organizationDetailsDto = new OrganizationDetailsDto();
            if (Guid.TryParse(accountid, out var organizationId))
            {
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

        public List<OrganizationDetailsDto> SearchOrganizationByNameAndCompanyHouseNumber(string organizationName, string companyHouseNumber)
        {
            var accounts = _accountRepository.GetAccountsByOrganizationNameAndCompanyHouseName(organizationName, companyHouseNumber);
            if(accounts != null && accounts.Count > 0)
            {
                var accountsToReturn = new List<OrganizationDetailsDto>();
                foreach(var account in accounts)
                {
                    var accountToSerialize = new OrganizationDetailsDto()
                    {
                        addressLine1 = account.Address1_Line1,
                        addressLine2 = account.Address1_Line2,
                        addressLine3 = account.Address1_Line3,
                        city = account.Address1_City,
                        postalcode = account.Address1_PostalCode,
                        country = account.Address1_Country,
                        registeredCompanyName = account.Name,
                        companyRegistrationNumber = account.he_CompaniesHouseNumber,
                    };
                    accountsToReturn.Add(accountToSerialize);
                }
                return accountsToReturn;
            }
            return new List<OrganizationDetailsDto>();
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
