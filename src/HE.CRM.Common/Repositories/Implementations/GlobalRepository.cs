using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Linq;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class GlobalRepository : CrmEntityRepository<Entity, DataverseContext>, IGlobalRepository
    {
        public GlobalRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        public Entity RetrieveEntityOfGivenTypeWithGivenFields(string entityName, Guid entityId, string[] fields)
        {
            QueryExpression qe = new QueryExpression(entityName);
            qe.ColumnSet = new ColumnSet(fields);
            qe.Criteria.AddCondition($"{entityName}id".ToLower(), ConditionOperator.Equal, entityId);
            var result = service.RetrieveMultiple(qe);
            if (result != null && result.Entities.Count > 0)
            {
                return result.Entities.AsEnumerable().FirstOrDefault();
            }
            return null;
        }
    }
}