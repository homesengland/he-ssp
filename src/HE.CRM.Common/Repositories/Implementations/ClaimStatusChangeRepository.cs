using System;
using System.Collections.Generic;
using System.Text;
using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class ClaimStatusChangeRepository : CrmEntityRepository<invln_ClaimsStatusChange, DataverseContext>, IClaimStatusChangeRepository
    {
        public ClaimStatusChangeRepository(CrmRepositoryArgs args) : base(args)
        {
        }
    }
}
