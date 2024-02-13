using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.CRM.AHP.Plugins.Services.Programme
{
    public interface IProgrammeService : ICrmService
    {
        ProgrammeDto GetProgramme(string programmeId);
        List<ProgrammeDto> GetProgrammes();
    }
}
