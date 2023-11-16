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
        private string userId => ExecutionData.GetInputParameter<string>(invln_deletehometypeRequest.Fields.invln_userid);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return homeTypeId != null && userId != null;
        }

        public override void DoWork()
        {
            TracingService.Trace("method");
            CrmServicesFactory.Get<IHomeTypeService>().DeleteHomeType(homeTypeId, userId);
        }
    }

    #endregion
}
