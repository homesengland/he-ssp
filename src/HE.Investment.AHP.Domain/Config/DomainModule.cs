extern alias Org;

using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.Application.Crm;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Delivery.Crm;
using HE.Investment.AHP.Domain.Delivery.Policies;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investment.AHP.Domain.Documents.Config;
using HE.Investment.AHP.Domain.Documents.Crm;
using HE.Investment.AHP.Domain.Documents.Services;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.Crm;
using HE.Investment.AHP.Domain.HomeTypes.Crm.Segments;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Mappers;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.Services;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investment.AHP.Domain.PrefillData.Repositories;
using HE.Investment.AHP.Domain.Programme;
using HE.Investment.AHP.Domain.Programme.Config;
using HE.Investment.AHP.Domain.Programme.Crm;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investment.AHP.Domain.Scheme.Services;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investment.AHP.Domain.Site.Crm;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investments.Account.Shared.Config;
using HE.Investments.Common;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Infrastructure.Cache.Interfaces;
using HE.Investments.Common.Utils;
using HE.Investments.FrontDoor.Shared.Config;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.PowerPlatform.Dataverse.Client;
using Org::HE.Investments.Organisation.LocalAuthorities;
using Org::HE.Investments.Organisation.LocalAuthorities.Repositories;

namespace HE.Investment.AHP.Domain.Config;

public static class DomainModule
{
    public static void AddDomainModule(this IServiceCollection services)
    {
        services.AddAccountSharedModule();
        services.AddFrontDoorSharedModule();
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient(typeof(IRequestExceptionHandler<,,>), typeof(DomainValidationHandler<,,>));

        services
            .AddProgramme()
            .AddSite()
            .AddApplication()
            .AddScheme()
            .AddHomeTypes()
            .AddFinancialDetails()
            .AddDelivery()
            .AddPrefillData()
            .AddDocuments();
    }

    private static IServiceCollection AddHomeTypes(this IServiceCollection services)
    {
        services.AddScoped<IHomeTypeRepository, HomeTypeRepository>();
        services.AddScoped<IHomeTypeCrmContext, HomeTypeCrmContext>();
        services.Decorate<IHomeTypeCrmContext, RequestCacheHomeTypeCrmContextDecorator>();
        services.AddSingleton<IHomeTypeCrmMapper, HomeTypeCrmMapper>();
        services.AddSingleton<IHomeTypeCrmSegmentMapper, HomeInformationCrmSegmentMapper>();
        services.AddSingleton<IHomeTypeCrmSegmentMapper, DisabledAndVulnerablePeopleCrmSegmentMapper>();
        services.AddSingleton<IHomeTypeCrmSegmentMapper, OlderPeopleCrmSegmentMapper>();
        services.AddSingleton<IHomeTypeCrmSegmentMapper, DesignPlansCrmSegmentMapper>();
        services.AddSingleton<IHomeTypeCrmSegmentMapper, SupportedHousingInformationCrmSegmentMapper>();
        services.AddSingleton<IHomeTypeCrmSegmentMapper, TenureDetailsCrmSegmentMapper>();
        services.AddSingleton<IHomeTypeCrmSegmentMapper, ModernMethodsConstructionCrmSegmentMapper>();

        services.AddSingleton<IHomeTypeSegmentContractMapper<DesignPlansSegmentEntity, DesignPlans>, DesignPlansSegmentContractMapper>();
        services.AddSingleton<IHomeTypeSegmentContractMapper<DisabledPeopleHomeTypeDetailsSegmentEntity, DisabledPeopleHomeTypeDetails>, DisabledPeopleHomeTypeDetailsSegmentContractMapper>();
        services.AddSingleton<IHomeTypeSegmentContractMapper<HomeInformationSegmentEntity, HomeInformation>, HomeInformationSegmentContractMapper>();
        services.AddSingleton<IHomeTypeSegmentContractMapper<OlderPeopleHomeTypeDetailsSegmentEntity, OlderPeopleHomeTypeDetails>, OlderPeopleHomeTypeDetailsSegmentContractMapper>();
        services.AddSingleton<IHomeTypeSegmentContractMapper<SupportedHousingInformationSegmentEntity, SupportedHousingInformation>, SupportedHousingInformationSegmentContractMapper>();
        services.AddSingleton<IHomeTypeSegmentContractMapper<TenureDetailsSegmentEntity, TenureDetails>, TenureDetailsSegmentContractMapper>();
        services.AddSingleton<IHomeTypeSegmentContractMapper<ModernMethodsConstructionSegmentEntity, ModernMethodsConstruction>, ModernMethodsConstructionSegmentContractMapper>();

        services.AddScoped<IAhpFileLocationProvider<DesignFileParams>, DesignFileLocationProvider>();
        services.AddScoped<IAhpFileService<DesignFileParams>, AhpFileService<DesignFileParams>>();

        return services;
    }

    private static IServiceCollection AddFinancialDetails(this IServiceCollection services)
    {
        services.AddScoped<IFinancialDetailsRepository, FinancialDetailsRepository>();

        return services;
    }

    private static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IApplicationCrmContext, ApplicationCrmContext>();
        services.Decorate<IApplicationCrmContext, RequestCacheApplicationCrmContextDecorator>();
        services.AddScoped<IApplicationRepository, ApplicationRepository>();
        services.AddScoped<IApplicationSectionStatusChanger, ApplicationSectionStatusChanger>();

        return services;
    }

    private static IServiceCollection AddScheme(this IServiceCollection services)
    {
        services.AddScoped<ISchemeRepository, SchemeRepository>();

        services.AddScoped<IAhpFileLocationProvider<LocalAuthoritySupportFileParams>, LocalAuthoritySupportFileLocationProvider>();
        services.AddScoped<IAhpFileService<LocalAuthoritySupportFileParams>, AhpFileService<LocalAuthoritySupportFileParams>>();

        return services;
    }

    private static IServiceCollection AddSite(this IServiceCollection services)
    {
        services.AddScoped<ISiteCrmContext, SiteCrmContext>();
        services.Decorate<ISiteCrmContext, RequestCacheSiteCrmContextDecorator>();
        services.AddScoped<ISiteRepository, SiteRepository>();
        services.Decorate<ISiteRepository, CacheSiteRepositoryDecorator>();
        services.AddScoped<ILocalAuthorityRepository>(x => new LocalAuthorityRepository(
            x.GetRequiredService<IOrganizationServiceAsync2>(),
            x.GetRequiredService<ICacheService>(),
            LocalAuthoritySource.Ahp));

        return services;
    }

    private static IServiceCollection AddDelivery(this IServiceCollection services)
    {
        services.AddScoped<IDeliveryPhaseRepository, DeliveryPhaseRepository>();
        services.AddScoped<IDeliveryPhaseCrmContext, DeliveryPhaseCrmContext>();
        services.Decorate<IDeliveryPhaseCrmContext, RequestCacheDeliverPhaseCrmContextDecorator>();
        services.AddSingleton<IDeliveryPhaseCrmMapper, DeliveryPhaseCrmMapper>();
        services.AddScoped<IMilestoneDatesInProgrammeDateRangePolicy, MilestoneDatesInProgrammeDateRangePolicy>();

        return services;
    }

    private static IServiceCollection AddProgramme(this IServiceCollection services)
    {
        services.AddScoped<IAhpProgrammeRepository, AhpProgrammeRepository>();
        services.AddScoped<IProgrammeCrmContext, ProgrammeCrmContext>();
        services.Decorate<IProgrammeCrmContext, CacheProgrammeCrmContextDecorator>();
        services.Decorate<IProgrammeCrmContext, RequestCacheProgrammeCrmContextDecorator>();
        services.AddAppConfiguration<IProgrammeSettings, ProgrammeSettings>();

        return services;
    }

    private static IServiceCollection AddPrefillData(this IServiceCollection services)
    {
        services.AddScoped<IAhpPrefillDataRepository, AhpPrefillDataRepository>();

        return services;
    }

    private static void AddDocuments(this IServiceCollection services)
    {
        services.AddScoped<IDocumentsCrmContext, DocumentsCrmContext>();
        services.AddSingleton<IAhpDocumentSettings, AhpDocumentSettings>();
    }
}
