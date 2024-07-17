using HE.Investment.AHP.WWW;
using HE.Investments.AHP.IntegrationTests.Crm;
using HE.Investments.AHP.IntegrationTests.Prerequisites;
using HE.Investments.AHP.IntegrationTests.Utils;
using HE.Investments.FrontDoor.IntegrationTests.Utils;
using HE.Investments.IntegrationTestsFramework;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.AHP.IntegrationTests.Framework;

public class AhpIntegrationTestFixture : IntegrationTestFixture<Program>
{
    public AhpCrmContext AhpCrmContext => Scope.Value.ServiceProvider.GetRequiredService<AhpCrmContext>();

    public AhpDataManipulator AhpDataManipulator => Scope.Value.ServiceProvider.GetRequiredService<AhpDataManipulator>();

    protected override void ConfigureTestServices(IServiceCollection services)
    {
        services.AddScoped<AhpCrmContext>();
        services.AddScoped<AhpDataManipulator>();
        services.AddFrontDoorManipulator();
        services.AddScoped<IIntegrationTestPrerequisite, IsOrganisationAdminPrerequisite>();
        services.AddScoped<IIntegrationTestPrerequisite, IsNotUnregisteredBodyPrerequisite>();
        services.AddScoped<IIntegrationTestPrerequisite, IsConsortiumLeadPartnerPrerequisite>();
    }
}
