using System;
using System.Text.Json;
using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.DtoMapping;
using HE.CRM.Common.Repositories.Interfaces;

namespace HE.CRM.Plugins.Services.SiteDetails
{
    public class SiteDetailsService : CrmService, ISiteDetailsService
    {
        #region Fields

        private readonly ISiteDetailsRepository siteDetailsRepository;
        private readonly ILoanApplicationRepository _loanApplicationRepository;

        #endregion

        #region Constructors

        public SiteDetailsService(CrmServiceArgs args) : base(args)
        {
            siteDetailsRepository = CrmRepositoriesFactory.Get<ISiteDetailsRepository>();
            _loanApplicationRepository = CrmRepositoriesFactory.Get<ILoanApplicationRepository>();
        }

        #endregion

        #region Public Methods
        public void UpdateSiteDetails(string siteDetailsId, string siteDetail, string fieldsToUpdate, string loanApplicationId)
        {
            if(Guid.TryParse(siteDetailsId, out Guid detailsId))
            {
                var deserilizedSiteDetail = JsonSerializer.Deserialize<SiteDetailsDto>(siteDetail);
                var siteDetailsMapped = SiteDetailsDtoMapper.MapSiteDetailsDtoToRegularEntity(deserilizedSiteDetail, loanApplicationId);
                invln_SiteDetails siteDetailToUpdate = new invln_SiteDetails();
                if (string.IsNullOrEmpty(fieldsToUpdate))
                {
                    siteDetailToUpdate = siteDetailsMapped;
                }
                else
                {
                    var fields = fieldsToUpdate.Split(',');
                    if(fields.Length > 0)
                    {
                        foreach(var field in fields)
                        {
                            siteDetailToUpdate[field] = siteDetailsMapped[field];
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
            var siteDetailsToCreate = SiteDetailsDtoMapper.MapSiteDetailsDtoToRegularEntity(deserilizedSiteDetail, loanApplicationId);
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
        #endregion
    }
}
