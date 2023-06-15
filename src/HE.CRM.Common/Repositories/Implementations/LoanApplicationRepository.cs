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

        public List<invln_Loanapplication> GetContactLoans(EntityReference contactId)
        {
            logger.Trace("Get Loans for Contact");
            List<invln_Loanapplication> loanapplications = new List<invln_Loanapplication>();
            if (contactId != null)
            {
                logger.Trace("Contact id not null. Id: " + contactId.Id);
                QueryExpression qe = new QueryExpression(invln_Loanapplication.EntityLogicalName);
                qe.ColumnSet = new ColumnSet(nameof(invln_Loanapplication.invln_Contact).ToLower());
                qe.Criteria.AddCondition(nameof(invln_Loanapplication.invln_Contact).ToLower(), ConditionOperator.Equal, contactId.Id);
                var result = service.RetrieveMultiple(qe);
                if(result != null && result.Entities.Count > 0)
                {
                    logger.Trace("Loans: " + result.Entities.Count);
                    loanapplications.AddRange(result.Entities.Select(x => x.ToEntity<invln_Loanapplication>()));
                }
            }

            return loanapplications;
        }

       

        #endregion

        #region Interface Implementation


        #endregion
    }
}
