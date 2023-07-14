using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.Base.Services;

namespace HE.CRM.Plugins.Services.SiteDetails
{
    public interface ISiteDetailsService : ICrmService
    {
        void UpdateSiteDetails(string siteDetailsId, string siteDetail, string fieldsToUpdate);
    }
}
