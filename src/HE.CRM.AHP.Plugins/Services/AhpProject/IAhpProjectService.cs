using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.Base.Services;

namespace HE.CRM.AHP.Plugins.Services.AhpProject
{
    public interface IAhpProjectService : ICrmService
    {
        string CreateRecordFromPortal(string externalContactId, string organisationId, string heProjectId, string ahpProjectName, string consortiumId);
    }
}
