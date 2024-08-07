using HE.Investment.AHP.Domain.Config;
using HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;
using HE.Investment.AHP.WWW.Models.AllocationClaim.Factories;
using HE.Investment.AHP.WWW.Models.Application.Factories;
using HE.Investment.AHP.WWW.Models.Delivery.Factories;
using HE.Investment.AHP.WWW.Models.FinancialDetails.Factories;
using HE.Investment.AHP.WWW.Models.HomeTypes.Factories;
using HE.Investment.AHP.WWW.Models.Scheme.Factories;
using HE.Investment.AHP.WWW.Models.Site.Factories;
using HE.Investment.AHP.WWW.Notifications;
using HE.Investment.AHP.WWW.Routing;
using HE.Investments.AHP.Allocation.Domain.Config;
using HE.Investments.AHP.Consortium.Domain.Config;
using HE.Investments.AHP.ProjectDashboard.Domain.Config;
using HE.Investments.Api.Config;
using HE.Investments.Common;
using HE.Investments.Common.Config;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.Models.App;
using HE.Investments.Common.WWW.Config;
using HE.Investments.Common.WWW.Infrastructure.Authorization;
using HE.Investments.Common.WWW.Infrastructure.ErrorHandling;
using HE.Investments.Common.WWW.Infrastructure.Middlewares;
using HE.Investments.Consortium.Shared.Authorization;
using HE.Investments.DocumentService.Extensions;
using HE.Investments.Organisation.Config;
using HE.Investments.Programme.Domain.Config;
using HE.UtilsService.BannerNotification.Shared;

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
        services.AddConsortiumDomainModule();
        services.AddAllocationDomainModule();
        services.AddProjectDashboardDomainModule();
        services.AddProgrammeSubdomainModule();
        services.AddEventInfrastructure();
        services.AddNotificationPublisher(ApplicationType.Ahp);
        services.AddNotificationConsumer(ApplicationType.Ahp, typeof(HomeTypeHasBeenCreatedDisplayNotificationFactory).Assembly);
        services.AddDocumentServiceModule();
        services.AddApiModule();
        AddViewModelFactories(services);

        services.AddSingleton<IAhpExternalLinks, AhpExternalLinks>();
        services.AddSingleton<IFrontDoorLinks, FrontDoorLinks>();
        AddConsortiumAuthorization(services);
    }

    private static void AddConsortiumAuthorization(IServiceCollection services)
    {
        services.AddScoped<ConsortiumAccessPolicy>();
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
        services.AddScoped<IAllocationClaimCheckAnswersViewModelFactory, AllocationClaimCheckAnswersViewModelFactory>();
    }
}
