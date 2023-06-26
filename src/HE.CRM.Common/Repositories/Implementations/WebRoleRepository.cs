using System;
using System.Linq;
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

        public invln_Webrole GetContactRole(Guid contactId, string portalName)
        {
            string fetchXML = @"<fetch>
                                  <entity name=""invln_webrole"">
                                    <attribute name=""invln_name"" />
                                    <attribute name=""invln_permissionlevel"" />
                                    <filter>
                                      <condition attribute=""invln_portalname"" operator=""eq"" value=""" + portalName + @""" />
                                    </filter>
                                    <link-entity name=""invln_contact_webrole"" from=""invln_webroleid"" to=""invln_webroleid"" intersect=""true"">
                                      <link-entity name=""contact"" from=""contactid"" to=""contactid"" intersect=""true"">
                                        <filter>
                                          <condition attribute=""contactid"" operator=""eq"" value=""" + contactId + @""" />
                                        </filter>
                                      </link-entity>
                                    </link-entity>
                                  </entity>
                                </fetch>";

            EntityCollection result = service.RetrieveMultiple(new FetchExpression(fetchXML));
            return result.Entities.Select(x => x.ToEntity<invln_Webrole>()).AsEnumerable().FirstOrDefault();
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