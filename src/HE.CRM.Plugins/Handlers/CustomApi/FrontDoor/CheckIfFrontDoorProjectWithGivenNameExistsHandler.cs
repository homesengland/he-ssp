using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.FrontDoorProject;

namespace HE.CRM.Plugins.Handlers.CustomApi.FrontDoor
{
    public class CheckIfFrontDoorProjectWithGivenNameExistsHandler : CrmActionHandlerBase<invln_checkiffrontdoorprojectwithgivennameexistsRequest, DataverseContext>
    {
        #region Fields
        private string frontDoorProjectName => ExecutionData.GetInputParameter<string>(invln_checkiffrontdoorprojectwithgivennameexistsRequest.Fields.invln_frontdoorprojectname);
        private string organisationId => ExecutionData.GetInputParameter<string>(invln_checkiffrontdoorprojectwithgivennameexistsRequest.Fields.invln_organisationid);
        private string useHeTablesFromPortal => ExecutionData.GetInputParameter<string>(invln_getsinglefrontdoorprojectRequest.Fields.invln_usehetables);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(frontDoorProjectName);
        }

        public override void DoWork()
        {
            this.TracingService.Trace("CheckIfFrontDoorProjectWithGivenNameExistsHandler");
            var useHeTables = !string.IsNullOrEmpty(useHeTablesFromPortal);
            var frontDoorProjectExists = CrmServicesFactory.Get<IFrontDoorProjectService>().CheckIfFrontDoorProjectWithGivenNameExists(frontDoorProjectName, useHeTables, organisationId);
            this.TracingService.Trace("Send Response");
            if (frontDoorProjectExists != null)
            {
                ExecutionData.SetOutputParameter(invln_checkiffrontdoorprojectwithgivennameexistsResponse.Fields.invln_frontdoorprojectexists, frontDoorProjectExists);
            }
        }
        #endregion



    }
}
