using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk.Query;
using HE.Base.Repositories;
using DataverseModel;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Common.Repositories.Interfaces
{

    public interface IAccountRepository : ICrmEntityRepository<Account, DataverseContext>
    {
        Account GetDefaultAccount();
        Account RetrieveAccountById(EntityReference accountId, ColumnSet columnSet = null);
        bool AccountWithGivenIdExists(Guid accountId);
        List<Account> GetAccountsByOrganizationNameAndCompanyHouseName(string organizationName, string companyHouseName);
    }
}
