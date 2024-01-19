using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using HE.Base.Repositories;
using DataverseModel;
using HE.CRM.Common.Repositories.Interfaces;

namespace HE.CRM.Common.Repositories.implementations
{
    public class AhpStatusChangeRepository : CrmEntityRepository<invln_AHPStatusChange, DataverseContext>, IAhpStatusChangeRepository
    {
        public AhpStatusChangeRepository(CrmRepositoryArgs args) : base(args)
        {
        }
    }
}
