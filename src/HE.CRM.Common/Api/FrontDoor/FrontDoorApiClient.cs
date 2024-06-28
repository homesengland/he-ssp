using System;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Threading;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.Api.Auth;
using HE.CRM.Common.Api.Auth.AzureAd;
using HE.CRM.Common.Api.Exceptions;
using HE.CRM.Common.Api.FrontDoor.Contract.Requests;
using HE.CRM.Common.Api.FrontDoor.Contract.Responses;
using HE.CRM.Common.Api.FrontDoor.Mappers;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Common.Api.FrontDoor
{
    public sealed class FrontDoorApiClient : CrmService, IFrontDoorApiClient
    {
        private readonly ApiHttpClient _httpClient;

        public FrontDoorApiClient(CrmServiceArgs args)
            : base(args)
        {
            _httpClient = CreateHttpClient(
                CrmRepositoriesFactory.Get<ISecretVariableRepository>(),
                CrmServicesFactory.Get<IAzureAdTokenProviderFactory>());
        }

        public bool CheckProjectExists(Guid organisationId, string projectName)
        {
            Logger.Trace("FrontDoorApiClient.CheckProjectExists");

            var request = new CheckProjectExistsRequest
            {
                PartnerRecordId = organisationId,
                ProjectName = projectName,
            };

            var response = _httpClient.Send<CheckProjectExistsRequest, CheckProjectExistsResponse>(
                request,
                FrontDoorApiUrls.CheckProjectExists,
                HttpMethod.Post);

            return response.Result;
        }

        public string DeactivateProject(Guid projectId)
        {
            Logger.Trace("FrontDoorApiClient.DeactivateProject");

            var request = new DeactivateProjectRequest { ProjectRecordId = projectId };

            var response = _httpClient.Send<DeactivateProjectRequest, DeactivateProjectResponse>(
                request,
                FrontDoorApiUrls.DeactivateProject,
                HttpMethod.Post);

            return response.Result;
        }

        public void RemoveSite(Guid siteId)
        {
            Logger.Trace("FrontDoorApiClient.RemoveSite");

            var request = new RemoveSiteRequest { ProjectSiteRecordId = siteId };

            _httpClient.Send<RemoveSiteRequest, RemoveSiteResponse>(
                request,
                FrontDoorApiUrls.RemoveSite,
                HttpMethod.Post);
        }

        public GetProjectsResponse GetProjects(Guid organisationId)
        {
            Logger.Trace("FrontDoorApiClient.GetProjects");

            try
            {
                var baseResponse = _httpClient.Send<GetProjectsResponse>(
                    FrontDoorApiUrls.GetProjects(organisationId),
                    HttpMethod.Get);

                return baseResponse;
            }
            catch (ApiException apiEx)
            {
                throw new InvalidPluginExecutionException(apiEx.Message);
            }
        }

        public GetMultipleSitesResponse GetSites(Guid projectId)
        {
            Logger.Trace("FrontDoorApiClient.GetSites");

            try
            {
                var response = _httpClient.Send<GetMultipleSitesResponse>(
                FrontDoorApiUrls.GetSites(projectId),
                HttpMethod.Get);

                return response;
            }
            catch (ApiException apiEx)
            {
                throw new InvalidPluginExecutionException(apiEx.Message);
            }
        }

        public GetProjectResponse GetProject(Guid projectId)
        {
            Logger.Trace("FrontDoorApiClient.GetProject");

            try
            {
                var response = _httpClient.Send<GetProjectResponse>(
                FrontDoorApiUrls.GetProject(projectId),
                HttpMethod.Get);

                return response;
            }
            catch (ApiException apiEx)
            {
                throw new InvalidPluginExecutionException(apiEx.Message);
            }
        }

        public GetSiteResponse GetSite(Guid siteId)
        {
            Logger.Trace("FrontDoorApiClient.GetSite");

            try
            {
                var response = _httpClient.Send<GetSiteResponse>(
                        FrontDoorApiUrls.GetSite(siteId),
                        HttpMethod.Get);

                return response;
            }
            catch (ApiException apiEx)
            {
                throw new InvalidPluginExecutionException(apiEx.Message);
            }
        }

        public SaveProjectResponse SaveProject(FrontDoorProjectDto dto, Guid userId)
        {
            Logger.Trace("FrontDoorApiClient.SaveProject");
            try
            {
                var request = SaveProjectRequestMapper.Map(dto, userId);
                var response = _httpClient.Send<SaveProjectRequest, SaveProjectResponse>(
                    request,
                    FrontDoorApiUrls.SaveProject,
                    HttpMethod.Post);

                return response;
            }
            catch (ApiException apiEx)
            {
                throw new InvalidPluginExecutionException(apiEx.Message);
            }
        }

        public SaveSiteResponse SaveSite(FrontDoorProjectSiteDto dto, Guid projectId)
        {
            Logger.Trace("FrontDoorApiClient.SaveSite");
            try
            {
                var request = SaveSiteRequestMapper.Map(dto, projectId);
                var response = _httpClient.Send<SaveSiteRequest, SaveSiteResponse>(
                    request,
                    FrontDoorApiUrls.SaveSite,
                    HttpMethod.Post);

                return response;
            }
            catch (ApiException apiEx)
            {
                throw new InvalidPluginExecutionException(apiEx.Message);
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        private static ApiHttpClient CreateHttpClient(
            ISecretVariableRepository secretVariableRepository,
            IAzureAdTokenProviderFactory tokenProviderFactory)
        {
            var secrets = secretVariableRepository.GetMultiple(
                EnvironmentVariables.FrontDoorApiBaseUrl,
                EnvironmentVariables.AzureAd.TenantId,
                EnvironmentVariables.AzureAd.ClientId,
                EnvironmentVariables.AzureAd.ClientSecret,
                EnvironmentVariables.AzureAd.Scope
            );

            var azureAdAuthConfig = new AzureAdAuthConfig
            {
                TenantId = secrets.First(x => x.invln_Name == EnvironmentVariables.AzureAd.TenantId).invln_Value,
                ClientId = secrets.First(x => x.invln_Name == EnvironmentVariables.AzureAd.ClientId).invln_Value,
                ClientSecret = secrets.First(x => x.invln_Name == EnvironmentVariables.AzureAd.ClientSecret).invln_Value,
                Scope = secrets.First(x => x.invln_Name == EnvironmentVariables.AzureAd.Scope).invln_Value,
            };

            var tokenProvider = tokenProviderFactory.Create(azureAdAuthConfig);
            var frontDoorApiUrl = secrets.First(x => x.invln_Name == EnvironmentVariables.FrontDoorApiBaseUrl).invln_Value;
            var httpClient = new HttpClient(new BearerTokenAuthorizationHandler(tokenProvider))
            {
                BaseAddress = new Uri(frontDoorApiUrl)
            };

            return new ApiHttpClient(httpClient);
        }
    }
}
