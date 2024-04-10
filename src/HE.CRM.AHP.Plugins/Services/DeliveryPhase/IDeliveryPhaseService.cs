using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.CRM.AHP.Plugins.Services.DeliveryPhase
{
    public interface IDeliveryPhaseService : ICrmService
    {
        void DeleteDeliveryPhase(string applicationId, string organisationId, string deliveryPhaseId, string externalUserId);
        DeliveryPhaseDto GetDeliveryPhase(string applicationId, string organizationId, string externalUserId, string deliveryPhaseId, string fieldsToRetrieve);
        List<DeliveryPhaseDto> GetDeliveryPhases(string applicationId, string organizationId, string externalUserId, string fieldsToRetrieve);
        Guid SetDeliveryPhase(string deliveryPhase, string userId, string organisationId, string applicationId, string fieldsToSet = null);
        void CalculateFunding(invln_scheme application, invln_DeliveryPhase deliveryPhaseMapped, List<invln_milestoneframeworkitem> milestones, invln_DeliveryPhase deliveryPhaseToUpdateOrCreate);

    }
}
