extern alias Org;

using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Application.Repositories.Interfaces;
using HE.Investment.AHP.Domain.Data;
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
using HE.Investment.AHP.Domain.Programme;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investment.AHP.Domain.Scheme.Services;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investments.Account.Shared.Config;
using HE.Investments.Common.Utils;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using Org::HE.Investments.Organisation.LocalAuthorities.Repositories;

namespace HE.Investment.AHP.Domain.Config;

public static class DomainModule
{
    public static void AddDomainModule(this IServiceCollection services)
    {
        services.AddAccountSharedModule(true);
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient(typeof(IRequestExceptionHandler<,,>), typeof(DomainValidationHandler<,,>));

        services.AddScoped<ApplicationCrmContext>();
        services.AddScoped<IApplicationCrmContext>(x => new RequestCacheApplicationCrmContextDecorator(x.GetRequiredService<ApplicationCrmContext>()));
        services.AddScoped<IDocumentsCrmContext, DocumentsCrmContext>();
        services.AddSingleton<IAhpDocumentSettings, AhpDocumentSettings>();

        AddHomeTypes(services);
        AddFinancialDetails(services);
        AddApplication(services);
        AddScheme(services);
        AddSite(services);
        AddDelivery(services);
    }

    private static void AddHomeTypes(IServiceCollection services)
    {
        services.AddScoped<IHomeTypeRepository, HomeTypeRepository>();
        services.AddScoped<IHomeTypeCrmContext, HomeTypeCrmContext>();
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
    }

    private static void AddFinancialDetails(IServiceCollection services)
    {
        services.AddScoped<IFinancialDetailsRepository, FinancialDetailsRepository>();
    }

    private static void AddApplication(IServiceCollection services)
    {
        services.AddScoped<IApplicationRepository, ApplicationRepository>();
        services.AddScoped<IAhpProgrammeRepository, AhpProgrammeRepository>();
        services.AddScoped<IChangeApplicationStatus, ApplicationRepository>();
    }

    private static void AddScheme(IServiceCollection services)
    {
        services.AddScoped<ISchemeRepository, SchemeRepository>();

        services.AddScoped<IAhpFileLocationProvider<LocalAuthoritySupportFileParams>, LocalAuthoritySupportFileLocationProvider>();
        services.AddScoped<IAhpFileService<LocalAuthoritySupportFileParams>, AhpFileService<LocalAuthoritySupportFileParams>>();
    }

    private static void AddSite(IServiceCollection services)
    {
        services.AddScoped<ISiteRepository, SiteRepository>();
        services.AddScoped<ILocalAuthorityRepository, LocalAuthorityRepository>();
    }

    private static void AddDelivery(IServiceCollection services)
    {
        services.AddScoped<IDeliveryPhaseRepository, DeliveryPhaseRepository>();
        services.AddScoped<IDeliveryPhaseCrmContext, DeliveryPhaseCrmContext>();
        services.AddSingleton<IDeliveryPhaseCrmMapper, DeliveryPhaseCrmMapper>();
        services.AddScoped<IMilestoneDatesInProgrammeDateRangePolicy, MilestoneDatesInProgrammeDateRangePolicy>();
    }
}
