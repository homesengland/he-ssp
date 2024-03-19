using System;
using System.Linq;
using DataverseModel;
using HE.Base.Repositories;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.Helpers;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class AhgLocalAuthorityRepository : CrmEntityRepository<invln_AHGLocalAuthorities, DataverseContext>, IAhgLocalAuthorityRepository
    {
        public AhgLocalAuthorityRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        public PagedResponseDto<invln_AHGLocalAuthorities> Get(PagingRequestDto paging, string searchPhrase, string fieldsToRetrieve)
        {
            var filter = string.IsNullOrWhiteSpace(searchPhrase)
                ? string.Empty
                : $@"<filter>
                        <condition attribute=""invln_localauthorityname"" operator=""like"" value=""%{searchPhrase}%"" />
                   </filter>";

            var fetchXml =
                $@"<fetch page=""{paging.pageNumber}"" count=""{paging.pageSize}"" returntotalrecordcount=""true"">
	                <entity name=""invln_ahglocalauthorities"">
                        {FetchXmlHelper.GenerateAttributes(fieldsToRetrieve)}
                        <order attribute=""invln_localauthorityname""/>
                        {filter}
	                </entity>
                </fetch>";

            var result = service.RetrieveMultiple(new FetchExpression(fetchXml));

            return new PagedResponseDto<invln_AHGLocalAuthorities>
            {
                paging = paging,
                items = result.Entities.Select(x => x.ToEntity<invln_AHGLocalAuthorities>()).AsEnumerable().ToList(),
                totalItemsCount = result.TotalRecordCount,
            };
        }

        public invln_AHGLocalAuthorities GetLocalAuthorityWithGivenCode(string code)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                return ctx.CreateQuery<invln_AHGLocalAuthorities>().FirstOrDefault(x => x.invln_GSSCode == code);
            }
        }

        public invln_AHGLocalAuthorities GetAhpLocalAuthoritiesReletedToSite(Guid siteId)
        {
            var query = new QueryExpression
            {
                EntityName = invln_Sites.EntityLogicalName,
                ColumnSet = new ColumnSet(invln_Sites.Fields.Id, invln_Sites.Fields.invln_LocalAuthority)
            };
            query.Criteria.AddCondition(invln_Sites.Fields.Id, ConditionOperator.Equal, siteId);
            var site = service.RetrieveMultiple(query).Entities.Select(x => x.ToEntity<invln_Sites>()).FirstOrDefault();

            var queryLA = new QueryExpression
            {
                EntityName = invln_AHGLocalAuthorities.EntityLogicalName,
                ColumnSet = new ColumnSet(invln_AHGLocalAuthorities.Fields.Id, invln_AHGLocalAuthorities.Fields.invln_GrowthManager)
            };
            query.Criteria.AddCondition(invln_AHGLocalAuthorities.Fields.Id, ConditionOperator.Equal, site.invln_LocalAuthority.Id);
            return service.RetrieveMultiple(queryLA).Entities.Select(x => x.ToEntity<invln_AHGLocalAuthorities>()).FirstOrDefault();
        }
    }
}
