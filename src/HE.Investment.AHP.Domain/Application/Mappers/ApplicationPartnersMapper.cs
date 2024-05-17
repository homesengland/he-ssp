extern alias Org;

using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investments.Common.Contract;
using Org::HE.Investments.Organisation.ValueObjects;

namespace HE.Investment.AHP.Domain.Application.Mappers;

internal static class ApplicationPartnersMapper
{
    public static ApplicationPartners ToDomain(AhpApplicationDto dto)
    {
        if (string.IsNullOrEmpty(dto.developingPartnerId)
            || string.IsNullOrEmpty(dto.ownerOfTheLandDuringDevelopmentId)
            || string.IsNullOrEmpty(dto.ownerOfTheHomesAfterCompletionId))
        {
            return ApplicationPartners.ConfirmedPartner(OrganisationId.From(dto.organisationId));
        }

        return new ApplicationPartners(
            MapPartner(dto.developingPartnerId, dto.developingPartnerName),
            MapPartner(dto.ownerOfTheLandDuringDevelopmentId, dto.ownerOfTheLandDuringDevelopmentName),
            MapPartner(dto.ownerOfTheHomesAfterCompletionId, dto.ownerOfTheHomesAfterCompletionName),
            dto.applicationPartnerConfirmation);
    }

    private static InvestmentsOrganisation MapPartner(string organisationId, string organisationName) =>
        new(OrganisationId.From(organisationId), organisationName);
}
