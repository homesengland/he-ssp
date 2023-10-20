using HE.Base.Repositories;
using DataverseModel;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using System;

namespace HE.CRM.Common.Repositories.Interfaces
{

    public interface ISiteDetailsRepository : ICrmEntityRepository<invln_SiteDetails, DataverseContext>
    {
        void DeleteSiteDetailsRelatedToLoanApplication(EntityReference loanApplicationId);
        List<invln_SiteDetails> GetSiteDetailRelatedToLoanApplication(EntityReference loanApplicationId);
        invln_SiteDetails GetsiteDetailWithFieldsToRetrieve(Guid siteDetailsGuid, string attributes);
    }
}
