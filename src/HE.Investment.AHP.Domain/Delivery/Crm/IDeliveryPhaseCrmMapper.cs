using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Policies;
using HE.Investments.Account.Shared;

namespace HE.Investment.AHP.Domain.Delivery.Crm;

public interface IDeliveryPhaseCrmMapper
{
    DeliveryPhaseEntity MapToDomain(
        ApplicationBasicInfo application,
        OrganisationBasicInfo organisation,
        DeliveryPhaseDto dto,
        IOnlyCompletionMilestonePolicy onlyCompletionMilestonePolicy);

    DeliveryPhaseDto MapToDto(DeliveryPhaseEntity entity);
}
