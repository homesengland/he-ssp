using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using HE.CRM.Common.Repositories.Interfaces;
using DataverseModel;
using HE.Base.Repositories;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class IspRepository : CrmEntityRepository<invln_ISP, DataverseContext>, IIspRepository
    {
        public IspRepository(CrmRepositoryArgs args) : base(args)
        {
        }

    }
}
