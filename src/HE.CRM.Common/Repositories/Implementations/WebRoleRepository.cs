using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class WebRoleRepository : CrmEntityRepository<invln_Webrole, DataverseContext>, IWebRoleRepository
    {
        public WebRoleRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        public List<invln_contactwebrole> GetContactWebRole(Guid contactId, string portalType)
        {
            string fetchXML = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
	                                <entity name='invln_contactwebrole'>
		                                <attribute name='invln_contactwebroleid'/>
		                                <attribute name='invln_name'/>
		                                <attribute name='createdon'/>
		                                <attribute name='invln_webroleid'/>
		                                <attribute name='invln_accountid'/>
		                                <attribute name='statuscode'/>
		                                <attribute name='statecode'/>
		                                <order attribute='invln_name' descending='false'/>
		                                <filter type='and'>
			                                <condition attribute='invln_contactid' operator='eq' uitype='contact' value='" + contactId + @"'/>
		                                </filter>
		                                <link-entity name='invln_webrole' from='invln_webroleid' to='invln_webroleid' link-type='inner' alias='ae'>
			                                <attribute name='invln_name'/>
                                             <link-entity name=""invln_portalpermissionlevel"" from=""invln_portalpermissionlevelid"" to=""invln_portalpermissionlevelid"" alias=""pl"">
                                                    <attribute name=""invln_name"" />
                                                    <attribute name=""invln_permission"" />
                                                  </link-entity>
			                                <link-entity name='invln_portal' from='invln_portalid' to='invln_portalid' link-type='inner' alias='ag'>
				                                <filter type='and'>
					                                <condition attribute='invln_portal' operator='eq' value='" + portalType + @"'/>
				                                </filter>
			                                </link-entity>
		                                </link-entity>
	                                </entity>
                                </fetch>";

            EntityCollection result = service.RetrieveMultiple(new FetchExpression(fetchXML));
            return result.Entities.Select(x => x.ToEntity<invln_contactwebrole>()).ToList();
        }

        public List<invln_Webrole> GetDefaultPortalRoles(string portalType)
        {
			string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
									<entity name='invln_webrole'>
										<attribute name='invln_webroleid'/>
										<attribute name='invln_portalpermissionlevelid'/>
										<attribute name='invln_name'/>
										<attribute name='createdon'/>
										<order attribute='invln_name' descending='false'/>
										<filter type='and'>
											<condition attribute='invln_isdefaultrole' operator='eq' value='1'/>
										</filter>
										<link-entity name='invln_portal' from='invln_portalid' to='invln_portalid' link-type='inner' alias='ad'>
											<filter type='and'>
												<condition attribute='invln_portal' operator='eq' value='" + portalType + @"'/>
											</filter>
										</link-entity>
									</entity>
								</fetch>";

            EntityCollection result = service.RetrieveMultiple(new FetchExpression(fetchXml));
            return result.Entities.Select(x => x.ToEntity<invln_Webrole>()).ToList();
        }

        public invln_Webrole GetRoleByName(string name)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                return ctx.CreateQuery<invln_Webrole>()
                    .Where(x => x.invln_Name == name).AsEnumerable().FirstOrDefault();
            }
        }
    }
}
