using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;

namespace HE.CRM.Common.Repositories.implementations
{
    public class DeliveryPhaseRepository : CrmEntityRepository<invln_DeliveryPhase, DataverseContext>, IDeliveryPhaseRepository
    {
        public DeliveryPhaseRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        public invln_DeliveryPhase GetDeliveryPhaseForNullableUserAndOrganisationByIdAndApplicationId(string deliveryPhaseId, string applicationId, string externaluserId, string organisationId, string attributes = null)
        {
            var fetchXml = @"<fetch>
                  <entity name=""invln_deliveryphase"">"
                    + attributes +
                    @"<filter>
                    <condition attribute=""invln_deliveryphaseid"" operator=""eq"" value=""" + deliveryPhaseId + @""" />
                      <condition attribute=""invln_application"" operator=""eq"" value=""" + applicationId + @""" />
                    </filter>
                    <link-entity name=""invln_scheme"" from=""invln_schemeid"" to=""invln_application"">
                          <filter>
                            <condition attribute=""invln_organisationid"" operator=""eq"" value=""" + organisationId + @""" />
                          </filter>
                          <link-entity name=""contact"" from=""contactid"" to=""invln_contactid"">"
                             + GenerateContactFilter(externaluserId) +
                          @"</link-entity>
                        </link-entity>
                  </entity>
                </fetch>";
            EntityCollection result = service.RetrieveMultiple(new FetchExpression(fetchXml));
            return result.Entities.Select(x => x.ToEntity<invln_DeliveryPhase>()).AsEnumerable().FirstOrDefault();
        }

        public List<invln_DeliveryPhase> GetDeliveryPhasesForNullableUserAndOrganisationRelatedToApplication(string applicationId, string externaluserId, string organisationId, string attributes = null)
        {
            var fetchXml = @"<fetch>
                  <entity name=""invln_deliveryphase"">"
                    + attributes +
                    @"<filter>
                      <condition attribute=""invln_application"" operator=""eq"" value=""" + applicationId + @""" />
                    </filter>
                    <link-entity name=""invln_scheme"" from=""invln_schemeid"" to=""invln_application"">
                          <filter>
                            <condition attribute=""invln_organisationid"" operator=""eq"" value=""" + organisationId + @""" />
                          </filter>
                          <link-entity name=""contact"" from=""contactid"" to=""invln_contactid"">"
                             + GenerateContactFilter(externaluserId) +
                          @"</link-entity>
                        </link-entity>
                  </entity>
                </fetch>";
            EntityCollection result = service.RetrieveMultiple(new FetchExpression(fetchXml));
            return result.Entities.Select(x => x.ToEntity<invln_DeliveryPhase>()).ToList();
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
