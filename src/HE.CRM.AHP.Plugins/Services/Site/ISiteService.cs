using System;
using System.Collections.Generic;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.CRM.AHP.Plugins.Services.Site
{
    public interface ISiteService : ICrmService
    {
        PagedResponseDto<SiteDto> GetMultiple(PagingRequestDto paging, string fieldsToRetrieve, string externalContactId, string accountId);

        SiteDto GetSingle(string id, string fieldsToRetrieve, string externalContactId, string accountId);

        bool Exist(string name);

        bool StrategicSiteNameExists(string strategicSiteName, Guid organisationGuid);

        string Save(string siteId, SiteDto site, string fieldsToSet, string externalContactId, string accountId);
    }
}
