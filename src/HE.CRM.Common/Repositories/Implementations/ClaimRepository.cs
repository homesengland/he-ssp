using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk.Query;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class ClaimRepository : CrmEntityRepository<invln_Claim, DataverseContext>, IClaimRepository
    {
        public ClaimRepository(CrmRepositoryArgs args) : base(args)
        {
        }
    }
}

