using HE.Base.Repositories;
using HE.Base.Services;
using HE.CRM.Common.Extensions;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.CRM.Common.Repositories;
using DataverseModel;
using HE.CRM.Common.Repositories.Interfaces;
using HE.CRM.Common.Repositories.Implementations;

namespace HE.CRM.Plugins.Services.Accounts
{
    public class AccountService : CrmService, IAccountService
    {
        #region Fields

        private readonly IAccountRepository accountRepository;

        #endregion

        #region Constructors

        public AccountService(CrmServiceArgs args) : base(args)
        {
            accountRepository = CrmRepositoriesFactory.Get<IAccountRepository>();
        }

        #endregion

        #region Public Methods

        public string GenerateRandomAccountSampleName()
        {
            return this.GenerateRandomNumber().ToString();
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
