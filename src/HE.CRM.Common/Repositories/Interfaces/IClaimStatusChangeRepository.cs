using System;
using System.Collections.Generic;
using System.Text;
using DataverseModel;
using HE.Base.Repositories;

namespace HE.CRM.Common.Repositories.Interfaces
{
    public interface IClaimStatusChangeRepository : ICrmEntityRepository<invln_ClaimsStatusChange, DataverseContext>
    {
    }
}
