using System.Text.Json;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.HomeType;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi
{
    public class SetHomeTypeHandler : CrmActionHandlerBase<invln_sethometypeRequest, DataverseContext>
    {
        #region Fields

        private string homeType => ExecutionData.GetInputParameter<string>(invln_sethometypeRequest.Fields.invln_hometype);
        private string fieldsToSet => ExecutionData.GetInputParameter<string>(invln_sethometypeRequest.Fields.invln_fieldstoset);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return homeType != null;
        }

        public override void DoWork()
        {
            TracingService.Trace("method");
            CrmServicesFactory.Get<IHomeTypeService>().SetHomeType(homeType, fieldsToSet);
        }

        #endregion
    }
}
