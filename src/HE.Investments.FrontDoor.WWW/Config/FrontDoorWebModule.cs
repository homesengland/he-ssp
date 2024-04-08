using HE.Investments.Common;
using HE.Investments.Common.Config;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.Models.App;
using HE.Investments.Common.WWW.Config;
using HE.Investments.Common.WWW.Infrastructure.Authorization;
using HE.Investments.Common.WWW.Infrastructure.ErrorHandling;
using HE.Investments.Common.WWW.Infrastructure.Middlewares;
using HE.Investments.FrontDoor.Domain.Config;
using HE.Investments.FrontDoor.WWW.Models.Factories;
using HE.Investments.FrontDoor.WWW.Routing;
using HE.Investments.Organisation.Config;
using HE.Investments.Organisation.LocalAuthorities.Repositories;

namespace HE.Investments.FrontDoor.WWW.Config;

public static class FrontDoorWebModule
{
    public static void AddWebModule(this IServiceCollection services)
    {
        services.AddAppConfiguration<IMvcAppConfig, MvcAppConfig>();
        services.AddOrganisationCrmModule();
        services.AddScoped<ILocalAuthorityRepository, LocalAuthorityRepository>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DomainModule).Assembly));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DomainValidationHandler<,,>).Assembly));
        services.AddScoped<NonceModel>();
        services.AddNotificationPublisher(ApplicationType.FrontDoor);

        AddConfiguration(services);
        services.AddDomainModule();
        services.AddHttpUserContext();
        services.AddEventInfrastructure();
    }

    private static void AddConfiguration(IServiceCollection services)
    {
        services.AddSingleton<IExternalLinks, ExternalLinks>();
        services.AddSingleton<IErrorViewPaths, FrontDoorErrorViewPaths>();
        services.AddAppConfiguration<ContactInfoOptions>("ContactInfo");
        services.AddAppConfiguration<IDataverseConfig, DataverseConfig>("Dataverse");
        services.AddScoped<IProjectSummaryViewModelFactory, ProjectSummaryViewModelFactory>();
        services.AddAppConfiguration<LoanApplicationConfig>("LoanApplicationService");
    }
}
