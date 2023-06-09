using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk.Query;
using HE.Base.Repositories;
using DataverseModel;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Common.Repositories.Interfaces
{

    public interface ILoanApplicationRepository : ICrmEntityRepository<invln_Loanapplication, DataverseContext>
    {
        bool LoanWithGivenIdExists(Guid id);
        List<invln_Loanapplication> GetContactLoans(EntityReference contactId);
        List<invln_Loanapplication> GetLoanApplicationsForGivenAccountAndContact(Guid accountId, string externalContactId, string loanApplicationId = null);
    }
}
