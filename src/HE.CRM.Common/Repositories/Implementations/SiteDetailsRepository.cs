using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System.Linq;

public class SiteDetailsRepository : CrmEntityRepository<invln_SiteDetails, DataverseContext>, ISiteDetailsRepository
{
    public SiteDetailsRepository(CrmRepositoryArgs args) : base(args)
    {
    }

    public void DeleteSiteDetailsRelatedToLoanApplication(EntityReference loanApplicationId)
    {
        if (loanApplicationId != null)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                var siteDetails = ctx.CreateQuery<invln_SiteDetails>()
                    .Where(x => x.invln_Loanapplication.Id == loanApplicationId.Id).ToList();

                foreach (var site in siteDetails)
                {
                    service.Delete(invln_SiteDetails.EntityLogicalName, site.Id);
                }
            }
        }
    }
}

