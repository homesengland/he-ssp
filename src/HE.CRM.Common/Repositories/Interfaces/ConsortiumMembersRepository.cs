using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System;
using Microsoft.Xrm.Sdk.Client;
using System.Linq;
using System.Collections.Generic;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class ConsortiumMemberRepository : CrmEntityRepository<invln_ConsortiumMember, DataverseContext>, IConsortiumMemberRepository
    {
        public ConsortiumMemberRepository(CrmRepositoryArgs args) : base(args)
        {
        }

    }
}
