using System;
using System.Linq;
using System.Net.Http;
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
        private static readonly TimeSpan TimeOutParameter = TimeSpan.FromSeconds(20);
        private readonly Lazy<ApiHttpClient> _httpClient;

        public FrontDoorApiClient(CrmServiceArgs args)
            : base(args)
        {
            _httpClient = new Lazy<ApiHttpClient>(() => CreateHttpClient(
                CrmRepositoriesFactory.Get<ISecretVariableRepository>(),
                CrmServicesFactory.Get<IAzureAdTokenProviderFactory>()));
        }

        public bool CheckProjectExists(Guid organisationId, string projectName)
        {
            Logger.Trace("FrontDoorApiClient.CheckProjectExists");

            var request = new CheckProjectExistsRequest
            {
                PartnerRecordId = organisationId,
                ProjectName = projectName,
            };

            try
            {
                var response = _httpClient.Value.Send<CheckProjectExistsRequest, CheckProjectExistsResponse>(
                    request,
                    FrontDoorApiUrls.CheckProjectExists,
                    HttpMethod.Post);

                return response.Result;
            }
            catch (ApiException ex)
            {
                HandleApiException(ex);
            }
            return false;
        }

        public string DeactivateProject(Guid projectId)
        {
            Logger.Trace("FrontDoorApiClient.DeactivateProject");

            var request = new DeactivateProjectRequest { ProjectRecordId = projectId };

            try
            {
                var response = _httpClient.Value.Send<DeactivateProjectRequest, DeactivateProjectResponse>(
                    request,
                    FrontDoorApiUrls.DeactivateProject,
                    HttpMethod.Post);

                return response.Result;
            }
            catch (ApiException ex)
            {
                HandleApiException(ex);
            }
            return null;
        }

        public void RemoveSite(Guid siteId)
        {
            Logger.Trace("FrontDoorApiClient.RemoveSite");

            var request = new RemoveSiteRequest { ProjectSiteRecordId = siteId };

            try
            {
                _httpClient.Value.Send<RemoveSiteRequest, RemoveSiteResponse>(
                    request,
                    FrontDoorApiUrls.RemoveSite,
                    HttpMethod.Post);
            }
            catch (ApiException ex)
            {
                HandleApiException(ex);
            }
        }

        public GetProjectsResponse GetProjects(Guid organisationId)
        {
            Logger.Trace("FrontDoorApiClient.GetProjects");

            try
            {
                var baseResponse = _httpClient.Value.Send<GetProjectsResponse>(
                    FrontDoorApiUrls.GetProjects(organisationId),
                    HttpMethod.Get);

                return baseResponse;
            }
            catch (ApiException ex)
            {
                HandleApiException(ex);
            }
            return null;
        }

        public GetMultipleSitesResponse GetSites(Guid projectId)
        {
            Logger.Trace("FrontDoorApiClient.GetSites");

            try
            {
                var response = _httpClient.Value.Send<GetMultipleSitesResponse>(
                FrontDoorApiUrls.GetSites(projectId),
                HttpMethod.Get);

                return response;
            }
            catch (ApiException ex)
            {
                HandleApiException(ex);
            }
            return null;
        }

        public GetProjectResponse GetProject(Guid projectId)
        {
            Logger.Trace("FrontDoorApiClient.GetProject");

            try
            {
                var response = _httpClient.Value.Send<GetProjectResponse>(
                FrontDoorApiUrls.GetProject(projectId),
                HttpMethod.Get);

                return response;
            }
            catch (ApiException ex)
            {
                HandleApiException(ex);
            }
            return null;
        }

        public GetSiteResponse GetSite(Guid siteId)
        {
            Logger.Trace("FrontDoorApiClient.GetSite");

            try
            {
                var response = _httpClient.Value.Send<GetSiteResponse>(
                        FrontDoorApiUrls.GetSite(siteId),
                        HttpMethod.Get);

                return response;
            }
            catch (ApiException ex)
            {
                HandleApiException(ex);
            }
            return null;
        }

        public SaveProjectResponse SaveProject(FrontDoorProjectDto dto, Guid userId)
        {
            Logger.Trace("FrontDoorApiClient.SaveProject");

            try
            {
                var request = SaveProjectRequestMapper.Map(dto, userId);
                var response = _httpClient.Value.Send<SaveProjectRequest, SaveProjectResponse>(
                    request,
                    FrontDoorApiUrls.SaveProject,
                    HttpMethod.Post);

                return response;
            }
            catch (ApiException ex)
            {
                HandleApiException(ex);
            }
            return null;
        }

        public SaveSiteResponse SaveSite(FrontDoorProjectSiteDto dto, Guid projectId)
        {
            Logger.Trace("FrontDoorApiClient.SaveSite");

            try
            {
                var request = SaveSiteRequestMapper.Map(dto, projectId);
                var response = _httpClient.Value.Send<SaveSiteRequest, SaveSiteResponse>(
                    request,
                    FrontDoorApiUrls.SaveSite,
                    HttpMethod.Post);

                return response;
            }
            catch (ApiException ex)
            {
                HandleApiException(ex);
            }
            return null;
        }

        public void Dispose()
        {
            _httpClient?.Value?.Dispose();
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
                BaseAddress = new Uri(frontDoorApiUrl),
                Timeout = TimeOutParameter,
            };
            httpClient.DefaultRequestHeaders.ConnectionClose = true; //Set KeepAlive to false

            return new ApiHttpClient(httpClient);
        }

        private void HandleApiException(ApiException ex)
        {
            if (ex is ApiCommunicationException exception)
            {
                var message = ex.Message;
                var errorContent = exception.ErrorContent;

                if (!string.IsNullOrEmpty(errorContent))
                {
                    message += Environment.NewLine + $"  --> {errorContent}";
                }
                Logger.Warn(message);
                throw new InvalidPluginExecutionException(ex.Message, PluginHttpStatusCode.RequestTimeout);
            }
            else if (ex is ApiSerializationException serializationException)
            {
                var message = ex.Message;
                var errorContent = serializationException.ResponseContent;

                if (!string.IsNullOrEmpty(errorContent))
                {
                    message += Environment.NewLine + $"  --> Response content: {errorContent}";
                }
                Logger.Warn(message);
                throw new InvalidPluginExecutionException(serializationException.ToString(), PluginHttpStatusCode.InternalServerError);
            }
            else if (ex is ApiException apiException)
            {
                throw new InvalidPluginExecutionException(apiException.ToString(), PluginHttpStatusCode.InternalServerError);
            }
        }
    }

}
