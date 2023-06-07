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
           using(var ctx = new OrganizationServiceContext(service))
            {
                return ctx.CreateQuery<invln_Loanapplication>()
                    .Where(x => x.Id == id).AsEnumerable().Any();
            }
        }

        #endregion

        #region Interface Implementation


        #endregion
    }
}
