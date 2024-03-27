using System.Collections.Generic;
using System.Linq;
using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk.Client;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.Helpers;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class LocalAuthorityRepository : CrmEntityRepository<invln_localauthority, DataverseContext>, ILocalAuthorityRepository
    {


        public LocalAuthorityRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        public List<invln_localauthority> GetAll()
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                return ctx.CreateQuery<invln_localauthority>().ToList();
            }
        }

        public invln_localauthority GetLocalAuthorityWithGivenOnsCode(string onsCode)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                return ctx.CreateQuery<invln_localauthority>()
                    .Where(x => x.invln_onscode == onsCode).FirstOrDefault();
            }
        }



        public PagedResponseDto<invln_localauthority> GetLocalAuthoritiesForLoan(PagingRequestDto pagingRequestDto, string searchPhrase)
        {
            var filter = string.IsNullOrWhiteSpace(searchPhrase)
                ? string.Empty
                : $@"<filter>
                        <condition attribute=""invln_localauthorityname"" operator=""like"" value=""%{searchPhrase}%"" />
                   </filter>";

            var fetchXml =
                $@"<fetch page=""{pagingRequestDto.pageNumber}"" count=""{pagingRequestDto.pageSize}"" returntotalrecordcount=""true"">
	                <entity name=""invln_localauthority"">
                        <attribute name=""invln_localauthorityid"" />
		                <attribute name=""invln_localauthorityname"" />
		                <attribute name=""invln_onscode"" />
                        <order attribute=""invln_localauthorityname""/>
                        {filter}
	                </entity>
                </fetch>";


            var result = service.RetrieveMultiple(new FetchExpression(fetchXml));

            return new PagedResponseDto<invln_localauthority>
            {
                paging = pagingRequestDto,
                items = result.Entities.Select(x => x.ToEntity<invln_localauthority>()).AsEnumerable().ToList(),
                totalItemsCount = result.TotalRecordCount,
            };
        }
        public PagedResponseDto<invln_AHGLocalAuthorities> GetLocalAuthoritiesForAHP(PagingRequestDto pagingRequestDto, string searchPhrase)
        {
            var filter = string.IsNullOrWhiteSpace(searchPhrase)
                ? string.Empty
                : $@"<filter>
                        <condition attribute=""invln_localauthorityname"" operator=""like"" value=""%{searchPhrase}%"" />
                   </filter>";

            var fetchXml =
                $@"<fetch page=""{pagingRequestDto.pageNumber}"" count=""{pagingRequestDto.pageSize}"" returntotalrecordcount=""true"">
	                <entity name=""invln_ahglocalauthorities"">
		                <attribute name=""invln_ahglocalauthoritiesid"" />
		                <attribute name=""invln_gsscode"" />
		                <attribute name=""invln_localauthorityname"" />
                        <order attribute=""invln_localauthorityname""/>
                        {filter}
	                </entity>
                </fetch>";

            var result = service.RetrieveMultiple(new FetchExpression(fetchXml));

            return new PagedResponseDto<invln_AHGLocalAuthorities>
            {
                paging = pagingRequestDto,
                items = result.Entities.Select(x => x.ToEntity<invln_AHGLocalAuthorities>()).AsEnumerable().ToList(),
                totalItemsCount = result.TotalRecordCount,
            };
        }
        public invln_localauthority GetLocalAuthorityrelatedToLoanApplication(Guid id)
        {
            var query = new QueryExpression(invln_localauthority.EntityLogicalName);
            query.ColumnSet.AllColumns = true;
            var query_invln_sitedetails = query.AddLink(invln_SiteDetails.EntityLogicalName, "invln_localauthorityid", "invln_localauthorityid");
            query_invln_sitedetails.LinkCriteria.AddCondition("invln_sitedetailsid", ConditionOperator.Equal, id);
            return service.RetrieveMultiple(query).Entities.Select(x => x.ToEntity<invln_localauthority>()).FirstOrDefault();
        }
    }
}
