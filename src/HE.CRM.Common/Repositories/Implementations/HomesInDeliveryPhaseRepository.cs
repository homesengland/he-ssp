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

        public List<invln_homesindeliveryphase> GetHomesInDeliveryPhaseForApplication(Guid applicationId, params string[] homesInDeliveryPhaseColumns)
        {
            var query = new QueryExpression(invln_homesindeliveryphase.EntityLogicalName)
            {
                ColumnSet = homesInDeliveryPhaseColumns != null && homesInDeliveryPhaseColumns.Any() ? new ColumnSet(homesInDeliveryPhaseColumns) : new ColumnSet(true),
                LinkEntities =
                {
                    // Add link-entity invln_deliveryphase
                    new LinkEntity()
                    {
                        LinkFromEntityName = invln_homesindeliveryphase.EntityLogicalName,
                        LinkFromAttributeName = invln_homesindeliveryphase.Fields.invln_deliveryphaselookup,
                        LinkToEntityName = invln_DeliveryPhase.EntityLogicalName,
                        LinkToAttributeName = invln_DeliveryPhase.PrimaryIdAttribute,
                        JoinOperator = JoinOperator.Inner,
                        EntityAlias = invln_DeliveryPhase.EntityLogicalName,
                        LinkCriteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(invln_DeliveryPhase.Fields.invln_Application, ConditionOperator.Equal, applicationId)
                            }
                        }
                    },
                    // Add link-entity invln_hometype
                    new LinkEntity()
                    {
                        LinkFromEntityName = invln_homesindeliveryphase.EntityLogicalName,
                        LinkFromAttributeName = invln_homesindeliveryphase.Fields.invln_hometypelookup,
                        LinkToEntityName = invln_HomeType.EntityLogicalName,
                        LinkToAttributeName = invln_HomeType.PrimaryIdAttribute,
                        JoinOperator = JoinOperator.Inner,
                        EntityAlias = invln_HomeType.EntityLogicalName
                    }
                }
            };

            return RetrieveAll(query).Entities
                .Select(e => e.ToEntity<invln_homesindeliveryphase>())
                .ToList();
        }
    }
}
