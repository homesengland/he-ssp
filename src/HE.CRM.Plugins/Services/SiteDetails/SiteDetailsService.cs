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

        #endregion

        #region Constructors

        public SiteDetailsService(CrmServiceArgs args) : base(args)
        {
            siteDetailsRepository = CrmRepositoriesFactory.Get<ISiteDetailsRepository>();
        }

        #endregion
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
            }
        }
    }
}
