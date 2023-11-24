using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Data;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.Crm;
using HE.Investment.AHP.Domain.HomeTypes.Crm.Segments;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.Services;
using HE.Investment.AHP.Domain.Mock;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investments.Account.Shared.Config;
using HE.Investments.Loans.Common.Utils;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investment.AHP.Domain.Config;

public static class DomainModule
{
    public static void AddDomainModule(this IServiceCollection services)
    {
        services.AddAccountSharedModule(true);
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient(typeof(IRequestExceptionHandler<,,>), typeof(DomainValidationHandler<,,>));

        services.AddScoped<IApplicationCrmContext, ApplicationCrmContext>();

        AddHomeTypes(services);
        AddFinancialDetails(services);
        AddApplication(services);
        AddScheme(services);
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
        services.AddSingleton<IHomeTypeCrmSegmentMapper, SupportedHousingInformationSegmentMapper>();

        // TODO: change service do scoped after introducing integration with IHttpDocumentService
        services.AddSingleton<IDesignFileService, DesignFileService>();
    }

    private static void AddFinancialDetails(IServiceCollection services)
    {
        services.AddScoped<IFinancialDetailsRepository, FinancialDetailsRepository>();
    }

    private static void AddApplication(IServiceCollection services)
    {
        services.AddScoped<IApplicationRepository, ApplicationRepository>();
    }

    private static void AddScheme(IServiceCollection services)
    {
        services.AddScoped<ISchemeRepository, SchemeRepository>();

        // TODO: change scope when file source implemented
        services.AddSingleton<IFileService, FileService>();
    }
}
