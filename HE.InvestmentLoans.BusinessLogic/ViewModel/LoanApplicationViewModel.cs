using HE.InvestmentLoans.BusinessLogic._LoanApplication.Workflow;
using HE.InvestmentLoans.BusinessLogic.Enums;
using HE.InvestmentLoans.CRM.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace HE.InvestmentLoans.BusinessLogic.ViewModel
{
    public class LoanApplicationViewModel
    {

        public LoanApplicationViewModel()
        {
            Company = new CompanyStructureViewModel();
            Funding = new FundingViewModel();
            Security = new SecurityViewModel();
            Sites = new List<SiteViewModel>();
            ID = Guid.NewGuid();
            State = LoanApplicationWorkflow.State.AboutLoan;
            Account = TemporaryAccount();


        }

        private AccountDetailsViewModel TemporaryAccount() => new AccountDetailsViewModel
        {
            RegisteredName = "ABC Developments",
            RegistrationNumber = "AC012345",
            Address = new AddressViewModel
            {
                City = "Leeds",
                Country = "United Kingdom",
                Postcode = "L21 37W",
                Street = "12 Wharf Street"
            },
            ContactName = "Johan Aswani",
            EmailAddress = "example@mail.com",
            TelephoneNumber = "01238 956738"
        };

        public Guid ID { get; set; }
        public LoanApplicationWorkflow.State State
        {
            get;
            set;
        }
        public DateTime Timestamp { get; set; }
        public CompanyStructureViewModel Company { get; set; }
        public FundingViewModel Funding { get; set; }
        public SecurityViewModel Security { get; set; }
        public List<SiteViewModel> Sites { get; set; }

        public AccountDetailsViewModel Account { get; set; }
        public FundingPurpose? Purpose { get; set; }
    }
}
