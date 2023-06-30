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

        public invln_Webrole GetContactRole(Guid contactId, Guid portalId)
        {
            string fetchXML = @"<fetch>
                                  <entity name=""invln_webrole"">
                                    <attribute name=""invln_name"" />
                                    <link-entity name=""invln_portal"" from=""invln_portalid"" to=""invln_portalid"">
                                      <filter>
                                        <condition attribute=""invln_portalid"" operator=""eq"" value="""+ portalId + @""" />
                                      </filter>
                                    </link-entity>
                                    <link-entity name=""invln_contactwebrole"" from=""invln_webroleid"" to=""invln_webroleid"">
                                      <link-entity name=""contact"" from=""contactid"" to=""invln_contactid"">
                                        <filter>
                                          <condition attribute=""contactid"" operator=""eq"" value="""+ contactId + @""" />
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