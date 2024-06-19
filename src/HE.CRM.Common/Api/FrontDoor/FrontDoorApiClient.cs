using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.Api.FrontDoor.Contract.Requests;
using HE.CRM.Common.Api.FrontDoor.Contract.Responses;
using HE.CRM.Common.Api.FrontDoor.Mappers;

namespace HE.CRM.Common.Api.FrontDoor
{
    public sealed class FrontDoorApiClient : IFrontDoorApiClient
    {
        private readonly IApiHttpClient _httpClient;

        public FrontDoorApiClient(IApiHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> CheckProjectExists(Guid organisationId, string projectName, CancellationToken cancellationToken)
        {
            var request = new CheckProjectExistsRequest
            {
                PartnerRecordId = organisationId,
                ProjectName = projectName,
            };

            var response = await _httpClient.SendAsync<CheckProjectExistsRequest, CheckProjectExistsResponse>(
                request,
                FrontDoorApiUrls.CheckProjectExists,
                HttpMethod.Post,
                cancellationToken);

            return response.Result;
        }

        public async Task DeactivateProject(Guid projectId, CancellationToken cancellationToken)
        {
            var request = new DeactivateProjectRequest { ProjectRecordId = projectId };

            await _httpClient.SendAsync<DeactivateProjectRequest, DeactivateProjectResponse>(
                request,
                FrontDoorApiUrls.DeactivateProject,
                HttpMethod.Post,
                cancellationToken);
        }

        public async Task RemoveSite(Guid siteId, CancellationToken cancellationToken)
        {
            var request = new RemoveSiteRequest { ProjectSiteRecordId = siteId };

            await _httpClient.SendAsync<RemoveSiteRequest, RemoveSiteResponse>(
                request,
                FrontDoorApiUrls.RemoveSite,
                HttpMethod.Post,
                cancellationToken);
        }

        public async Task<IList<FrontDoorProjectDto>> GetProjects(Guid organisationId, CancellationToken cancellationToken)
        {
            var response = await _httpClient.SendAsync<GetProjectsResponse>(
                FrontDoorApiUrls.GetProjects(organisationId),
                HttpMethod.Get,
                cancellationToken);

            return response.Select(GetProjectResponseMapper.Map).ToList();
        }

        public async Task<IList<FrontDoorProjectSiteDto>> GetSites(Guid projectId, CancellationToken cancellationToken)
        {
            var response = await _httpClient.SendAsync<GetMultipleSitesResponse>(
                FrontDoorApiUrls.GetSites(projectId),
                HttpMethod.Get,
                cancellationToken);

            return response.Select(GetSiteResponseMapper.Map).ToList();
        }

        public async Task<FrontDoorProjectDto> GetProject(Guid projectId, CancellationToken cancellationToken)
        {
            var response = await _httpClient.SendAsync<GetProjectResponse>(
                FrontDoorApiUrls.GetProject(projectId),
                HttpMethod.Get,
                cancellationToken);

            return GetProjectResponseMapper.Map(response);
        }

        public async Task<FrontDoorProjectSiteDto> GetSite(Guid siteId, CancellationToken cancellationToken)
        {
            var response = await _httpClient.SendAsync<GetSiteResponse>(
                FrontDoorApiUrls.GetSite(siteId),
                HttpMethod.Get,
                cancellationToken);

            return GetSiteResponseMapper.Map(response);
        }

        public async Task<Guid> SaveProject(FrontDoorProjectDto dto, Guid userId, CancellationToken cancellationToken)
        {
            var request = SaveProjectRequestMapper.Map(dto, userId);
            var response = await _httpClient.SendAsync<SaveProjectRequest, SaveProjectResponse>(
                request,
                FrontDoorApiUrls.SaveProject,
                HttpMethod.Post,
                cancellationToken);

            return Guid.Parse(response.Result);
        }

        public async Task<Guid> SaveSite(FrontDoorProjectSiteDto dto, Guid projectId, CancellationToken cancellationToken)
        {
            var request = SaveSiteRequestMapper.Map(dto, projectId);
            var response = await _httpClient.SendAsync<SaveSiteRequest, SaveSiteResponse>(
                request,
                FrontDoorApiUrls.SaveSite,
                HttpMethod.Post,
                cancellationToken);

            return Guid.Parse(response.Result);
        }
    }
}