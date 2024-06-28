using System;
using System.Linq;
using System.Text.Json;
using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.Api.FrontDoor;
using HE.CRM.Common.Api.FrontDoor.Mappers;
using HE.CRM.Common.Repositories.Interfaces;

namespace HE.CRM.Plugins.Services.FrontDoorProjectSite.V2
{
    public class FrontDoorProjectSiteService : CrmService, IFrontDoorProjectSiteService
    {
        #region Fields
        private readonly IFrontDoorApiClient _frontDoorApiClient;
        private readonly IFrontDoorProjectSiteRepository _frontDoorProjectSiteRepository;
        private readonly ILocalAuthorityRepository _localAuthorityRepository;

        private readonly IHeProjectLocalAuthorityRepository _heProjectLocalAuthorityRepository;
        private readonly IHeLocalAuthorityRepository _heLocalAuthorityRepository;
        #endregion

        #region Constructors
        public FrontDoorProjectSiteService(CrmServiceArgs args) : base(args)
        {
            _frontDoorApiClient = CrmServicesFactory.Get<IFrontDoorApiClient>();

            _frontDoorProjectSiteRepository = CrmRepositoriesFactory.Get<IFrontDoorProjectSiteRepository>();
            _localAuthorityRepository = CrmRepositoriesFactory.Get<ILocalAuthorityRepository>();
            _heProjectLocalAuthorityRepository = CrmRepositoriesFactory.Get<IHeProjectLocalAuthorityRepository>();
            _heLocalAuthorityRepository = CrmRepositoriesFactory.Get<IHeLocalAuthorityRepository>();
        }
        #endregion


        public PagedResponseDto<FrontDoorProjectSiteDto> GetFrontDoorProjectSites(PagingRequestDto pagingRequestDto, Guid frontDoorProjectId, string fieldsToRetrieve = null)
        {
            Logger.Trace($"FrontDoorProjectSite.V2.{nameof(GetFrontDoorProjectSites)}");

            var sites = _frontDoorApiClient.GetSites(frontDoorProjectId);

            //  implement paging ? No

            var sitesDto = sites.Select(x => GetSiteResponseMapper.Map(x)).ToList();

            return new PagedResponseDto<FrontDoorProjectSiteDto>
            {
                paging = new PagingRequestDto() { pageNumber = 1, pageSize = 100 },
                totalItemsCount = 100,
                items = sitesDto
            };
        }

        public FrontDoorProjectSiteDto GetFrontDoorProjectSite(Guid frontDoorProjectId, Guid frontDoorProjectSiteId)
        {
            Logger.Trace($"FrontDoorProjectSite.V2.{nameof(GetFrontDoorProjectSite)}");

            var site = _frontDoorApiClient.GetSite(frontDoorProjectSiteId);
            //if (site.ProjectRecordId != frontDoorProjectId)
            //{
            //    return null;
            //};
            return GetSiteResponseMapper.Map(site);
        }

        public string CreateRecordFromPortal(Guid frontDoorProjectId, string entityFieldsParameters, string frontDoorSiteId = null)
        {
            Logger.Trace($"FrontDoorProjectSite.V2.{nameof(CreateRecordFromPortal)}");

            var frontDoorSiteGUID = Guid.NewGuid();

            var frontDoorSiteFromPortal = JsonSerializer.Deserialize<FrontDoorProjectSiteDto>(
                entityFieldsParameters,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var response = _frontDoorApiClient.SaveSite(frontDoorSiteFromPortal, frontDoorProjectId);
            return response.Result;
        }

        public bool DeactivateFrontDoorSite(Guid frontDoorSiteId)
        {
            Logger.Trace($"FrontDoorProjectSite.V2.{nameof(DeactivateFrontDoorSite)}");

            _frontDoorApiClient.RemoveSite(frontDoorSiteId);

            return true;
        }
    }
}
