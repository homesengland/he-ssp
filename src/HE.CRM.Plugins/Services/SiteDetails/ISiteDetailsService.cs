using DataverseModel;
using HE.Base.Services;

namespace HE.CRM.Plugins.Services.SiteDetails
{
    public interface ISiteDetailsService : ICrmService
    {
        void UpdateSiteDetails(bool useHeTables, string siteDetailsId, string siteDetail, string fieldsToUpdate, string loanApplicationId);
        void DeleteSiteDetails(string siteDetailsId);
        void CreateSiteDetail(bool useHeTables, string siteDetail, string loanApplicationId);
        string GetSingleSiteDetail(bool useHeTables, string siteDetailsId, string accountId, string contactExternalId, string fieldsToRetrieve = null);
        void SetLastModificationDateOnRelatedLoanApplication(invln_SiteDetails siteDetails);
        void FulfillRegionOnLocalAuthorityChange(invln_SiteDetails target, invln_SiteDetails preImage);
    }
}
