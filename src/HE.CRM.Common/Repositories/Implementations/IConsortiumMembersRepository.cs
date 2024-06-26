using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk.Query;
using HE.Base.Repositories;
using DataverseModel;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Common.Repositories.Interfaces
{

    public interface IConsortiumMemberRepository : ICrmEntityRepository<invln_ConsortiumMember, DataverseContext>
    {
        invln_ConsortiumMember GetMemberByOrganizstationIdAndConsortiumId(string organizationId, string consortiumId);
    }
}
