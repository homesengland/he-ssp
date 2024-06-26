using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System;
using Microsoft.Xrm.Sdk.Client;
using System.Linq;
using System.Collections.Generic;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class AccountRepository : CrmEntityRepository<Account, DataverseContext>, IAccountRepository
    {
        #region Constructors

        public AccountRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        #endregion Constructors

        #region Interface Implementation

        public Account GetDefaultAccount()
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                return ctx.CreateQuery<Account>()
                    .Where(x => x.Name == "DO_NOT_DELETE_DEFAULT_ACCOUNT").FirstOrDefault();
            }
        }

        public Account RetrieveAccountById(EntityReference accountId, ColumnSet columnSet = null)
        {
            logger.Trace("RetrieveAccountById");
            if (accountId != null)
            {
                logger.Trace("acount not null: " + accountId.LogicalName + accountId.Id);
                return service.Retrieve(accountId.LogicalName, accountId.Id, columnSet).ToEntity<Account>();
            }

            logger.Trace("acount null");

            return null;
        }

        public bool AccountWithGivenIdExists(Guid accountId)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                return ctx.CreateQuery<Account>()
                    .Where(x => x.Id == accountId).AsEnumerable().Any();
            }
        }

        #endregion Interface Implementation
    }
}
