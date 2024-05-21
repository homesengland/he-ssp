using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using DataverseModel;
using HE.Base.Repositories;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.Helpers;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class SiteRepository : CrmEntityRepository<invln_Sites, DataverseContext>, ISiteRepository
    {
        public SiteRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        public invln_Sites GetSingle(string siteIdFilter, string fieldsToRetrieve, string externalContactIdFilter, string accountIdFilter)
        {
            logger.Trace("SiteRepository GetSingle");
            string fieldsToRetrieveParameters = string.Empty;
            if (!string.IsNullOrEmpty(fieldsToRetrieve))
            {
                fieldsToRetrieveParameters = FetchXmlHelper.GenerateAttributes(fieldsToRetrieve);
            }

            var fetchXml =
                $@"<fetch>
	                <entity name=""invln_sites"">
                        {fieldsToRetrieveParameters}
                        {accountIdFilter}
                        {siteIdFilter}
		                <link-entity name=""contact"" from=""contactid"" to=""invln_createdbycontactid"">
			                {externalContactIdFilter}
		                </link-entity>
	                </entity>
                </fetch>";

            //var result = service.RetrieveMultiple(new FetchExpression(fetchXml));

            EntityCollection result = service.RetrieveMultiple(new FetchExpression(fetchXml));
            return result.Entities.Select(x => x.ToEntity<invln_Sites>()).SingleOrDefault();
        }

        public bool Exist(string name)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                return ctx.CreateQuery<invln_Sites>()
                    .Where(x => x.invln_sitename == name)
                    .AsEnumerable()
                    .Any();
            }
        }

        public bool StrategicSiteNameExists(string strategicSiteName, Guid organisationGuid)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                return ctx.CreateQuery<invln_Sites>()
                    .Where(x => x.invln_StrategicSiteN == strategicSiteName && x.invln_AccountId.Id == organisationGuid)
                    .AsEnumerable()
                    .Any();
            }
        }

        public PagedResponseDto<invln_Sites> GetMultiple(PagingRequestDto paging, string fieldsToRetrieve, string externalContactIdFilter, string accountIdFilter)
        {
            logger.Trace("SiteRepository GetMultiple");
            string fieldsToRetrieveParameters = string.Empty;
            if (!string.IsNullOrEmpty(fieldsToRetrieve))
            {
                fieldsToRetrieveParameters = FetchXmlHelper.GenerateAttributes(fieldsToRetrieve);
            }

            var fetchXml =
                $@"<fetch page=""{paging.pageNumber}"" count=""{paging.pageSize}"" returntotalrecordcount=""true"">
	                <entity name=""invln_sites"">
                        {fieldsToRetrieveParameters}
                        <order attribute=""createdon"" descending=""true"" />
                        {accountIdFilter}
		                <link-entity name=""contact"" from=""contactid"" to=""invln_createdbycontactid"">
			                {externalContactIdFilter}
		                </link-entity>
	                </entity>
                </fetch>";

            var result = service.RetrieveMultiple(new FetchExpression(fetchXml));

            return new PagedResponseDto<invln_Sites>
            {
                paging = paging,
                items = result.Entities.Select(x => x.ToEntity<invln_Sites>()).AsEnumerable().ToList(),
                totalItemsCount = result.TotalRecordCount,
            };
        }

        public List<invln_Sites> GetbyConsortiumId(Guid consortiumId)
        {
            var query = new QueryExpression(invln_Sites.EntityLogicalName);
            query.ColumnSet = new ColumnSet(invln_Sites.Fields.invln_developingpartner);
            var query_invln_ahpproject = query.AddLink(
                invln_ahpproject.EntityLogicalName,
                invln_Sites.Fields.invln_AHPProjectId,
                invln_ahpproject.Fields.invln_ahpprojectId);

            query_invln_ahpproject.LinkCriteria.AddCondition(invln_ahpproject.Fields.invln_ConsortiumId, ConditionOperator.Equal, consortiumId);
            return service.RetrieveMultiple(query).Entities.Select(x => x.ToEntity<invln_Sites>()).ToList();
        }

    }
}
