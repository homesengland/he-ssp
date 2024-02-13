using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.AHP.Plugins.Services.Programme;
using HE.CRM.AHP.Plugins.Services.HomeType;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi
{
    public class GetMultipleProgrammeHandler : CrmActionHandlerBase<invln_getsingleprogrammeRequest, DataverseContext>
    {
        #region Fields

        #endregion
        #region Base Methods Overrides
        public override bool CanWork()
        {
            return true;
            ;
        }

        public override void DoWork()
        {
            TracingService.Trace("method");
            var programmes = CrmServicesFactory.Get<IProgrammeService>().GetProgrammes();
            if (programmes != null)
            {
                var programmesSerialized = JsonSerializer.Serialize(programmes);
                ExecutionData.SetOutputParameter(invln_getmultipleprogrammeResponse.Fields.invln_programmeList, programmesSerialized);
            }
        }
        #endregion
    }
}
