using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HE.Base.Repositories;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using HE.CRM.Common.Repositories.Interfaces;
using DataverseModel;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class TeamRepository : CrmEntityRepository<Team, DataverseContext>, ITeamRepository
    {
        public TeamRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        public Team GetTeamByName(string teamName)
        {
            var qe = new QueryExpression(Team.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(nameof(Team.Name).ToLower())
            };

            qe.Criteria.AddCondition(nameof(Team.Name).ToLower(), ConditionOperator.Equal, teamName);
            var result = this.service.RetrieveMultiple(qe);
            if (result.Entities.Count > 0)
            {
                return result.Entities[0].ToEntity<Team>();
            }

            return null;
        }
    }
}
