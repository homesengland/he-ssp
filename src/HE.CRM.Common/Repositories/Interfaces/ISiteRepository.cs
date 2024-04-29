using System;
using DataverseModel;
using HE.Base.Repositories;
using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.CRM.Common.Repositories.Interfaces
{
    public interface ISiteRepository : ICrmEntityRepository<invln_Sites, DataverseContext>
    {
        PagedResponseDto<invln_Sites> GetMultiple(PagingRequestDto paging, string fieldsToRetrieve, string externalContactIdFilter, string accountIdFilter);
        invln_Sites GetSingle(string siteIdFilter, string fieldsToRetrieve, string externalContactIdFilter, string accountIdFilter);
        bool Exist(string name);
        bool StrategicSiteNameExists(string strategicSiteName, Guid organisationGuid);
    }
}
