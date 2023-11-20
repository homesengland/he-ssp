using System;
using System.Collections.Generic;
using System.Linq;
using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class AhpApplicationRepository : CrmEntityRepository<invln_scheme, DataverseContext>, IAhpApplicationRepository
    {
        public AhpApplicationRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        public bool ApplicationWithGivenIdExistsForOrganisationAndContract(Guid applicationId, Guid organisationId, string userId)
        {
            using (DataverseContext ctx = new DataverseContext(service))
            {
                return (from app in ctx.invln_schemeSet
                        join cnt in ctx.ContactSet on app.invln_contactid.Id equals cnt.ContactId
                        where app.invln_schemeId == applicationId && cnt.invln_externalid == userId && app.invln_organisationid.Id == organisationId
                        select app).AsEnumerable().Any();

            }
        }

        public bool ApplicationWithGivenNameExists(string name)
        {
            using(var ctx = new OrganizationServiceContext(service))
            {
                return ctx.CreateQuery<invln_scheme>()
                    .Where(x => x.invln_schemename == name).AsEnumerable().Any();
            }
        }

        public List<invln_scheme> GetApplicationsForOrganisationAndContact(string organisationId, string contactId, string attributes, string additionalRecordFilters)
        {
            var fetchXml = @"<fetch>
                              <entity name=""invln_scheme"">"
                                + attributes +
                              @"<filter>
                                  <condition attribute=""invln_organisationid"" operator=""eq"" value=""" + organisationId + @""" />"
                                    + additionalRecordFilters +
                                @"</filter>
                                    <link-entity name=""contact"" from=""contactid"" to=""invln_contactid"">
                                          <filter>
                                            <condition attribute=""invln_externalid"" operator=""eq"" value=""" + contactId + @""" />
                                          </filter>
                                        </link-entity>
                                </entity>
                            </fetch>";
            EntityCollection result = service.RetrieveMultiple(new FetchExpression(fetchXml));
            return result.Entities.Select(x => x.ToEntity<invln_scheme>()).ToList();
        }
    }
}
