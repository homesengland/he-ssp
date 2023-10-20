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
        List<invln_Loanapplication> GetLoanApplicationsForGivenAccountAndContact(Guid accountId, string externalContactId, string loanApplicationId = null, string fieldsToRetrieve = null);
        List<invln_Loanapplication> GetAccountLoans(Guid accountId);
        invln_sendinternalcrmnotificationResponse ExecuteNotificatioRequest(invln_sendinternalcrmnotificationRequest request);
        invln_sendgovnotifyemailResponse ExecuteGovNotifyNotificationRequest(invln_sendgovnotifyemailRequest request);
        bool LoanWithGivenNameExists(string loanName, Guid organisationId);
        invln_Loanapplication GetLoanApplicationRelatedToSiteDetails(Guid siteDetailsId);
    }
}
