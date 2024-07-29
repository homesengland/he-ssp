using HE.Investments.IntegrationTestsFramework;
using HE.Investments.IntegrationTestsFramework.Auth;
using HE.Investments.Organisation.Services;

namespace HE.Investments.AHP.IntegrationTests.Framework.Prerequisites;

public class IsNotUnregisteredBodyPrerequisite : IIntegrationTestPrerequisite
{
    private readonly IOrganizationService _organisationService;

    public IsNotUnregisteredBodyPrerequisite(IOrganizationService organisationService)
    {
        _organisationService = organisationService;
    }

    public async Task<string?> Verify(ILoginData loginData)
    {
        var organisationDetails = await _organisationService.GetOrganizationDetails(loginData.OrganisationId, loginData.UserGlobalId);

        return organisationDetails.isUnregisteredBody
            ? $"Organisation {loginData.OrganisationId} should not be Unregistered Body"
            : null;
    }
}
