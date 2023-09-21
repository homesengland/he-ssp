using System;
using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using System.Diagnostics;
using Microsoft.Xrm.Sdk.Client;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class LoanApplicationRepository : CrmEntityRepository<invln_Loanapplication, DataverseContext>, ILoanApplicationRepository
    {
        #region Constructors

        public LoanApplicationRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        public bool LoanWithGivenIdExists(Guid id)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                return ctx.CreateQuery<invln_Loanapplication>()
                    .Where(x => x.Id == id).AsEnumerable().Any();
            }
        }

        public List<invln_Loanapplication> GetLoanApplicationsForGivenAccountAndContact(Guid accountId, string externalContactId, string loanApplicationId = null)
        {
            using (DataverseContext ctx = new DataverseContext(service))
            {
                if (loanApplicationId == null)
                {
                    return (from la in ctx.invln_LoanapplicationSet
                            join cnt in ctx.ContactSet on la.invln_Contact.Id equals cnt.ContactId
                            where la.invln_Account.Id == accountId && cnt.invln_externalid == externalContactId &&
                            la.StatusCode.Value != (int)invln_Loanapplication_StatusCode.Inactive_Inactive &&
                            la.StatusCode.Value != (int)invln_Loanapplication_StatusCode.Inactive_Active && la.StateCode.Value != (int)invln_loanapplicationState.Inactive
                            select la).ToList();
                }
                else
                {
                    if (Guid.TryParse(loanApplicationId, out Guid loanApplicationGuid))
                    {
                        return (from la in ctx.invln_LoanapplicationSet
                                join cnt in ctx.ContactSet on la.invln_Contact.Id equals cnt.ContactId
                                where la.invln_Account.Id == accountId && cnt.invln_externalid == externalContactId && la.invln_LoanapplicationId == loanApplicationGuid &&
                                la.StatusCode.Value != (int)invln_Loanapplication_StatusCode.Inactive_Inactive &&
                                la.StatusCode.Value != (int)invln_Loanapplication_StatusCode.Inactive_Active && la.StateCode.Value != (int)invln_loanapplicationState.Inactive
                                select la).ToList();
                    }
                    else
                    {
                        return new List<invln_Loanapplication>();
                    }
                }
            }
        }

        public List<invln_Loanapplication> GetAccountLoans(Guid accountId)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                return ctx.CreateQuery<invln_Loanapplication>()
                    .Where(x => x.invln_Account.Id == accountId && x.StatusCode.Value != (int)invln_Loanapplication_StatusCode.Inactive_Inactive &&
                            x.StatusCode.Value != (int)invln_Loanapplication_StatusCode.Inactive_Active && x.StateCode.Value != (int)invln_loanapplicationState.Inactive).ToList();
            }
        }

        public invln_sendinternalcrmnotificationResponse ExecuteNotificatioRequest(invln_sendinternalcrmnotificationRequest request)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                return (invln_sendinternalcrmnotificationResponse)ctx.Execute(request);
            }
        }

        #endregion

        #region Interface Implementation


        #endregion
    }
}
