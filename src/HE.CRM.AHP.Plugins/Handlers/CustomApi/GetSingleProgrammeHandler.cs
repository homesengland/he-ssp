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
    public class GetSingleProgrammeHandler : CrmActionHandlerBase<invln_getsingleprogrammeRequest, DataverseContext>
    {
        #region Fields

        private string programmeId => ExecutionData.GetInputParameter<string>(invln_getsingleprogrammeRequest.Fields.invln_programmeId);


        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return programmeId != null;
        }

        public override void DoWork()
        {
            TracingService.Trace("method");
            var programme = CrmServicesFactory.Get<IProgrammeService>().GetProgramme(programmeId);
            if (programme != null)
            {
                var programmeSerialized = JsonSerializer.Serialize(programme);
                ExecutionData.SetOutputParameter(invln_getsingleprogrammeResponse.Fields.invln_programme, programmeSerialized);
            }
        }

        #endregion
    }
}
