using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investments.Account.Shared;

namespace HE.Investment.AHP.Domain.Delivery.Crm;

public interface IDeliveryPhaseCrmMapper
{
    DeliveryPhaseEntity MapToDomain(ApplicationBasicInfo application, OrganisationBasicInfo organisation, DeliveryPhaseDto dto);

    DeliveryPhaseDto MapToDto(DeliveryPhaseEntity entity);
}
