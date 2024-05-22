using System;
using System.Collections.Generic;
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
        List<invln_Sites> GetSitesForAhpProject(Guid ahpProjectGuid, invln_Permission contactWebRole, Contact contact, Guid organisationGuid, string consortiumId = null);
        List<invln_Sites> GetbyConsortiumId(Guid guid);
    }
}