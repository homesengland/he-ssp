using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk.Query;
using HE.Base.Repositories;
using DataverseModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Crm.Sdk.Messages;

namespace HE.CRM.Common.Repositories.Interfaces
{
    public interface IIspRepository : ICrmEntityRepository<invln_ISP, DataverseContext>
    {
    }
}
