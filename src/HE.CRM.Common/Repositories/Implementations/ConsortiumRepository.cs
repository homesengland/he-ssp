using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System;
using Microsoft.Xrm.Sdk.Client;
using System.Linq;
using System.Collections.Generic;
using System.IdentityModel.Metadata;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class ConsortiumRepository : CrmEntityRepository<invln_Consortium, DataverseContext>, IConsortiumRepository
    {
        public ConsortiumRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        public bool GetByProgrammeAndOrganization(Guid programmeId, Guid organizationId)
        {
            var query_1 = new QueryExpression(invln_Consortium.EntityLogicalName);
            query_1.ColumnSet = new ColumnSet(invln_Consortium.Fields.Id);
            query_1.Criteria.AddCondition(invln_Consortium.Fields.invln_Programme, ConditionOperator.Equal, programmeId);
            query_1.Criteria.AddCondition(invln_Consortium.Fields.invln_LeadPartner, ConditionOperator.Equal, organizationId);
            var response_1 = service.RetrieveMultiple(query_1);

            var query_2 = new QueryExpression(invln_Consortium.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(invln_Consortium.Fields.Id)
            };
            query_2.Criteria.AddCondition(invln_Consortium.Fields.invln_Programme, ConditionOperator.Equal, programmeId);
            var query_invln_consortiummember = query_2.AddLink(invln_ConsortiumMember.EntityLogicalName, invln_Consortium.Fields.invln_ConsortiumId, invln_ConsortiumMember.Fields.invln_Consortium);
            query_invln_consortiummember.LinkCriteria.AddCondition(invln_ConsortiumMember.Fields.invln_Partner, ConditionOperator.Equal, organizationId);
            query_invln_consortiummember.LinkCriteria.AddCondition(invln_ConsortiumMember.Fields.StatusCode, ConditionOperator.NotEqual, (int)invln_ConsortiumMember_StatusCode.Removalconfirmed);
            var response_2 = service.RetrieveMultiple(query_2);
            return response_1.Entities.Count > 0 || response_2.Entities.Count > 0;
        }

        public List<invln_Consortium> GetByLeadPartnerInConsoriumMember(string organisationId)
        {
            var query = new QueryExpression(invln_Consortium.EntityLogicalName);
            query.ColumnSet = new ColumnSet(invln_Consortium.Fields.Id, invln_Consortium.Fields.invln_Programme,
                                                invln_Consortium.Fields.invln_LeadPartner, invln_Consortium.Fields.StatusCode, invln_Consortium.Fields.invln_Name);
            var query_invln_consortiummember = query.AddLink(invln_ConsortiumMember.EntityLogicalName, invln_Consortium.Fields.invln_ConsortiumId, invln_ConsortiumMember.Fields.invln_Consortium);

            query_invln_consortiummember.LinkCriteria.AddCondition(invln_ConsortiumMember.Fields.invln_Partner, ConditionOperator.Equal, organisationId);
            query_invln_consortiummember.LinkCriteria.AddCondition(invln_ConsortiumMember.Fields.StatusCode, ConditionOperator.NotEqual, (int)invln_ConsortiumMember_StatusCode.Removalconfirmed);
            return service.RetrieveMultiple(query).Entities.Select(x => x.ToEntity<invln_Consortium>()).ToList();
        }

        public EntityCollection GetConsortiumIdForAhpApplication(Guid ahpApplicationId)
        {
            var query_invln_schemeid = ahpApplicationId.ToString();

            var query = new QueryExpression(invln_scheme.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(invln_scheme.Fields.invln_schemeId),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(invln_scheme.Fields.invln_schemeId, ConditionOperator.Equal, query_invln_schemeid)
                    }
                },
                LinkEntities =
                {
                    new LinkEntity(
                        invln_scheme.EntityLogicalName,
                        invln_Sites.EntityLogicalName,
                        invln_scheme.Fields.invln_Site,
                        invln_Sites.Fields.invln_SitesId,
                        JoinOperator.Inner)
                    {
                        EntityAlias = "Site",
                        Columns = new ColumnSet(invln_Sites.Fields.invln_SitesId),
                        LinkEntities =
                        {
                            new LinkEntity(
                                invln_Sites.EntityLogicalName,
                                invln_ahpproject.EntityLogicalName,
                                invln_Sites.Fields.invln_AHPProjectId,
                                invln_ahpproject.Fields.invln_ahpprojectId,
                                JoinOperator.Inner)
                            {
                                EntityAlias = "AhpProject",
                                Columns = new ColumnSet(invln_ahpproject.Fields.invln_ConsortiumId)
                            }
                        }
                    }
                }
            };
            return service.RetrieveMultiple(query);
        }

        public EntityCollection GetConsortiumIdForAhpSite(Guid siteId)
        {
            var query_invln_sitesid = siteId.ToString();

            var query = new QueryExpression(invln_Sites.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(invln_Sites.Fields.invln_SitesId),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(invln_Sites.Fields.invln_SitesId, ConditionOperator.Equal, query_invln_sitesid)
                    }
                },
                LinkEntities =
                {
                    new LinkEntity(
                        invln_Sites.EntityLogicalName,
                        invln_ahpproject.EntityLogicalName,
                        invln_Sites.Fields.invln_AHPProjectId,
                        invln_ahpproject.Fields.invln_ahpprojectId,
                        JoinOperator.Inner)
                    {
                        EntityAlias = "AhpProject",
                        Columns = new ColumnSet(invln_ahpproject.Fields.invln_ConsortiumId)
                    }
                }
            };
            return service.RetrieveMultiple(query);
        }

    }
}
