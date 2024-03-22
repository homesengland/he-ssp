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
    public class HeProjectLocalAuthorityRepository : CrmEntityRepository<he_ProjectLocalAuthority, DataverseContext>, IHeProjectLocalAuthorityRepository
    {
        public HeProjectLocalAuthorityRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        public PagedResponseDto<he_ProjectLocalAuthority> HeGetFrontDoorProjectSites(PagingRequestDto paging, string frontDoorProjectIdCondition)
        {
            var fetchXML = $@"<fetch page=""{paging.pageNumber}"" count=""{paging.pageSize}"" returntotalrecordcount=""true"">
	                                <entity name='he_projectlocalauthority'>
		                                    <attribute name=""he_projectlocalauthorityid"" />
		                                    <attribute name=""createdon"" />
		                                    <attribute name=""he_homes"" />
		                                    <attribute name=""he_localauthority"" />
		                                    <attribute name=""he_localauthorityname"" />
		                                    <attribute name=""he_name"" />
		                                    <attribute name=""he_project"" />
		                                    <attribute name=""he_projectname"" />
		                                    <attribute name=""he_planningstatusofthesite"" />
		                                    <attribute name=""he_planningstatusofthesitename"" />
                                            <filter>
                                              <condition attribute=""statecode"" operator=""eq"" value=""0"" />
                                              {frontDoorProjectIdCondition}
                                            </filter>
                                          </entity>
                                        </fetch>";

            var result = service.RetrieveMultiple(new FetchExpression(fetchXML));

            return new PagedResponseDto<he_ProjectLocalAuthority>
            {
                paging = paging,
                items = result.Entities.Select(x => x.ToEntity<he_ProjectLocalAuthority>()).AsEnumerable().ToList(),
                totalItemsCount = result.TotalRecordCount,
            };
        }

        public he_ProjectLocalAuthority HeGetFrontDoorProjectSite(string frontDoorProjectIdCondition, string frontDoorProjecSitetIdCondition)
        {
            var fetchXML = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
	                                <entity name='he_projectlocalauthority'>
                                            <attribute name=""he_projectlocalauthorityid"" />
		                                    <attribute name=""createdon"" />
		                                    <attribute name=""he_homes"" />
		                                    <attribute name=""he_localauthority"" />
		                                    <attribute name=""he_localauthorityname"" />
		                                    <attribute name=""he_name"" />
		                                    <attribute name=""he_project"" />
		                                    <attribute name=""he_projectname"" />
		                                    <attribute name=""he_planningstatusofthesite"" />
		                                    <attribute name=""he_planningstatusofthesitename"" />
                                            <filter>
                                              <condition attribute=""statecode"" operator=""eq"" value=""0"" />
                                              {frontDoorProjectIdCondition}
                                              {frontDoorProjecSitetIdCondition}
                                            </filter>
                                          </entity>
                                        </fetch>";

            EntityCollection result = service.RetrieveMultiple(new FetchExpression(fetchXML));
            return result.Entities.Select(x => x.ToEntity<he_ProjectLocalAuthority>()).SingleOrDefault();
        }

    }
}
