using System.Collections.Generic;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.CRM.AHP.Plugins.Services.Site
{
    public interface ISiteService : ICrmService
    {
        List<SiteDto> GetSites(string fieldsToRetrieve);

        SiteDto GetById(string id, string fieldsToRetrieve);

        bool Exist(string name);

        string Save(string siteId, SiteDto site, string fieldsToSet);
    }
}
