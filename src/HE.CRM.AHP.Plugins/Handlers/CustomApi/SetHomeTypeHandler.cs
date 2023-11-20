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
        private string userId => ExecutionData.GetInputParameter<string>(invln_sethometypeRequest.Fields.invln_userid);
        private string organisationId => ExecutionData.GetInputParameter<string>(invln_sethometypeRequest.Fields.invln_organisationid);
        private string applicationId => ExecutionData.GetInputParameter<string>(invln_sethometypeRequest.Fields.invln_applicationid);
        private string fieldsToSet => ExecutionData.GetInputParameter<string>(invln_sethometypeRequest.Fields.invln_fieldstoset);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return homeType != null && userId != null && organisationId != null && applicationId != null;
        }

        public override void DoWork()
        {
            TracingService.Trace("method");
            var createdRecordGuid = CrmServicesFactory.Get<IHomeTypeService>().SetHomeType(homeType, userId, organisationId, applicationId, fieldsToSet);
            ExecutionData.SetOutputParameter(invln_sethometypeResponse.Fields.invln_hometypeid, createdRecordGuid.ToString());
        }

        #endregion
    }
}
