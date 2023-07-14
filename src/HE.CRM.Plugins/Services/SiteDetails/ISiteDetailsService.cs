using HE.Base.Services;

namespace HE.CRM.Plugins.Services.SiteDetails
{
    public interface ISiteDetailsService : ICrmService
    {
        void UpdateSiteDetails(string siteDetailsId, string siteDetail, string fieldsToUpdate, string loanApplicationId);
    }
}
