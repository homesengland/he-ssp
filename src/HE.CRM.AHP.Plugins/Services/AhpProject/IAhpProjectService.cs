using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.CRM.AHP.Plugins.Services.AhpProject
{
    public interface IAhpProjectService : ICrmService
    {
        string CreateRecordFromPortal(string externalContactId, string organisationId, string heProjectId, string ahpProjectName, string consortiumId);
        AhpProjectDto GetAhpProjectWithApplicationsAndSitesAndAlloctions(bool useV2Version, string externalContactId, string organisationId, string ahpProjectId, string heProjectId, string consortiumId);
        PagedResponseDto<AhpProjectDto> GetAhpProjectsWithSites(string externalContactId, string organisationId, string consortiumId, PagingRequestDto paging);
    }
}
