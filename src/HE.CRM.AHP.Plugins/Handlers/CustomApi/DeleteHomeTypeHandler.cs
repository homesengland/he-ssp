using System.Text.Json;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.HomeType;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi
{
    public class DeleteHomeTypeHandler : CrmActionHandlerBase<invln_deletehometypeRequest, DataverseContext>
    {
        #region Fields

        private string homeTypeId => ExecutionData.GetInputParameter<string>(invln_deletehometypeRequest.Fields.invln_hometypeid);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return homeTypeId != null;
        }

        public override void DoWork()
        {
            TracingService.Trace("method");
            var homeTypeDto = CrmServicesFactory.Get<IHomeTypeService>().DeleteHomeType(homeTypeId);
            if (homeTypeDto != null)
            {
                var homeTypesSerialized = JsonSerializer.Serialize(homeTypeDto);
                ExecutionData.SetOutputParameter(invln_getsinglehometypeResponse.Fields.invln_hometype, homeTypesSerialized);
            }
        }

        #endregion
    }
}
