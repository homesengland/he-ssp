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

        #endregion

        #region Constructors

        public SiteDetailsService(CrmServiceArgs args) : base(args)
        {
            siteDetailsRepository = CrmRepositoriesFactory.Get<ISiteDetailsRepository>();
            _loanApplicationRepository = CrmRepositoriesFactory.Get<ILoanApplicationRepository>();
            _localAuthorityRepository = CrmRepositoriesFactory.Get<ILocalAuthorityRepository>();
        }

        #endregion

        #region Public Methods
        public void UpdateSiteDetails(string siteDetailsId, string siteDetail, string fieldsToUpdate, string loanApplicationId)
        {
            if (Guid.TryParse(siteDetailsId, out Guid detailsId))
            {
                var deserilizedSiteDetail = JsonSerializer.Deserialize<SiteDetailsDto>(siteDetail);
                invln_localauthority localAuthority = null;
                if (deserilizedSiteDetail.localAuthority != null && !string.IsNullOrEmpty(deserilizedSiteDetail.localAuthority.onsCode))
                {
                    localAuthority = _localAuthorityRepository.GetLocalAuthorityWithGivenOnsCode(deserilizedSiteDetail.localAuthority.onsCode);
                }
                var siteDetailsMapped = SiteDetailsDtoMapper.MapSiteDetailsDtoToRegularEntity(deserilizedSiteDetail, loanApplicationId, localAuthority);
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
                            if (field == nameof(invln_SiteDetails.invln_LocalAuthorityRegion).ToLower())
                            {
                                siteDetailToUpdate.invln_LocalAuthorityRegion = siteDetailsMapped.invln_LocalAuthorityRegion;
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

        public void CreateSiteDetail(string siteDetail, string loanApplicationId)
        {
            var deserilizedSiteDetail = JsonSerializer.Deserialize<SiteDetailsDto>(siteDetail);
            invln_localauthority localAuthority = null;
            if (deserilizedSiteDetail.localAuthority != null && !string.IsNullOrEmpty(deserilizedSiteDetail.localAuthority.onsCode))
            {
                localAuthority = _localAuthorityRepository.GetLocalAuthorityWithGivenOnsCode(deserilizedSiteDetail.localAuthority.onsCode);
            }
            var siteDetailsToCreate = SiteDetailsDtoMapper.MapSiteDetailsDtoToRegularEntity(deserilizedSiteDetail, loanApplicationId, localAuthority);
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

        public string GetSingleSiteDetail(string siteDetailsId, string accountId, string contactExternalId, string fieldsToRetrieve = null)
        {
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
                    invln_localauthority localAuthority = null;
                    if (retrievedSiteDetail.invln_LocalAuthorityRegion != null)
                    {
                        localAuthority = _localAuthorityRepository.GetById(retrievedSiteDetail.invln_LocalAuthorityID.Id);
                    }
                    var siteDetailsDto = SiteDetailsDtoMapper.MapSiteDetailsToDto(retrievedSiteDetail, localAuthority);
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
