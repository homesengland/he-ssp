using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Domain.Application.Crm;
using HE.Investment.AHP.Domain.Delivery.Crm;
using HE.Investment.AHP.Domain.HomeTypes.Crm;
using HE.Investment.AHP.Domain.Site.Crm;
using HE.Investments.AHP.Consortium.Domain.Crm;
using HE.Investments.Common.Contract;
using HE.Investments.IntegrationTestsFramework.Auth;
using HE.Investments.Programme.Contract.Config;

namespace HE.Investments.AHP.IntegrationTests.Framework.Crm;

public class AhpCrmContext
{
    private readonly ISiteCrmContext _siteCrmContext;

    private readonly IApplicationCrmContext _applicationCrmContext;

    private readonly IHomeTypeCrmContext _homeTypeCrmContext;

    private readonly IDeliveryPhaseCrmContext _deliveryPhaseCrmContext;

    private readonly IConsortiumCrmContext _consortiumCrmContext;

    private readonly string _ahpProgrammeId;

    public AhpCrmContext(
        ISiteCrmContext siteCrmContext,
        IApplicationCrmContext applicationCrmContext,
        IHomeTypeCrmContext homeTypeCrmContext,
        IDeliveryPhaseCrmContext deliveryPhaseCrmContext,
        IConsortiumCrmContext consortiumCrmContext,
        IProgrammeSettings programmeSettings)
    {
        _siteCrmContext = siteCrmContext;
        _applicationCrmContext = applicationCrmContext;
        _homeTypeCrmContext = homeTypeCrmContext;
        _deliveryPhaseCrmContext = deliveryPhaseCrmContext;
        _consortiumCrmContext = consortiumCrmContext;
        _ahpProgrammeId = programmeSettings.AhpProgrammeId;
    }

    public async Task ChangeApplicationStatus(string applicationId, ApplicationStatus applicationStatus, ILoginData loginData)
    {
        await _applicationCrmContext.ChangeApplicationStatus(
            applicationId,
            loginData.OrganisationId,
            loginData.UserGlobalId,
            applicationStatus,
            "[IntegrationTests]",
            true,
            CancellationToken.None);
    }

    public async Task<(ConsortiumId Id, bool IsLeadPartner)?> GetAhpConsortium(ILoginData loginData)
    {
        var consortia = await _consortiumCrmContext.GetConsortiumsListByMemberId(loginData.OrganisationId, CancellationToken.None);
        var consortium = consortia.FirstOrDefault(x => x.programmeId == _ahpProgrammeId);

        if (consortium == null)
        {
            return null;
        }

        return (ConsortiumId.From(consortium.id), consortium.leadPartnerId == loginData.OrganisationId);
    }

    public async Task<string> SaveAhpSite(SiteDto dto, ILoginData loginData, CancellationToken cancellationToken)
    {
        return await _siteCrmContext.Save(loginData.OrganisationId, loginData.UserGlobalId, dto, cancellationToken);
    }

    public async Task<string> SaveAhpApplication(AhpApplicationDto dto, ILoginData loginData, CancellationToken cancellationToken)
    {
        var organisationId = loginData.OrganisationId;
        var applicationId = await _applicationCrmContext.Save(dto, organisationId, loginData.UserGlobalId, cancellationToken);
        return applicationId;
    }

    public async Task<string> SaveAhpHomeType(HomeTypeDto homeType, ILoginData loginData, CancellationToken cancellationToken)
    {
        var organisationId = loginData.OrganisationId;
        return await _homeTypeCrmContext.Save(homeType, organisationId, loginData.UserGlobalId, cancellationToken);
    }

    public async Task<string> SaveAhpDeliveryPhase(DeliveryPhaseDto deliveryPhase, ILoginData loginData, CancellationToken cancellationToken)
    {
        var organisationId = loginData.OrganisationId;
        return await _deliveryPhaseCrmContext.Save(deliveryPhase, organisationId, loginData.UserGlobalId, cancellationToken);
    }
}
