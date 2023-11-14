using System.Text.Json;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.HomeType;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi
{
    public class GetTypeOfHomesListHandler : CrmActionHandlerBase<invln_gettypeofhomeslistRequest, DataverseContext>
    {
        #region Fields

        private string applicationId => ExecutionData.GetInputParameter<string>(invln_gettypeofhomeslistRequest.Fields.invln_applicationid);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return applicationId != null;
        }

        public override void DoWork()
        {
            TracingService.Trace("method");
            var homeTypesList = CrmServicesFactory.Get<IHomeTypeService>().GetApplicaitonHomeTypes(applicationId);
            if (homeTypesList != null)
            {
                var homeTypesSerialized = JsonSerializer.Serialize(homeTypesList);
                ExecutionData.SetOutputParameter(invln_gettypeofhomeslistResponse.Fields.invln_hometypeslist, homeTypesSerialized);
            }
        }

        #endregion
    }
}
