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
    public interface IDeliveryPhaseService2 : ICrmService
    {
        Guid SetDeliveryPhase(string deliveryPhase, string userId, string organisationId, string applicationId, string fieldsToSet = null);
        //void CalculateFunding(invln_scheme application, invln_DeliveryPhase deliveryPhaseMapped, List<invln_milestoneframeworkitem> milestones, invln_DeliveryPhase deliveryPhaseToUpdateOrCreate);

    }
}
