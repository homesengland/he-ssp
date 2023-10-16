using DataverseModel;
using HE.Base.Services;

namespace HE.CRM.Plugins.Services.SiteDetails
{
    public interface ISiteDetailsService : ICrmService
    {
        void UpdateSiteDetails(string siteDetailsId, string siteDetail, string fieldsToUpdate, string loanApplicationId);
        void DeleteSiteDetails(string siteDetailsId);
        void CreateSiteDetail(string siteDetail, string loanApplicationId);
        void SetLastModificationDateOnRelatedLoanApplication(invln_SiteDetails siteDetails);
    }
}
