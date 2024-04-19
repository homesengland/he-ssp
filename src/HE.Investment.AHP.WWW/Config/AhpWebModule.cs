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
using HE.Investments.DocumentService.Extensions;
using HE.Investments.Organisation.Config;

namespace HE.Investment.AHP.WWW.Config;

public static class AhpWebModule
{
    public static void AddWebModule(this IServiceCollection services)
    {
        services.AddAppConfiguration<IMvcAppConfig, MvcAppConfig>();
        services.AddOrganisationCrmModule();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SaveHomeTypeDetailsCommandHandler).Assembly));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DomainValidationHandler<,,>).Assembly));
        services.AddScoped<NonceModel>();

        AddConfiguration(services);
        services.AddHttpUserContext();
        services.AddDomainModule();
        services.AddEventInfrastructure();
        services.AddNotificationPublisher(ApplicationType.Ahp);
        services.AddNotificationConsumer(ApplicationType.Ahp, typeof(HomeTypeHasBeenCreatedDisplayNotificationFactory).Assembly);
        services.AddDocumentServiceModule();
        AddViewModelFactories(services);

        services.AddSingleton<IAhpExternalLinks, AhpExternalLinks>();
    }

    private static void AddConfiguration(IServiceCollection services)
    {
        services.AddSingleton<IErrorViewPaths, AhpErrorViewPaths>();
        services.AddAppConfiguration<ContactInfo>("ContactInfo");
        services.AddAppConfiguration<IDataverseConfig, DataverseConfig>("Dataverse");
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
