using System.Collections.Generic;
using System.Linq;
using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk.Query;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class SecretVariableRepository : CrmEntityRepository<invln_SecretVariable, DataverseContext>, ISecretVariableRepository
    {
        #region Constructors

        public SecretVariableRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        #endregion Constructors

        #region Interface Implementation

        public IEnumerable<invln_SecretVariable> GetMultiple(string[] variableNames)
        {
            var logVariables = string.Join(", ", variableNames);
            logger.Trace($"{nameof(SecretVariableRepository)}.{nameof(GetMultiple)}, variables: {logVariables}");
            if (!variableNames.Any())
            {
                logger.Warn($"No variables found.");
                return Enumerable.Empty<invln_SecretVariable>();
            }

            var query = new QueryExpression(invln_SecretVariable.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(invln_SecretVariable.Fields.invln_Name, invln_SecretVariable.Fields.invln_Value),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression(invln_SecretVariable.PrimaryNameAttribute, ConditionOperator.In, variableNames)
                    }
                }
            };

            var result = RetrieveAll(query).Entities.Select(e => e.ToEntity<invln_SecretVariable>());
            foreach (var entity in result)
            {
                if (!entity.invln_Name.ToLower().Contains("secret"))
                logger.Trace($"{entity.invln_Name}: {entity.invln_Value}");
            }

            logger.Trace($"Return {result.Count()} elements.");
            return result;
        }

        #endregion Interface Implementation
    }
}
