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
    public class LoanStatusChangeRepository : CrmEntityRepository<invln_Loanstatuschange, DataverseContext>, ILoanStatusChangeRepository
    {
        #region Constructors

        public LoanStatusChangeRepository(CrmRepositoryArgs args) : base(args)
        {
        }
        #endregion
    }
}
