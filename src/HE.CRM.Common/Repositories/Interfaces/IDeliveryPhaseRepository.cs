using System;
using System.Collections.Generic;
using System.Text;
using DataverseModel;
using HE.Base.Repositories;

namespace HE.CRM.Common.Repositories.Interfaces
{
    public interface IDeliveryPhaseRepository : ICrmEntityRepository<invln_DeliveryPhase, DataverseContext>
    {
        List<invln_DeliveryPhase> GetDeliveryPhasesForNullableUserAndOrganisationRelatedToApplication(string applicationId, string externaluserId, string organisationId, string attributes = null);
        invln_DeliveryPhase GetDeliveryPhaseForNullableUserAndOrganisationByIdAndApplicationId(string deliveryPhaseId, string applicationId, string externaluserId, string organisationId, string attributes = null);
        bool CheckIfGivenDeliveryPhaseIsAssignedToGivenOrganisationAndApplication(Guid deliveryPhaseId, Guid organisationId, Guid applicationId);
    }
}
