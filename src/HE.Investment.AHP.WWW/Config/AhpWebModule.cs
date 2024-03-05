using HE.Investment.AHP.Domain.Config;
using HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;
using HE.Investment.AHP.WWW.Models.Application.Factories;
using HE.Investment.AHP.WWW.Models.Delivery.Factories;
using HE.Investment.AHP.WWW.Models.FinancialDetails.Factories;
using HE.Investment.AHP.WWW.Models.HomeTypes.Factories;
using HE.Investment.AHP.WWW.Models.Scheme.Factories;
using HE.Investment.AHP.WWW.Models.Site.Factories;
using HE.Investment.AHP.WWW.Notifications;
using HE.Investment.AHP.WWW.Routing;
using HE.Investment.AHP.WWW.Utils;
using HE.Investments.Common;
using HE.Investments.Common.Config;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.Models.App;
using HE.Investments.Common.WWW.Infrastructure.Authorization;
using HE.Investments.Common.WWW.Infrastructure.ErrorHandling;
using HE.Investments.Common.WWW.Infrastructure.Middlewares;
using HE.Investments.DocumentService.Extensions;
using HE.Investments.Organisation.Config;

namespace HE.Investment.AHP.WWW.Config;

public static class AhpWebModule
{
    public static void AddWebModule(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddOrganisationCrmModule();
        service.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SaveHomeTypeDetailsCommandHandler).Assembly));
        service.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DomainValidationHandler<,,>).Assembly));
        service.AddScoped<NonceModel>();

        AddConfiguration(service, configuration);
        service.AddHttpUserContext();
        service.AddDomainModule();
        service.AddEventInfrastructure();
        service.AddNotifications("AHP", typeof(HomeTypeHasBeenCreatedDisplayNotificationFactory).Assembly);
        service.AddDocumentServiceModule();
        AddViewModelFactories(service);

        service.AddScoped<ISchemeProvider, CachedSchemeProvider>();
        service.AddScoped<IDeliveryPhaseProvider, CachedDeliveryPhaseProvider>();
        service.AddSingleton<IExternalLinks, ExternalLinks>();
    }

    private static void AddConfiguration(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IErrorViewPaths, AhpErrorViewPaths>();
        services.AddSingleton<IAhpAppConfig, AhpAppConfig>(x => x.GetRequiredService<IConfiguration>().GetSection("AppConfiguration").Get<AhpAppConfig>());
        services.Configure<ContactInfoOptions>(configuration.GetSection("AppConfiguration:ContactInfo"));
        services.AddSingleton<IDataverseConfig, DataverseConfig>(x =>
            x.GetRequiredService<IConfiguration>().GetSection("AppConfiguration:Dataverse").Get<DataverseConfig>());
    }

    private static void AddViewModelFactories(IServiceCollection services)
    {
        services.AddScoped<ISiteSummaryViewModelFactory, SiteSummaryViewModelFactory>();
        services.AddScoped<IApplicationSummaryViewModelFactory, ApplicationSummaryViewModelFactory>();
        services.AddScoped<ISchemeSummaryViewModelFactory, SchemeSummaryViewModelFactory>();
        services.AddScoped<IFinancialDetailsSummaryViewModelFactory, FinancialDetailsSummaryViewModelFactory>();
        services.AddScoped<IHomeTypeSummaryViewModelFactory, HomeTypeSummaryViewModelFactory>();
        services.AddScoped<IDeliveryPhaseCheckAnswersViewModelFactory, DeliveryPhaseCheckAnswersViewModelFactory>();
    }
}
