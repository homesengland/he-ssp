using System.Configuration;
using HE.Investments.Common.Extensions;
using HE.Investments.Organisation.CompaniesHouse;
using HE.Investments.Organisation.CrmRepository;
using HE.Investments.Organisation.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.Investments.Organisation.Config;

public static class ServiceCollectionExtensions
{
    public static void AddOrganizationsModule(this IServiceCollection services)
    {
        services.AddCompaniesHouseHttpClient();
        services.AddScoped<IOrganisationSearchService, OrganisationSearchService>();
        AddOrganisationCrmModule(services);

        services.AddAppConfiguration<ICompaniesHouseConfig, CompaniesHouseConfig>("CompaniesHouse");
    }

    public static void AddOrganisationCrmModule(this IServiceCollection services)
    {
        if (services.All(s => s.ServiceType != typeof(IOrganizationServiceAsync2)))
        {
            throw new ConfigurationErrorsException($"{nameof(IOrganizationServiceAsync2)} is required to be added to service collection.");
        }

        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IOrganizationCrmSearchService, OrganizationCrmSearchService>();
        services.AddScoped<IOrganisationChangeRequestRepository, OrganisationChangeRequestRepository>();
        services.AddScoped<IContactService, ContactService>();
        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<IWebRoleRepository, WebRoleRepository>();
        services.AddScoped<IPortalPermissionRepository, PortalPermissionRepository>();
        services.AddScoped<IOrganizationService, OrganizationService>();
        services.AddScoped<IProgrammeService, ProgrammeService>();
    }
}
