using System;
using System.Collections.Generic;
using System.Linq;
using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk.Client;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class ConditionRepository : CrmEntityRepository<invln_Conditions, DataverseContext>, IConditionRepository
    {
        public ConditionRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        public List<invln_Conditions> GetBespokeConditionsForLoanApplication(Guid loanId)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                return ctx.CreateQuery<invln_Conditions>()
                     .Where(x => x.invln_Loanapplication.Id == loanId && x.invln_ConditionSource.Value == (int)invln_ConditionSource.Bespoke).ToList();
            }
        }
    }
}
