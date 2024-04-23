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
    public class ConsortiumRepository : CrmEntityRepository<invln_Consortium, DataverseContext>, IConsortiumRepository
    {
        public ConsortiumRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        public invln_Consortium GetConsortiumById(string consortiumId, ColumnSet columnSet = null)
        {
            var query = new QueryExpression
            {
                ColumnSet = columnSet ?? new ColumnSet(true),
                EntityName = invln_Consortium.EntityLogicalName
            };
            query.Criteria.AddCondition(invln_Consortium.Fields.Id, ConditionOperator.Equal, consortiumId);
            return service.RetrieveMultiple(query).Entities.Select(x => x.ToEntity<invln_Consortium>()).FirstOrDefault();
        }
    }
}
