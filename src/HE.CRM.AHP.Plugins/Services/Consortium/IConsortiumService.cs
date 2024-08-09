using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Services;
using static HE.CRM.AHP.Plugins.Services.Consortium.ConsortiumService;

namespace HE.CRM.AHP.Plugins.Services.Consortium
{
    public interface IConsortiumService : ICrmService
    {
        bool CheckAccess(Operation operation, RecordType recordtype, string externalUserId, string siteId = null, string applicationId = null, string consortiumId = null, string organizationId = null, string ahpProject = null);

        Guid GetConsortiumIdForApplication(Guid ahpAplicationId);
        Guid GetConsortiumIdForSite(Guid siteId);
    }
}
