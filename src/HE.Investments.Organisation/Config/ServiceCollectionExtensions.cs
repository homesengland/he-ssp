using System.Configuration;
using HE.Investments.Organisation.CompaniesHouse;
using HE.Investments.Organisation.CrmRepository;
using HE.Investments.Organisation.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.Investments.Organisation.Config;

public static class ServiceCollectionExtensions
{
    public static void AddOrganizationsModule(this IServiceCollection serviceCollections)
    {
        serviceCollections.AddCompaniesHouseHttpClient();
        serviceCollections.AddScoped<IOrganisationSearchService, OrganisationSearchService>();
        AddOrganisationCrmModule(serviceCollections);

        serviceCollections.AddSingleton<ICompaniesHouseConfig>(x =>
            x.GetRequiredService<IConfiguration>().GetRequiredSection("AppConfiguration:CompaniesHouse").Get<CompaniesHouseConfig>());
    }

    public static void AddOrganisationCrmModule(this IServiceCollection serviceCollections)
    {
        if (serviceCollections.All(s => s.ServiceType != typeof(IOrganizationServiceAsync2)))
        {
            throw new ConfigurationErrorsException($"{nameof(IOrganizationServiceAsync2)} is required to be added to service collection.");
        }

        serviceCollections.AddScoped<IOrganizationRepository, OrganizationRepository>();
        serviceCollections.AddScoped<IOrganizationCrmSearchService, OrganizationCrmSearchService>();
        serviceCollections.AddScoped<IOrganisationChangeRequestRepository, OrganisationChangeRequestRepository>();
        serviceCollections.AddScoped<IContactService, ContactService>();
        serviceCollections.AddScoped<IContactRepository, ContactRepository>();
        serviceCollections.AddScoped<IWebRoleRepository, WebRoleRepository>();
        serviceCollections.AddScoped<IPortalPermissionRepository, PortalPermissionRepository>();
        serviceCollections.AddScoped<IOrganizationService, OrganizationService>();
    }
}
