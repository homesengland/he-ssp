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

        public List<invln_Loanapplication> GetLoanApplicationsForGivenAccountAndContact(Guid accountId, string externalContactId, string loanApplicationId = null, string fieldsToRetrieve = null)
        {

            if (loanApplicationId == null)
            {
                using (DataverseContext ctx = new DataverseContext(service))
                {
                    return (from la in ctx.invln_LoanapplicationSet
                            join cnt in ctx.ContactSet on la.invln_Contact.Id equals cnt.ContactId
                            where la.invln_Account.Id == accountId && cnt.invln_externalid == externalContactId &&
                            la.StatusCode.Value != (int)invln_Loanapplication_StatusCode.Inactive
                            select la).ToList();

                }
            }
            else
            {
                if (Guid.TryParse(loanApplicationId, out Guid loanApplicationGuid))
                {
                    var fetchXML = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
	                                <entity name='invln_loanapplication'>"
                                    + fieldsToRetrieve +
                                    @"<filter>
                                              <condition attribute=""statuscode"" operator=""ne"" value=""2"" />
                                              <condition attribute=""invln_account"" operator=""eq"" value=""" + accountId + @""" />
                                              <condition attribute=""invln_loanapplicationid"" operator=""eq"" value=""" + loanApplicationId + @""" />
                                            </filter>
                                            <link-entity name=""contact"" from=""contactid"" to=""invln_contact"">
                                              <filter>
                                                <condition attribute=""invln_externalid"" operator=""eq"" value=""" + externalContactId + @""" />
                                              </filter>
                                            </link-entity>
                                          </entity>
                                        </fetch>";
                    EntityCollection result = service.RetrieveMultiple(new FetchExpression(fetchXML));
                    return result.Entities.Select(x => x.ToEntity<invln_Loanapplication>()).ToList();
                }
                else
                {
                    return new List<invln_Loanapplication>();
                }
            }
        }

        public List<invln_Loanapplication> GetAccountLoans(Guid accountId)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                return ctx.CreateQuery<invln_Loanapplication>()
                    .Where(x => x.invln_Account.Id == accountId && x.StatusCode.Value != (int)invln_Loanapplication_StatusCode.Inactive && x.StateCode.Value != (int)invln_loanapplicationState.Inactive).ToList();
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

        public invln_sendgovnotifyemailResponse ExecuteGovNotifyNotificationRequest(invln_sendgovnotifyemailRequest request)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                return (invln_sendgovnotifyemailResponse)ctx.Execute(request);
            }
        }

        public bool LoanWithGivenNameExists(string loanName, Guid organisationId)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                return ctx.CreateQuery<invln_Loanapplication>()
                    .Where(x => x.invln_Name == loanName && x.invln_Account.Id == organisationId).AsEnumerable().Any();
            }
        }

        #endregion
    }
}
