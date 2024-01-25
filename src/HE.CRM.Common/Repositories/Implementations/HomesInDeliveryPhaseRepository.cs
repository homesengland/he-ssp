using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk.Query;

namespace HE.CRM.Common.Repositories.implementations
{
    public class HomesInDeliveryPhaseRepository : CrmEntityRepository<invln_homesindeliveryphase, DataverseContext>, IHomesInDeliveryPhaseRepository
    {
        public HomesInDeliveryPhaseRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        public List<invln_homesindeliveryphase> GetHomesInDeliveryPhase(Guid deliveryPhaseId)
        {
            var homesInDeliveryPhase = new List<invln_homesindeliveryphase>();
            var qe = new QueryExpression(invln_homesindeliveryphase.EntityLogicalName);
            qe.Criteria.AddCondition(invln_homesindeliveryphase.Fields.invln_deliveryphaselookup, ConditionOperator.Equal, deliveryPhaseId);
            qe.ColumnSet = new ColumnSet(invln_homesindeliveryphase.Fields.invln_name, invln_homesindeliveryphase.Fields.invln_hometypelookup, invln_homesindeliveryphase.Fields.invln_numberofhomes);

            var result = this.RetrieveAll(qe);
            if (result != null && result.Entities.Count > 0)
            {
                homesInDeliveryPhase.AddRange(result.Entities.Select(x => x.ToEntity<invln_homesindeliveryphase>()).ToList());
            }

            return homesInDeliveryPhase;
        }
    }
}
