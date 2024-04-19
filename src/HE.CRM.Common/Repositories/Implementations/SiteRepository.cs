using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using DataverseModel;
using HE.Base.Repositories;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.Helpers;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class SiteRepository : CrmEntityRepository<invln_Sites, DataverseContext>, ISiteRepository
    {
        public SiteRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        public invln_Sites GetById(string id, string fieldsToRetrieve)
        {
            var result = Get(fieldsToRetrieve, new PagingRequestDto { pageNumber = 1, pageSize = 1 }, id);
            return result.items.FirstOrDefault();
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
                        <link-entity name=""invln_ahglocalauthorities"" to=""invln_localauthority"" from=""invln_ahglocalauthoritiesid""  link-type=""outer"">
                            <attribute name=""invln_gsscode"" />
                            <attribute name=""invln_localauthorityname"" />
                        </link-entity>
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

        private PagedResponseDto<invln_Sites> Get(string fieldsToRetrieve, PagingRequestDto paging, string id = null)
        {
            var filter = GenerateIdFilter(id);

            var fetchXml =
                $@"<fetch page=""{paging.pageNumber}"" count=""{paging.pageSize}"" returntotalrecordcount=""true"">
	                <entity name=""invln_sites"">
                        {FetchXmlHelper.GenerateAttributes(fieldsToRetrieve)}
                        <order attribute=""createdon"" descending=""true"" />
                        {filter}
                        <link-entity name=""invln_ahglocalauthorities"" to=""invln_localauthority"" from=""invln_ahglocalauthoritiesid""  link-type=""outer"">
                            <attribute name=""invln_gsscode"" />
                            <attribute name=""invln_localauthorityname"" />
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

        private static string GenerateIdFilter(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return string.Empty;
            }

            return @"<filter type=""and"">
			                <condition attribute=""invln_sitesid"" operator=""eq"" value=""" + id + @""" />
		                </filter>";
        }
    }
}
