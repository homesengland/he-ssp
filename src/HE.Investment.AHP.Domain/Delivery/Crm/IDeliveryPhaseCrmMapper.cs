using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investments.Account.Shared;

namespace HE.Investment.AHP.Domain.Delivery.Crm;

public interface IDeliveryPhaseCrmMapper
{
    IReadOnlyCollection<string> CrmFields { get; }

    DeliveryPhaseEntity MapToDomain(ApplicationBasicInfo application, OrganisationBasicInfo organisation, DeliveryPhaseDto dto, SchemeFunding schemeFunding);

    DeliveryPhaseDto MapToDto(DeliveryPhaseEntity entity);
}
