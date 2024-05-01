using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace HE.Investments.Organisation.CrmRepository;
public class WebRoleRepository : IWebRoleRepository
{
    public List<Entity> GetContactWebRole(IOrganizationServiceAsync2 service, string contactId, string portalTypeFilter)
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
			                               <link-entity name=""invln_portal"" from=""invln_portalid"" to=""invln_portalid"">"
                                            + portalTypeFilter +
                                          @"</link-entity>
		                                </link-entity>
	                                </entity>
                                </fetch>";

        var result = service.RetrieveMultiple(new FetchExpression(fetchXML));
        return result.Entities.ToList();
    }

    public List<Entity> GetDefaultPortalRoles(IOrganizationServiceAsync2 service, int portalType)
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

    public Entity? GetContactWebRoleForGivenOrganisationAndPortal(IOrganizationServiceAsync2 service, string organisationId, string contactId, string? portalTypeFiler = null)
    {
        var fetchXml = @"<fetch>
                      <entity name=""invln_contactwebrole"">
                        <attribute name=""invln_contactwebroleid"" />
                        <filter>
                          <condition attribute=""invln_contactid"" operator=""eq"" value=""" + contactId + @""" />
                          <condition attribute=""invln_accountid"" operator=""eq"" value=""" + organisationId + @""" />
                        </filter>
                        <link-entity name=""invln_webrole"" from=""invln_webroleid"" to=""invln_webroleid"">
                          <link-entity name=""invln_portal"" from=""invln_portalid"" to=""invln_portalid"">"
                            + portalTypeFiler +
                          @"</link-entity>
                        </link-entity>
                      </entity>
                    </fetch>";

        var result = service.RetrieveMultiple(new FetchExpression(fetchXml));
        return result.Entities.FirstOrDefault();
    }

    public Entity? GetContactWebRoleForOrganisation(IOrganizationServiceAsync2 service, string contactId, string organisationId)
    {
        var condition1 = new ConditionExpression("invln_accountid", ConditionOperator.Equal, organisationId);
        var condition2 = new ConditionExpression("invln_contactid", ConditionOperator.Equal, contactId);
        var filter1 = new FilterExpression()
        {
            Conditions =
                {
                    condition1,
                    condition2,
                },
            FilterOperator = LogicalOperator.And,
        };
        var cols = new ColumnSet(true);

        var query = new QueryExpression("invln_contactwebrole")
        {
            ColumnSet = cols,
        };
        query.Criteria.FilterOperator = LogicalOperator.And;
        query.Criteria.AddFilter(filter1);

        var result1 = service.RetrieveMultiple(query);
        return result1.Entities.FirstOrDefault();
    }

    public Entity? GetWebRoleByName(IOrganizationServiceAsync2 service, string webRoleName)
    {
        var condition1 = new ConditionExpression("invln_name", ConditionOperator.Equal, webRoleName);
        var filter1 = new FilterExpression()
        {
            Conditions =
                {
                    condition1,
                },
            FilterOperator = LogicalOperator.Or,
        };
        var cols = new ColumnSet(true);

        var query = new QueryExpression("invln_webrole")
        {
            ColumnSet = cols,
        };
        query.Criteria.FilterOperator = LogicalOperator.And;
        query.Criteria.AddFilter(filter1);

        var result1 = service.RetrieveMultiple(query);
        return result1.Entities.FirstOrDefault();
    }

    public Entity? GetWebRoleByPermissionOptionSetValue(IOrganizationServiceAsync2 service, int permission, string portalTypeFilter)
    {
        var fetchXml = @"<fetch>
                          <entity name=""invln_webrole"">
                            <attribute name=""invln_name"" />
                            <attribute name=""invln_webroleid"" />
                            <link-entity name=""invln_portalpermissionlevel"" from=""invln_portalpermissionlevelid"" to=""invln_portalpermissionlevelid"">
                              <filter>
                                <condition attribute=""invln_permission"" operator=""eq"" value=""" + permission + @""" />
                              </filter>
                            </link-entity>
                            <link-entity name=""invln_portal"" from=""invln_portalid"" to=""invln_portalid"">"
                            + portalTypeFilter +
                          @"</link-entity>
                          </entity>
                        </fetch>";

        var result = service.RetrieveMultiple(new FetchExpression(fetchXml));
        return result.Entities.FirstOrDefault();
    }

    public List<Entity> GetWebRolesForPassedContacts(IOrganizationServiceAsync2 service, string contactExternalIds, string organisationId)
    {
        var fetchXml = @"<fetch>
                          <entity name=""invln_contactwebrole"">
                            <attribute name=""invln_accountid"" />
                            <attribute name=""invln_contactid"" />
                            <attribute name=""invln_contactwebroleid"" />
                            <attribute name=""invln_webroleid"" />
                            <filter>
                                  <condition attribute=""invln_accountid"" operator=""eq"" value=""" + organisationId + @""" />
                                </filter>
                            <link-entity name=""contact"" from=""contactid"" to=""invln_contactid"" alias=""cnt"">
                                <attribute name=""invln_externalid"" />
                              <filter type=""or"">"
                                + contactExternalIds +
                              @"</filter>
                            </link-entity>
                            <link-entity name=""invln_webrole"" from=""invln_webroleid"" to=""invln_webroleid"" alias=""wr"">
                              <attribute name=""invln_name"" />
                              <attribute name=""invln_portalid"" />
                              <link-entity name=""invln_portalpermissionlevel"" from=""invln_portalpermissionlevelid"" to=""invln_portalpermissionlevelid"" alias=""ppl"">
                                <attribute name=""invln_name"" />
                                <attribute name=""invln_permission"" />
                                <attribute name=""invln_portalpermissionlevelid"" />
                              </link-entity>
                            </link-entity>
                            <link-entity name=""account"" from=""accountid"" to=""invln_accountid"" alias=""acc"">
                                  <attribute name=""name"" />
                                </link-entity>
                          </entity>
                        </fetch>";

        var result = service.RetrieveMultiple(new FetchExpression(fetchXml));
        return result.Entities.ToList();
    }
}
