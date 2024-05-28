using HE.Investment.AHP.Domain.Application.Crm;
using HE.Investment.AHP.Domain.Config;
using HE.Investments.AHP.Consortium.Domain.Crm;
using HE.Investments.Common.Contract;
using HE.Investments.IntegrationTestsFramework.Auth;

namespace HE.Investments.AHP.IntegrationTests.Crm;

public class AhpCrmContext
{
    private readonly IApplicationCrmContext _applicationCrmContext;

    private readonly IConsortiumCrmContext _consortiumCrmContext;

    private readonly string _ahpProgrammeId;

    public AhpCrmContext(
        IApplicationCrmContext applicationCrmContext,
        IConsortiumCrmContext consortiumCrmContext,
        IProgrammeSettings programmeSettings)
    {
        _applicationCrmContext = applicationCrmContext;
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
}
