using System.Text.Json;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.HomeType;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi
{
    public class GetSingleHomeTypeHandler : CrmActionHandlerBase<invln_getsinglehometypeRequest, DataverseContext>
    {
        #region Fields

        private string homeTypeId => ExecutionData.GetInputParameter<string>(invln_getsinglehometypeRequest.Fields.invln_hometypeid);
        private string applicationId => ExecutionData.GetInputParameter<string>(invln_getsinglehometypeRequest.Fields.invln_applicationid);
        private string organisationId => ExecutionData.GetInputParameter<string>(invln_getsinglehometypeRequest.Fields.invln_organisationid);
        private string userId => ExecutionData.GetInputParameter<string>(invln_getsinglehometypeRequest.Fields.invln_userid);
        private string fieldsToRetrieve => ExecutionData.GetInputParameter<string>(invln_getsinglehometypeRequest.Fields.invln_fieldstoretrieve);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return homeTypeId != null && applicationId != null && organisationId != null && userId != null;
        }

        public override void DoWork()
        {
            TracingService.Trace("method");
            var homeTypeDto = CrmServicesFactory.Get<IHomeTypeService>().GetHomeType(homeTypeId, applicationId, userId, organisationId, fieldsToRetrieve);
            if (homeTypeDto != null)
            {
                var homeTypesSerialized = JsonSerializer.Serialize(homeTypeDto);
                ExecutionData.SetOutputParameter(invln_getsinglehometypeResponse.Fields.invln_hometype, homeTypesSerialized);
            }
        }

        #endregion
    }
}
