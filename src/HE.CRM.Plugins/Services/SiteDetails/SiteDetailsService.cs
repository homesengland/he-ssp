using System;
using System.Text.Json;
using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.DtoMapping;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Plugins.Services.SiteDetails
{
    public class SiteDetailsService : CrmService, ISiteDetailsService
    {
        #region Fields

        private readonly ISiteDetailsRepository siteDetailsRepository;
        private readonly ILoanApplicationRepository _loanApplicationRepository;
        private readonly ILocalAuthorityRepository _localAuthorityRepository;

        private readonly IHeLocalAuthorityRepository _heLocalAuthorityRepository;

        #endregion

        #region Constructors

        public SiteDetailsService(CrmServiceArgs args) : base(args)
        {
            siteDetailsRepository = CrmRepositoriesFactory.Get<ISiteDetailsRepository>();
            _loanApplicationRepository = CrmRepositoriesFactory.Get<ILoanApplicationRepository>();
            _localAuthorityRepository = CrmRepositoriesFactory.Get<ILocalAuthorityRepository>();

            _heLocalAuthorityRepository = CrmRepositoriesFactory.Get<IHeLocalAuthorityRepository>();
        }

        #endregion

        #region Public Methods
        public void UpdateSiteDetails(bool useHeTables, string siteDetailsId, string siteDetail, string fieldsToUpdate, string loanApplicationId)
        {
            this.TracingService.Trace($"UpdateSiteDetails");
            if (Guid.TryParse(siteDetailsId, out Guid detailsId))
            {
                var deserilizedSiteDetail = JsonSerializer.Deserialize<SiteDetailsDto>(siteDetail);
                he_LocalAuthority heLocalAuthority = null;
                invln_localauthority localAuthority = null;

                if (deserilizedSiteDetail.localAuthority != null)
                {
                    if (useHeTables)
                    {
                        if (!string.IsNullOrEmpty(deserilizedSiteDetail.localAuthority.onsCode))
                        {
                            heLocalAuthority = _heLocalAuthorityRepository.GetLocalAuthorityWithGivenCode(deserilizedSiteDetail.localAuthority.onsCode);
                        }
                        if (!string.IsNullOrEmpty(deserilizedSiteDetail.localAuthority.code))
                        {
                            heLocalAuthority = _heLocalAuthorityRepository.GetLocalAuthorityWithGivenCode(deserilizedSiteDetail.localAuthority.code);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(deserilizedSiteDetail.localAuthority.onsCode))
                        {
                            localAuthority = _localAuthorityRepository.GetLocalAuthorityWithGivenOnsCode(deserilizedSiteDetail.localAuthority.onsCode);
                        }
                        if (!string.IsNullOrEmpty(deserilizedSiteDetail.localAuthority.code))
                        {
                            localAuthority = _localAuthorityRepository.GetLocalAuthorityWithGivenOnsCode(deserilizedSiteDetail.localAuthority.code);
                        }
                    }
                }
                var siteDetailsMapped = SiteDetailsDtoMapper.MapSiteDetailsDtoToRegularEntity(useHeTables, deserilizedSiteDetail, loanApplicationId, localAuthority, heLocalAuthority);

                invln_SiteDetails siteDetailToUpdate = new invln_SiteDetails();
                if (string.IsNullOrEmpty(fieldsToUpdate))
                {
                    siteDetailToUpdate = siteDetailsMapped;
                }
                else
                {
                    var fields = fieldsToUpdate.Split(',');
                    if (fields.Length > 0)
                    {
                        foreach (var field in fields)
                        {
                            TracingService.Trace($"field {field}");
                            if (field.ToLower() == nameof(invln_SiteDetails.invln_LocalAuthorityID).ToLower())
                            {
                                siteDetailToUpdate.invln_LocalAuthorityID = siteDetailsMapped.invln_LocalAuthorityID;
                            }
                            else
                            {
                                siteDetailToUpdate[field] = siteDetailsMapped[field];
                            }
                        }
                    }
                }
                siteDetailToUpdate.Id = detailsId;
                siteDetailsRepository.Update(siteDetailToUpdate);
                SetLastModificationDateOnRelatedLoanApplication(siteDetailToUpdate);
            }
        }
        public void DeleteSiteDetails(string siteDetailsId)
        {
            if (Guid.TryParse(siteDetailsId, out Guid detailsId))
            {
                var siteDetailsToUpdate = new invln_SiteDetails()
                {
                    Id = detailsId,
                    StateCode = new Microsoft.Xrm.Sdk.OptionSetValue(1),
                };
                siteDetailsRepository.Update(siteDetailsToUpdate);
                SetLastModificationDateOnRelatedLoanApplication(siteDetailsToUpdate);
            }
        }

        public void CreateSiteDetail(bool useHeTables, string siteDetail, string loanApplicationId)
        {
            this.TracingService.Trace($"CreateSiteDetail");
            var deserilizedSiteDetail = JsonSerializer.Deserialize<SiteDetailsDto>(siteDetail);
            this.TracingService.Trace($"1");
            he_LocalAuthority heLocalAuthority = null;
            invln_localauthority localAuthority = null;

            if (deserilizedSiteDetail.localAuthority != null)
            {
                if (useHeTables)
                {
                    if (!string.IsNullOrEmpty(deserilizedSiteDetail.localAuthority.onsCode))
                    {
                        heLocalAuthority = _heLocalAuthorityRepository.GetLocalAuthorityWithGivenCode(deserilizedSiteDetail.localAuthority.onsCode);
                    }
                    if (!string.IsNullOrEmpty(deserilizedSiteDetail.localAuthority.code))
                    {
                        heLocalAuthority = _heLocalAuthorityRepository.GetLocalAuthorityWithGivenCode(deserilizedSiteDetail.localAuthority.code);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(deserilizedSiteDetail.localAuthority.onsCode))
                    {
                        localAuthority = _localAuthorityRepository.GetLocalAuthorityWithGivenOnsCode(deserilizedSiteDetail.localAuthority.onsCode);
                    }
                    if (!string.IsNullOrEmpty(deserilizedSiteDetail.localAuthority.code))
                    {
                        localAuthority = _localAuthorityRepository.GetLocalAuthorityWithGivenOnsCode(deserilizedSiteDetail.localAuthority.code);
                    }
                }
            }
            var siteDetailsToCreate = SiteDetailsDtoMapper.MapSiteDetailsDtoToRegularEntity(useHeTables, deserilizedSiteDetail, loanApplicationId, localAuthority, heLocalAuthority);
            siteDetailsRepository.Create(siteDetailsToCreate);

            SetLastModificationDateOnRelatedLoanApplication(siteDetailsToCreate);
        }

        public void SetLastModificationDateOnRelatedLoanApplication(invln_SiteDetails siteDetails)
        {
            if (siteDetails.invln_Loanapplication != null)
            {
                var loanApplicationToUpdate = new invln_Loanapplication()
                {
                    Id = siteDetails.invln_Loanapplication.Id,
                    invln_lastmmodificationdate = DateTime.UtcNow,
                };

                _loanApplicationRepository.Update(loanApplicationToUpdate);
            }
        }

        public string GetSingleSiteDetail(bool useHeTables, string siteDetailsId, string accountId, string contactExternalId, string fieldsToRetrieve = null)
        {
            this.TracingService.Trace($"SiteDetailsService");
            if (Guid.TryParse(siteDetailsId, out Guid siteDetailsGuid))
            {
                invln_SiteDetails retrievedSiteDetail;
                if (!string.IsNullOrEmpty(fieldsToRetrieve))
                {
                    var attributes = GenerateFetchXmlAttributes(fieldsToRetrieve);
                    retrievedSiteDetail = siteDetailsRepository.GetSiteDetailForAccountAndContact(siteDetailsGuid, accountId, contactExternalId, attributes);
                }
                else
                {
                    retrievedSiteDetail = siteDetailsRepository.GetById(siteDetailsGuid);
                }
                if (retrievedSiteDetail != null)
                {

                    he_LocalAuthority heLocalAuthority = null;
                    invln_localauthority localAuthority = null;
                    if (useHeTables)
                    {
                        if (retrievedSiteDetail.invln_HeLocalAuthorityId != null)
                        {
                            heLocalAuthority = _heLocalAuthorityRepository.GetById(retrievedSiteDetail.invln_HeLocalAuthorityId.Id);
                        }
                    }
                    else
                    {
                        if (retrievedSiteDetail.invln_LocalAuthorityID != null)
                        {
                            localAuthority = _localAuthorityRepository.GetById(retrievedSiteDetail.invln_LocalAuthorityID.Id);
                        }
                    }

                    var siteDetailsDto = SiteDetailsDtoMapper.MapSiteDetailsToDto(useHeTables, retrievedSiteDetail, localAuthority, heLocalAuthority);
                    var relatedLoan = _loanApplicationRepository.GetLoanApplicationRelatedToSiteDetails(siteDetailsGuid);
                    if (relatedLoan != null)
                    {
                        siteDetailsDto.loanApplicationStatus = relatedLoan.invln_ExternalStatus?.Value;
                    }
                    return JsonSerializer.Serialize(siteDetailsDto);
                }
            }
            return null;
        }

        public void FulfillRegionOnLocalAuthorityChange(invln_SiteDetails target, invln_SiteDetails preImage)
        {
            if (target.invln_LocalAuthorityID != null &&
                (preImage == null || preImage.invln_LocalAuthorityID == null || (preImage.invln_LocalAuthorityID != null && preImage.invln_LocalAuthorityID.Id != target.invln_LocalAuthorityID.Id)))
            {
                var localAuthority = _localAuthorityRepository.GetById(target.invln_LocalAuthorityID.Id, new string[] {nameof(invln_localauthority.invln_Region).ToLower()});
                target.invln_LocalAuthorityRegion = localAuthority.invln_Region;
            }
            else if (target.invln_LocalAuthorityID == null)
            {
                target.invln_LocalAuthorityRegion = null;
            }
        }

        private string GenerateFetchXmlAttributes(string fieldsToRetrieve)
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
        #endregion
    }
}
