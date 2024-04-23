using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk.Query;
using HE.Base.Repositories;
using DataverseModel;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Common.Repositories.Interfaces
{

    public interface IConsortiumRepository : ICrmEntityRepository<invln_Consortium, DataverseContext>
    {
        invln_Consortium GetConsortiumById(string consortiumId, ColumnSet columnSet);
    }
}
