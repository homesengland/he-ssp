using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace HE.Investments.Organisation.CrmRepository;
public class WebRoleRepository : IWebRoleRepository
{
    public List<Entity> GetContactWebrole(IOrganizationServiceAsync2 service, Guid contactId, string portalType)
    {
        var fetchXML = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
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
			                                <attribute name='invln_portalpermissionlevelid'/>
			                                <attribute name='invln_name'/>
			                                <link-entity name='invln_portal' from='invln_portalid' to='invln_portalid' link-type='inner' alias='ag'>
				                                <filter type='and'>
					                                <condition attribute='invln_portal' operator='eq' value='" + portalType + @"'/>
				                                </filter>
			                                </link-entity>
		                                </link-entity>
	                                </entity>
                                </fetch>";

        var result = service.RetrieveMultiple(new FetchExpression(fetchXML));
        return result.Entities.ToList();
    }

    public List<Entity> GetDefaultPortalRoles(IOrganizationServiceAsync2 service, string portalType)
    {
        var fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
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

        var result = service.RetrieveMultiple(new FetchExpression(fetchXml));
        return result.Entities.ToList();
    }
}
