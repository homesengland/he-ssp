using HE.Investment.AHP.WWW;
using HE.Investments.AHP.IntegrationTests.Framework.Crm;
using HE.Investments.AHP.IntegrationTests.Framework.Helpers;
using HE.Investments.AHP.IntegrationTests.Framework.Prerequisites;
using HE.Investments.AHP.ProjectDashboard.Domain.Project.Crm;
using HE.Investments.FrontDoor.IntegrationTests.Utils;
using HE.Investments.IntegrationTestsFramework;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.AHP.IntegrationTests.Framework;

public class AhpIntegrationTestFixture : IntegrationTestFixture<Program>
{
    public AhpCrmContext AhpCrmContext => Scope.Value.ServiceProvider.GetRequiredService<AhpCrmContext>();

    public AhpDataManipulator AhpDataManipulator => Scope.Value.ServiceProvider.GetRequiredService<AhpDataManipulator>();

    public AhpProjectDataManipulator AhpProjectDataManipulator => Scope.Value.ServiceProvider.GetRequiredService<AhpProjectDataManipulator>();

    protected override void ConfigureTestServices(IServiceCollection services)
    {
        services.AddScoped<AhpCrmContext>();
        services.AddScoped<AhpDataManipulator>();
        services.AddScoped<AhpProjectDataManipulator>();
        services.AddFrontDoorManipulator();
        services.AddScoped<ProjectCrmContext>();
        services.AddScoped<IIntegrationTestPrerequisite, IsOrganisationAdminPrerequisite>();
        services.AddScoped<IIntegrationTestPrerequisite, IsNotUnregisteredBodyPrerequisite>();
        services.AddScoped<IIntegrationTestPrerequisite, IsConsortiumLeadPartnerPrerequisite>();
    }
}
