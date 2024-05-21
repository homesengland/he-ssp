using System;
using System.Collections.Generic;
using System.Linq;
using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class AhpApplicationRepository : CrmEntityRepository<invln_scheme, DataverseContext>, IAhpApplicationRepository
    {
        public AhpApplicationRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        public bool ApplicationWithGivenIdExistsForOrganisation(Guid applicationId, Guid organisationId)
        {
            using (DataverseContext ctx = new DataverseContext(service))
            {
                return (from app in ctx.invln_schemeSet
                        where app.invln_schemeId == applicationId && app.invln_organisationid.Id == organisationId
                        select app).AsEnumerable().Any();

            }
        }

        public bool ApplicationWithGivenNameAndOrganisationExists(string name, Guid organisationId)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                return ctx.CreateQuery<invln_scheme>()
                    .Where(x => x.invln_schemename == name && x.invln_organisationid.Id == organisationId).AsEnumerable().Any();
            }
        }

        public bool ApplicationWithGivenNameExists(string name)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                return ctx.CreateQuery<invln_scheme>()
                    .Where(x => x.invln_schemename == name).AsEnumerable().Any();
            }
        }

        public List<invln_scheme> GetApplicationsForOrganisationAndContact(string organisationId, string contactFilter, string attributes, string additionalRecordFilters)
        {
            var fetchXml = $@"<fetch>
                              <entity name=""invln_scheme"">
                                <attribute name=""invln_contactid""/>
                                {attributes}
                                <filter>
                                  <condition attribute= ""invln_organisationid"" operator=""eq"" value = ""{organisationId}"" />
                                  <condition attribute = ""invln_externalstatus"" operator= ""ne"" value = ""{(int)invln_ExternalStatusAHP.Deleted}"" />
                                  {additionalRecordFilters}
                                </filter>
                                    <link-entity name=""contact"" from=""contactid"" to=""invln_contactid"">
                                       {contactFilter}
                                    </link-entity>
                                </entity>
                            </fetch>";
            EntityCollection result = service.RetrieveMultiple(new FetchExpression(fetchXml));
            return result.Entities.Select(x => x.ToEntity<invln_scheme>()).ToList();
        }

        public List<invln_scheme> GetApplicationsForAhpProject(Guid ahpProjectGuid, invln_Permission contactWebRole, Contact contact, Guid organisationGuid, string consortiumId = null)
        {
            var query_invln_sites_invln_ahpprojectid = ahpProjectGuid.ToString();

            var query = new QueryExpression(invln_scheme.EntityLogicalName);
            query.Distinct = true;
            query.ColumnSet.AddColumns(
                invln_scheme.Fields.invln_applicationid,
                invln_scheme.Fields.invln_schemename,
                invln_scheme.Fields.invln_ExternalStatus,
                invln_scheme.Fields.invln_Tenure,
                invln_scheme.Fields.invln_lastexternalmodificationon,
                invln_scheme.Fields.invln_fundingrequired,
                invln_scheme.Fields.invln_noofhomes
                );

            var query_invln_sites = query.AddLink(
                invln_Sites.EntityLogicalName,
                invln_scheme.Fields.invln_Site,
                invln_Sites.Fields.invln_SitesId);

            query_invln_sites.LinkCriteria.AddCondition(invln_Sites.Fields.invln_AHPProjectId, ConditionOperator.Equal, query_invln_sites_invln_ahpprojectid);

            if (contactWebRole == invln_Permission.Limiteduser)
            {
                var query_invln_contactid = contact.Id.ToString();
                query.Criteria.AddCondition(invln_scheme.Fields.invln_contactid, ConditionOperator.Equal, query_invln_contactid);
            }

            if (contactWebRole != invln_Permission.Limiteduser)
            {
                var query_invln_organisationid = organisationGuid.ToString();
                query.Criteria.AddCondition(invln_scheme.Fields.invln_organisationid, ConditionOperator.Equal, query_invln_organisationid);

            }

            return service.RetrieveMultiple(query).Entities.Select(x => x.ToEntity<invln_scheme>()).ToList();
        }

        public List<invln_scheme> GetByConsortiumId(Guid consortiumId)
        {
            var query = new QueryExpression(invln_scheme.EntityLogicalName);
            query.ColumnSet = new ColumnSet(invln_scheme.Fields.invln_DevelopingPartner,
                invln_scheme.Fields.invln_OwneroftheHomes,
                invln_scheme.Fields.invln_OwneroftheLand);
            var query_invln_sites = query.AddLink(
                invln_Sites.EntityLogicalName,
                invln_scheme.Fields.invln_Site,
                invln_Sites.Fields.invln_SitesId);
            var query_invln_sites_invln_ahpproject = query_invln_sites.AddLink(
                invln_ahpproject.EntityLogicalName,
                invln_Sites.Fields.invln_AHPProjectId,
                invln_ahpproject.Fields.invln_ahpprojectId);

            query_invln_sites_invln_ahpproject.LinkCriteria.AddCondition(invln_ahpproject.Fields.invln_ConsortiumId, ConditionOperator.Equal, consortiumId);

            return service.RetrieveMultiple(query).Entities.Select(x => x.ToEntity<invln_scheme>()).ToList();
        }
    }
}
