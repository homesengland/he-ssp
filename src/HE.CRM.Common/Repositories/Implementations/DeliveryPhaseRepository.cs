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
                    </link-entity>
                  </entity>
                </fetch>";
            EntityCollection result = service.RetrieveMultiple(new FetchExpression(fetchXml));
            return result.Entities.Select(x => x.ToEntity<invln_DeliveryPhase>()).ToList();
        }

        public bool CheckIfGivenDeliveryPhaseIsAssignedToGivenOrganisationAndApplication(Guid deliveryPhaseId, Guid organisationId, Guid applicationId)
        {
            using (DataverseContext ctx = new DataverseContext(service))
            {
                return (from ht in ctx.invln_DeliveryPhaseSet
                        join app in ctx.invln_schemeSet on ht.invln_Application.Id equals app.invln_schemeId
                        where ht.invln_DeliveryPhaseId == deliveryPhaseId && app.invln_organisationid.Id == organisationId
                        && app.invln_schemeId == applicationId
                        select ht).ToList().Any();

            }
        }

        public List<invln_DeliveryPhase> GetDeliveryPhasesRequiresAdditionalPaymentsForApplication(Guid applicationId)
        {

            var query_invln_urbrequestingearlymilestonepayments = true;
            var query_invln_application = applicationId.ToString();

            var query = new QueryExpression(invln_DeliveryPhase.EntityLogicalName);
            query.ColumnSet.AddColumns(
                invln_DeliveryPhase.Fields.invln_DeliveryPhaseId,
                invln_DeliveryPhase.Fields.invln_Application,
                invln_DeliveryPhase.Fields.invln_urbrequestingearlymilestonepayments);
            query.Criteria.AddCondition(invln_DeliveryPhase.Fields.invln_urbrequestingearlymilestonepayments, ConditionOperator.Equal, query_invln_urbrequestingearlymilestonepayments);
            query.Criteria.AddCondition(invln_DeliveryPhase.Fields.invln_Application, ConditionOperator.Equal, query_invln_application);

            return service.RetrieveMultiple(query).Entities.Select(x => x.ToEntity<invln_DeliveryPhase>()).ToList();
        }

        public List<invln_DeliveryPhase> GetDeliveryPhasesForAllocation(Guid allocationId)
        {
            var query_invln_application = allocationId.ToString();

            var query = new QueryExpression(invln_DeliveryPhase.EntityLogicalName);
            query.ColumnSet.AddColumns(
                invln_DeliveryPhase.Fields.invln_DeliveryPhaseId,
                invln_DeliveryPhase.Fields.invln_Application,
                invln_DeliveryPhase.Fields.invln_phasename,
                invln_DeliveryPhase.Fields.invln_NoofHomes,

                invln_DeliveryPhase.Fields.invln_nbrh,
                invln_DeliveryPhase.Fields.invln_buildactivitytype,
                invln_DeliveryPhase.Fields.invln_rehabactivitytype,


                invln_DeliveryPhase.Fields.invln_acquisitiondate,
                invln_DeliveryPhase.Fields.invln_AcquisitionValue,
                invln_DeliveryPhase.Fields.invln_AcquisitionPercentageValue,
                invln_DeliveryPhase.Fields.invln_acquisitionmilestoneclaimdate,

                invln_DeliveryPhase.Fields.invln_startonsitedate,
                invln_DeliveryPhase.Fields.invln_StartOnSiteValue,
                invln_DeliveryPhase.Fields.invln_StartOnSitePercentageValue,
                invln_DeliveryPhase.Fields.invln_startonsitemilestoneclaimdate,

                invln_DeliveryPhase.Fields.invln_completiondate,
                invln_DeliveryPhase.Fields.invln_CompletionValue,
                invln_DeliveryPhase.Fields.invln_CompletionPercentageValue,
                invln_DeliveryPhase.Fields.invln_completionmilestoneclaimdate


                );
            query.Criteria.AddCondition(invln_DeliveryPhase.Fields.invln_Application, ConditionOperator.Equal, query_invln_application);

            return service.RetrieveMultiple(query).Entities.Select(x => x.ToEntity<invln_DeliveryPhase>()).ToList();
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
