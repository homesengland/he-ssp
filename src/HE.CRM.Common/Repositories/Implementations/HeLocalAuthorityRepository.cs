using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Services;
using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using System.Reflection.Emit;


namespace HE.CRM.Common.Repositories.Implementations
{
    public class HeLocalAuthorityRepository : CrmEntityRepository<he_LocalAuthority, DataverseContext>, IHeLocalAuthorityRepository
    {
        public HeLocalAuthorityRepository(CrmRepositoryArgs args) : base(args)
        {
        }
        public he_LocalAuthority GetLocalAuthorityWithGivenCode(string code)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                return ctx.CreateQuery<he_LocalAuthority>()
                    .Where(x => x.he_GSSCode == code).FirstOrDefault();
            }
        }

        public PagedResponseDto<he_LocalAuthority> GetLocalAuthoritiesForFdLoan(PagingRequestDto pagingRequestDto, string searchPhrase)
        {
            var filter = string.IsNullOrWhiteSpace(searchPhrase)
            ? string.Empty
            : $@"<filter>
                        <condition attribute=""he_name"" operator=""like"" value=""%{searchPhrase}%"" />
                   </filter>";

            var fetchXml =
                $@"<fetch page=""{pagingRequestDto.pageNumber}"" count=""{pagingRequestDto.pageSize}"" returntotalrecordcount=""true"">
	                <entity name=""he_localauthority"">
		                <attribute name=""he_localauthorityid"" />
		                <attribute name=""he_gsscode"" />
		                <attribute name=""he_name"" />
                        <order attribute=""he_name""/>
                        {filter}
	                </entity>
                </fetch>";

            var result = service.RetrieveMultiple(new FetchExpression(fetchXml));

            return new PagedResponseDto<he_LocalAuthority>
            {
                paging = pagingRequestDto,
                items = result.Entities.Select(x => x.ToEntity<he_LocalAuthority>()).AsEnumerable().ToList(),
                totalItemsCount = result.TotalRecordCount,
            };
        }

        public he_LocalAuthority GetHeLocalAuthorityrelatedToLoanApplication(Guid siteDetailsId)
        {
            var query = new QueryExpression(he_LocalAuthority.EntityLogicalName);
            query.ColumnSet.AllColumns = true;
            var query_invln_sitedetails = query.AddLink(invln_SiteDetails.EntityLogicalName, "he_localauthorityid", "invln_helocalauthorityid");
            query_invln_sitedetails.LinkCriteria.AddCondition("invln_sitedetailsid", ConditionOperator.Equal, siteDetailsId);
            return service.RetrieveMultiple(query).Entities.Select(x => x.ToEntity<he_LocalAuthority>()).FirstOrDefault();
        }

        public he_LocalAuthority GetAhpLocalAuthoritiesReletedToSite(Guid siteId)
        {
            var query = new QueryExpression
            {
                EntityName = invln_Sites.EntityLogicalName,
                ColumnSet = new ColumnSet(invln_Sites.Fields.Id, invln_Sites.Fields.invln_HeLocalAuthorityId)
            };
            query.Criteria.AddCondition(invln_Sites.Fields.Id, ConditionOperator.Equal, siteId);
            var site = service.RetrieveMultiple(query).Entities.Select(x => x.ToEntity<invln_Sites>()).FirstOrDefault();
            if (site.invln_HeLocalAuthorityId == null)
            {
                return null;
            }

            var queryLA = new QueryExpression
            {
                EntityName = he_LocalAuthority.EntityLogicalName,
                ColumnSet = new ColumnSet(he_LocalAuthority.Fields.Id, he_LocalAuthority.Fields.OwningUser)
            };
            queryLA.Criteria.AddCondition(invln_AHGLocalAuthorities.Fields.Id, ConditionOperator.Equal, site.invln_LocalAuthority.Id);
            return service.RetrieveMultiple(queryLA).Entities.Select(x => x.ToEntity<he_LocalAuthority>()).FirstOrDefault();
        }

    }
}
