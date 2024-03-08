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



namespace HE.CRM.Common.Repositories.Implementations
{
    public class FrontDoorProjectSiteRepository : CrmEntityRepository<invln_FrontDoorProjectSitePOC, DataverseContext>, IFrontDoorProjectSiteRepository
    {
        public FrontDoorProjectSiteRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        public PagedResponseDto<invln_FrontDoorProjectSitePOC> GetFrontDoorProjectSites(PagingRequestDto paging, string frontDoorProjectIdCondition, string attributes)
        {
            var fetchXML = $@"<fetch page=""{paging.pageNumber}"" count=""{paging.pageSize}"" returntotalrecordcount=""true"">
	                                <entity name='invln_frontdoorprojectsitepoc'>
                                            <attribute name=""ownerid"" />
                                              {attributes}
                                            <filter>
                                              <condition attribute=""statecode"" operator=""eq"" value=""0"" />
                                              {frontDoorProjectIdCondition}
                                            </filter>
                                          </entity>
                                        </fetch>";

            var result = service.RetrieveMultiple(new FetchExpression(fetchXML));

            return new PagedResponseDto<invln_FrontDoorProjectSitePOC>
            {
                paging = paging,
                items = result.Entities.Select(x => x.ToEntity<invln_FrontDoorProjectSitePOC>()).AsEnumerable().ToList(),
                totalItemsCount = result.TotalRecordCount,
            };
        }

        public invln_FrontDoorProjectSitePOC GetFrontDoorProjectSite(string frontDoorProjectIdCondition, string frontDoorProjecSitetIdCondition, string attributes)
        {
            var fetchXML = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
	                                <entity name='invln_frontdoorprojectsitepoc'>
                                            <attribute name=""ownerid"" />
                                            {attributes}
                                            <filter>
                                              <condition attribute=""statecode"" operator=""eq"" value=""0"" />
                                              {frontDoorProjectIdCondition}
                                              {frontDoorProjecSitetIdCondition}
                                            </filter>
                                          </entity>
                                        </fetch>";

            EntityCollection result = service.RetrieveMultiple(new FetchExpression(fetchXML));
            return result.Entities.Select(x => x.ToEntity<invln_FrontDoorProjectSitePOC>()).SingleOrDefault();
        }
    }
}
