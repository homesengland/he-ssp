using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.DtoMapping;
using HE.CRM.Common.Repositories.interfaces;
using HE.CRM.Common.Repositories.Interfaces;
using HE.CRM.Model.CrmSerializedParameters;
using HE.CRM.Plugins.Services.FrontDoorProject;
using HE.CRM.Plugins.Services.GovNotifyEmail;
using HE.CRM.Plugins.Services.LoanApplication;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Policy;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Xml.Linq;

namespace HE.CRM.Plugins.Services.FrontDoorProjectSite
{
    public class FrontDoorProjectSiteService : CrmService, IFrontDoorProjectSiteService
    {
        #region Fields
        private readonly IFrontDoorProjectSiteRepository _frontDoorProjectSiteRepository;
        private readonly ILocalAuthorityRepository _localAuthorityRepository;
        #endregion

        #region Constructors
        public FrontDoorProjectSiteService(CrmServiceArgs args) : base(args)
        {
            _frontDoorProjectSiteRepository = CrmRepositoriesFactory.Get<IFrontDoorProjectSiteRepository>();
            _localAuthorityRepository = CrmRepositoriesFactory.Get<ILocalAuthorityRepository>();
        }
        #endregion


        public PagedResponseDto<FrontDoorProjectSiteDto> GetFrontDoorProjectSites(PagingRequestDto pagingRequestDto, string frontDoorProjectId, string fieldsToRetrieve = null)
        {
            this.TracingService.Trace("GetFrontDoorProjectSites");
            var frontDoorProjectIdCondition = GetFetchXmlConditionForGivenField(frontDoorProjectId, nameof(invln_FrontDoorProjectSitePOC.invln_FrontDoorProjectId).ToLower());
            var attributes = GenerateFetchXmlAttributes(fieldsToRetrieve);

            var frontDoorProjectPaging = _frontDoorProjectSiteRepository.GetFrontDoorProjectSites(pagingRequestDto, frontDoorProjectIdCondition, attributes);

            List<FrontDoorProjectSiteDto> frontDoorProjectSiteDtoList = new List<FrontDoorProjectSiteDto>();
            foreach (var siteFromCrm in frontDoorProjectPaging.items)
            {
                invln_localauthority localauthority = new invln_localauthority();
                if (siteFromCrm.invln_LocalAuthorityId != null)
                {
                    localauthority = _localAuthorityRepository.GetById(siteFromCrm.invln_LocalAuthorityId.Id, new string[] { nameof(invln_localauthority.invln_localauthorityId).ToLower(), nameof(invln_localauthority.invln_localauthorityname).ToLower(), nameof(invln_localauthority.invln_onscode).ToLower() });
                }
                var frontDoorProjectSiteDto = FrontDoorProjectSiteMapper.MapFrontDoorProjectSiteToDto(siteFromCrm, localauthority);
                frontDoorProjectSiteDtoList.Add(frontDoorProjectSiteDto);
            }

            return new PagedResponseDto<FrontDoorProjectSiteDto>
            {
                paging = frontDoorProjectPaging.paging,
                totalItemsCount = frontDoorProjectPaging.totalItemsCount,
                items = frontDoorProjectSiteDtoList,
            };
        }

        public FrontDoorProjectSiteDto GetFrontDoorProjectSite(string frontDoorProjectId, string fieldsToRetrieve = null, string frontDoorProjectSiteId = null)
        {
            this.TracingService.Trace("GetFrontDoorProjectSites");

            var frontDoorProjectIdCondition = GetFetchXmlConditionForGivenField(frontDoorProjectId, nameof(invln_FrontDoorProjectSitePOC.invln_FrontDoorProjectId).ToLower());
            var attributes = GenerateFetchXmlAttributes(fieldsToRetrieve);
            var frontDoorProjectSiteIdCondition = GetFetchXmlConditionForGivenField(frontDoorProjectSiteId, nameof(invln_FrontDoorProjectSitePOC.invln_FrontDoorProjectSitePOCId).ToLower());

            var frontDoorProjectSite = _frontDoorProjectSiteRepository.GetFrontDoorProjectSite(frontDoorProjectIdCondition, frontDoorProjectSiteIdCondition, attributes);

            if (frontDoorProjectSite != null)
            {
                invln_localauthority localauthority = new invln_localauthority();
                if (frontDoorProjectSite.invln_LocalAuthorityId != null)
                {
                    localauthority = _localAuthorityRepository.GetById(frontDoorProjectSite.invln_LocalAuthorityId.Id, new string[] { nameof(invln_localauthority.invln_localauthorityId).ToLower(), nameof(invln_localauthority.invln_localauthorityname).ToLower(), nameof(invln_localauthority.invln_onscode).ToLower() });
                }

                return FrontDoorProjectSiteMapper.MapFrontDoorProjectSiteToDto(frontDoorProjectSite, localauthority);
            }
            else
            {
                return null;
            }
        }

        public string CreateRecordFromPortal(string frontDoorProjectId, string entityFieldsParameters, string frontDoorSiteId = null)
        {
            Guid frontDoorSiteGUID = Guid.NewGuid();
            this.TracingService.Trace("frontDoorProjectId:" + frontDoorProjectId);
            this.TracingService.Trace("entityFieldsParameters:" + entityFieldsParameters);

            FrontDoorProjectSiteDto frontDoorSiteFromPortal = JsonSerializer.Deserialize<FrontDoorProjectSiteDto>(entityFieldsParameters);
            if (frontDoorSiteFromPortal.LocalAuthorityCode != null)
            {
                frontDoorSiteFromPortal.LocalAuthority = _localAuthorityRepository.GetLocalAuthorityWithGivenOnsCode(frontDoorSiteFromPortal.LocalAuthorityCode)?.Id.ToString();
            }

            var frontDoorSiteToCreate = FrontDoorProjectSiteMapper.MapFrontDoorProjectSiteDtoToRegularEntity(frontDoorSiteFromPortal, frontDoorProjectId);

            if (!string.IsNullOrEmpty(frontDoorSiteId) && Guid.TryParse(frontDoorSiteId, out Guid siteId))
            {
                this.TracingService.Trace("Update FrontDoorProjectSitePOC");
                frontDoorSiteGUID = siteId;
                frontDoorSiteToCreate.Id = siteId;
                _frontDoorProjectSiteRepository.Update(frontDoorSiteToCreate);
                this.TracingService.Trace("After update record");
            }
            else
            {
                this.TracingService.Trace("Create FrontDoorProjectSitePOC");
                frontDoorSiteGUID = _frontDoorProjectSiteRepository.Create(frontDoorSiteToCreate);
                this.TracingService.Trace("After create record");
            }

            return frontDoorSiteGUID.ToString();
        }

        public bool DeactivateFrontDoorSite(string frontDoorSiteId)
        {
            var frontDoorSite = _frontDoorProjectSiteRepository.GetById(new Guid(frontDoorSiteId), new string[] { nameof(invln_FrontDoorProjectSitePOC.invln_FrontDoorProjectSitePOCId).ToLower() });
            _frontDoorProjectSiteRepository.SetState(frontDoorSite, invln_FrontDoorProjectSitePOCState.Inactive, invln_FrontDoorProjectSitePOC_StatusCode.Inactive);
            var frontDoorSiteAfter = _frontDoorProjectSiteRepository.GetById(new Guid(frontDoorSiteId), new string[] { nameof(invln_FrontDoorProjectSitePOC.StateCode).ToLower() });
            return frontDoorSiteAfter.StateCode.Value == (int)invln_FrontDoorProjectSitePOCState.Inactive;
        }

        private string GenerateFetchXmlAttributes(string fieldsToRetrieve)
        {
            if (!string.IsNullOrEmpty(fieldsToRetrieve))
            {
                var fields = fieldsToRetrieve.Split(',');
                var generatedAttribuesFetchXml = "";
                if (fields.Length > 0)
                {
                    foreach (var field in fields)
                    {
                        generatedAttribuesFetchXml += $"<attribute name=\"{field}\" />";
                    }
                }
                return generatedAttribuesFetchXml;
            }
            return null;
        }

        private string GetFetchXmlConditionForGivenField(string fieldValue, string fieldName)
        {
            if (!string.IsNullOrEmpty(fieldValue))
            {
                return $"<condition attribute=\"{fieldName}\" operator=\"eq\" value=\"{fieldValue}\" />";
            }
            return string.Empty;
        }

    }
}
