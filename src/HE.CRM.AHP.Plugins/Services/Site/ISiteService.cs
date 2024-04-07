using System.Collections.Generic;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.CRM.AHP.Plugins.Services.Site
{
    public interface ISiteService : ICrmService
    {
        PagedResponseDto<SiteDto> GetMultiple(PagingRequestDto paging, string fieldsToRetrieve, string externalContactId, string accountId);

        SiteDto GetById(string id, string fieldsToRetrieve);

        bool Exist(string name);

        string Save(string siteId, SiteDto site, string fieldsToSet, string externalContactId, string accountId);
    }
}
