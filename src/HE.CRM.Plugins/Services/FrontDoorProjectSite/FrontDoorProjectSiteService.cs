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
using System.Runtime.InteropServices;
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

        private readonly IHeProjectLocalAuthorityRepository _heProjectLocalAuthorityRepository;
        private readonly IHeLocalAuthorityRepository _heLocalAuthorityRepository;
        #endregion

        #region Constructors
        public FrontDoorProjectSiteService(CrmServiceArgs args) : base(args)
        {
            _frontDoorProjectSiteRepository = CrmRepositoriesFactory.Get<IFrontDoorProjectSiteRepository>();
            _localAuthorityRepository = CrmRepositoriesFactory.Get<ILocalAuthorityRepository>();

            _heProjectLocalAuthorityRepository = CrmRepositoriesFactory.Get<IHeProjectLocalAuthorityRepository>();
            _heLocalAuthorityRepository = CrmRepositoriesFactory.Get<IHeLocalAuthorityRepository>();
        }
        #endregion


        public PagedResponseDto<FrontDoorProjectSiteDto> GetFrontDoorProjectSites(PagingRequestDto pagingRequestDto, string frontDoorProjectId, bool useHeTables, string fieldsToRetrieve = null)
        {
            this.TracingService.Trace($"GetFrontDoorProjectSites for useHeTables= {useHeTables}");

            if (useHeTables)
            {
                var frontDoorProjectIdCondition = GetFetchXmlConditionForGivenField(frontDoorProjectId, nameof(he_ProjectLocalAuthority.he_Project).ToLower());

                var frontDoorProjectPaging = _heProjectLocalAuthorityRepository.HeGetFrontDoorProjectSites(pagingRequestDto, frontDoorProjectIdCondition);

                List<FrontDoorProjectSiteDto> frontDoorProjectSiteDtoList = new List<FrontDoorProjectSiteDto>();
                foreach (var siteFromCrm in frontDoorProjectPaging.items)
                {
                    he_LocalAuthority localauthority = new he_LocalAuthority();
                    if (siteFromCrm.he_LocalAuthority!= null)
                    {
                        localauthority = _heLocalAuthorityRepository.GetById(siteFromCrm.he_LocalAuthority.Id, new string[] { nameof(he_LocalAuthority.he_LocalAuthorityId).ToLower(), nameof(he_LocalAuthority.he_Name).ToLower(), nameof(he_LocalAuthority.he_GSSCode).ToLower() });
                    }
                    var frontDoorProjectSiteDto = FrontDoorProjectSiteMapper.MapHeFrontDoorProjectSiteToDto(siteFromCrm, localauthority);
                    frontDoorProjectSiteDtoList.Add(frontDoorProjectSiteDto);
                }

                return new PagedResponseDto<FrontDoorProjectSiteDto>
                {
                    paging = frontDoorProjectPaging.paging,
                    totalItemsCount = frontDoorProjectPaging.totalItemsCount,
                    items = frontDoorProjectSiteDtoList,
                };
            }
            else
            {
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

        }

        public FrontDoorProjectSiteDto GetFrontDoorProjectSite(string frontDoorProjectId, bool useHeTables, string fieldsToRetrieve = null, string frontDoorProjectSiteId = null)
        {
            this.TracingService.Trace($"GetFrontDoorProjectSite for useHeTables= {useHeTables}");

            if (useHeTables)
            {
                var frontDoorProjectIdCondition = GetFetchXmlConditionForGivenField(frontDoorProjectId, nameof(he_ProjectLocalAuthority.he_Project).ToLower());
                var frontDoorProjectSiteIdCondition = GetFetchXmlConditionForGivenField(frontDoorProjectSiteId, nameof(he_ProjectLocalAuthority.he_ProjectLocalAuthorityId).ToLower());

                var frontDoorProjectSite = _heProjectLocalAuthorityRepository.HeGetFrontDoorProjectSite(frontDoorProjectIdCondition, frontDoorProjectSiteIdCondition);

                if (frontDoorProjectSite != null)
                {
                    he_LocalAuthority localauthority = new he_LocalAuthority();
                    if (frontDoorProjectSite.he_LocalAuthority != null)
                    {
                        localauthority = _heLocalAuthorityRepository.GetById(frontDoorProjectSite.he_LocalAuthority.Id, new string[] { nameof(he_LocalAuthority.he_LocalAuthorityId).ToLower(), nameof(he_LocalAuthority.he_Name).ToLower(), nameof(he_LocalAuthority.he_GSSCode).ToLower() });
                    }

                    return FrontDoorProjectSiteMapper.MapHeFrontDoorProjectSiteToDto(frontDoorProjectSite, localauthority);
                }
                else
                {
                    return null;
                }
            }
            else
            {
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

        }

        public string CreateRecordFromPortal(string frontDoorProjectId, string entityFieldsParameters, bool useHeTables, string frontDoorSiteId = null)
        {
            Guid frontDoorSiteGUID = Guid.NewGuid();
            this.TracingService.Trace($"frontDoorProjectId: {frontDoorProjectId}");
            this.TracingService.Trace($"entityFieldsParameters: {entityFieldsParameters}");
            this.TracingService.Trace($"useHeTables: {useHeTables}");

            FrontDoorProjectSiteDto frontDoorSiteFromPortal = JsonSerializer.Deserialize<FrontDoorProjectSiteDto>(entityFieldsParameters);

            if (useHeTables)
            {
                if (frontDoorSiteFromPortal.LocalAuthorityCode != null)
                {
                    var localAuthorityGUID = _heLocalAuthorityRepository.GetLocalAuthorityWithGivenCode(frontDoorSiteFromPortal.LocalAuthorityCode)?.Id;
                    if (localAuthorityGUID != null)
                    {
                        frontDoorSiteFromPortal.LocalAuthority = localAuthorityGUID.ToString();
                    }
                    else
                    {
                        frontDoorSiteFromPortal.LocalAuthority = null;
                    }
                }
                else
                {
                    frontDoorSiteFromPortal.LocalAuthority = null;
                }

                var frontDoorSiteToCreate = FrontDoorProjectSiteMapper.MapHeFrontDoorProjectSiteDtoToRegularEntity(frontDoorSiteFromPortal, frontDoorProjectId);


                if (!string.IsNullOrEmpty(frontDoorSiteId) && Guid.TryParse(frontDoorSiteId, out Guid siteId))
                {
                    this.TracingService.Trace("Update ProjectLocalAuthority");
                    frontDoorSiteGUID = siteId;
                    frontDoorSiteToCreate.Id = siteId;
                    _heProjectLocalAuthorityRepository.Update(frontDoorSiteToCreate);
                    this.TracingService.Trace("After update record");
                }
                else
                {
                    this.TracingService.Trace("Create ProjectLocalAuthority");
                    frontDoorSiteGUID = _heProjectLocalAuthorityRepository.Create(frontDoorSiteToCreate);
                    this.TracingService.Trace("After create record");
                }

            }
            else
            {
                if (frontDoorSiteFromPortal.LocalAuthorityCode != null)
                {
                    var localAuthorityGUID = _localAuthorityRepository.GetLocalAuthorityWithGivenOnsCode(frontDoorSiteFromPortal.LocalAuthorityCode)?.Id;
                    if (localAuthorityGUID != null)
                    {
                        frontDoorSiteFromPortal.LocalAuthority = localAuthorityGUID.ToString();
                    }
                    else
                    {
                        frontDoorSiteFromPortal.LocalAuthority = null;
                    }
                }
                else
                {
                    frontDoorSiteFromPortal.LocalAuthority = null;
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
            }

            return frontDoorSiteGUID.ToString();
        }

        public bool DeactivateFrontDoorSite(string frontDoorSiteId, bool useHeTables)
        {
            if (useHeTables)
            {
                var frontDoorSite = _heProjectLocalAuthorityRepository.GetById(new Guid(frontDoorSiteId), new string[] { nameof(he_ProjectLocalAuthority.he_ProjectLocalAuthorityId).ToLower() });
                _heProjectLocalAuthorityRepository.SetState(frontDoorSite, he_ProjectLocalAuthorityState.Inactive, he_ProjectLocalAuthority_StatusCode.Inactive);
                var frontDoorSiteAfter = _heProjectLocalAuthorityRepository.GetById(new Guid(frontDoorSiteId), new string[] { nameof(he_ProjectLocalAuthority.StateCode).ToLower() });
                return frontDoorSiteAfter.StateCode.Value == (int)he_ProjectLocalAuthorityState.Inactive;
            }
            else
            {
                var frontDoorSite = _frontDoorProjectSiteRepository.GetById(new Guid(frontDoorSiteId), new string[] { nameof(invln_FrontDoorProjectSitePOC.invln_FrontDoorProjectSitePOCId).ToLower() });
                _frontDoorProjectSiteRepository.SetState(frontDoorSite, invln_FrontDoorProjectSitePOCState.Inactive, invln_FrontDoorProjectSitePOC_StatusCode.Inactive);
                var frontDoorSiteAfter = _frontDoorProjectSiteRepository.GetById(new Guid(frontDoorSiteId), new string[] { nameof(invln_FrontDoorProjectSitePOC.StateCode).ToLower() });
                return frontDoorSiteAfter.StateCode.Value == (int)invln_FrontDoorProjectSitePOCState.Inactive;
            }
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
