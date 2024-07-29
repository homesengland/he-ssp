using HE.Investments.AHP.IntegrationTests.Framework.Crm;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.IntegrationTestsFramework.Auth;

namespace HE.Investments.AHP.IntegrationTests.Framework.Prerequisites;

public class IsConsortiumLeadPartnerPrerequisite : IIntegrationTestPrerequisite
{
    private readonly AhpCrmContext _ahpCrmContext;

    public IsConsortiumLeadPartnerPrerequisite(AhpCrmContext ahpCrmContext)
    {
        _ahpCrmContext = ahpCrmContext;
    }

    public async Task<string?> Verify(ILoginData loginData)
    {
        var consortium = await _ahpCrmContext.GetAhpConsortium(loginData);

        return consortium is { IsLeadPartner: true }
            ? null
            : $"Organisation {loginData.OrganisationId} needs to be Consortium Lead Partner";
    }
}
