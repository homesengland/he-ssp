using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class HomeTypeRepository : CrmEntityRepository<invln_HomeType, DataverseContext>, IHomeTypeRepository
    {
        public HomeTypeRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        public bool CheckIfGivenHomeTypeIsAssignedToGivenOrganisationAndApplication(Guid homeTypeId, Guid organisationId, Guid applicationId)
        {
            using (DataverseContext ctx = new DataverseContext(service))
            {
                return (from ht in ctx.invln_HomeTypeSet
                        join app in ctx.invln_schemeSet on ht.invln_application.Id equals app.invln_schemeId
                        where ht.invln_HomeTypeId == homeTypeId && app.invln_organisationid.Id == organisationId
                        && app.invln_schemeId == applicationId
                        select ht).ToList().Any();

            }
        }

        public invln_HomeType GetHomeTypeForNullableUserAndOrganisationByIdAndApplicationId(string homeTypeId, string applicationId, string userId, string organisationId, string attributes = null)
        {
            var fetchXml = @"<fetch>
                  <entity name=""invln_hometype"">"
                    + attributes +
                    @"<filter>
                    <condition attribute=""invln_hometypeid"" operator=""eq"" value=""" + homeTypeId + @""" />
                      <condition attribute=""invln_application"" operator=""eq"" value=""" + applicationId + @""" />
                    </filter>
                    <link-entity name=""invln_scheme"" from=""invln_schemeid"" to=""invln_application"">
                          <filter>
                            <condition attribute=""invln_organisationid"" operator=""eq"" value=""" + organisationId + @""" />
                          </filter>
                          <link-entity name=""contact"" from=""contactid"" to=""invln_contactid"">"
                             + GenerateContactFilter(userId) +
                          @"</link-entity>
                        </link-entity>
                  </entity>
                </fetch>";
            EntityCollection result = service.RetrieveMultiple(new FetchExpression(fetchXml));
            return result.Entities.Select(x => x.ToEntity<invln_HomeType>()).AsEnumerable().FirstOrDefault();
        }

        public List<invln_HomeType> GetHomeTypesForNullableUserAndOrganisationRelatedToApplication(string applicationId, string userId, string organisationId, string attributes = null)
        {
            var fetchXml = @"<fetch>
                  <entity name=""invln_hometype"">"
                    + attributes +
                    @"<filter>
                      <condition attribute=""invln_application"" operator=""eq"" value=""" + applicationId + @""" />
                    </filter>
                    <link-entity name=""invln_scheme"" from=""invln_schemeid"" to=""invln_application"">
                          <filter>
                            <condition attribute=""invln_organisationid"" operator=""eq"" value=""" + organisationId + @""" />
                          </filter>
                          <link-entity name=""contact"" from=""contactid"" to=""invln_contactid"">"
                             + GenerateContactFilter(userId) +
                          @"</link-entity>
                        </link-entity>
                  </entity>
                </fetch>";
            EntityCollection result = service.RetrieveMultiple(new FetchExpression(fetchXml));
            return result.Entities.Select(x => x.ToEntity<invln_HomeType>()).ToList();
        }

        private string GenerateContactFilter(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                return @"<filter>
                              <condition attribute=""invln_externalid"" operator=""eq"" value=""" + userId + @""" />
                            </filter>";
            }
            return string.Empty;
        }
    }
}
