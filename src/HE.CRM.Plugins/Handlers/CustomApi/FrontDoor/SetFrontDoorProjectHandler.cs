using DataverseModel;
using HE.Base.Plugins.Handlers;
using FrontDoorProjectV1 = HE.CRM.Plugins.Services.FrontDoorProject;
using FrontDoorProjectV2 = HE.CRM.Plugins.Services.FrontDoorProject.V2;

namespace HE.CRM.Plugins.Handlers.CustomApi.FrontDoor
{
    public class SetFrontDoorProjectHandler : CrmActionHandlerBase<invln_setfrontdoorprojectRequest, DataverseContext>
    {
        #region Fields
        private string OrganisationId => ExecutionData.GetInputParameter<string>(invln_setfrontdoorprojectRequest.Fields.invln_organisationid);
        private string ExternalContactId => ExecutionData.GetInputParameter<string>(invln_setfrontdoorprojectRequest.Fields.invln_userid);
        private string FrontDoorProjectId => ExecutionData.GetInputParameter<string>(invln_setfrontdoorprojectRequest.Fields.invln_frontdoorprojectid);
        private string EntityFieldsParameters => ExecutionData.GetInputParameter<string>(invln_setfrontdoorprojectRequest.Fields.invln_entityfieldsparameters);
        private string UseHeTablesFromPortal => ExecutionData.GetInputParameter<string>(invln_getsinglefrontdoorprojectRequest.Fields.invln_usehetables);
        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(ExternalContactId) && !string.IsNullOrEmpty(OrganisationId) && !string.IsNullOrEmpty(EntityFieldsParameters);
        }

        public override void DoWork()
        {
            Logger.Trace($"{nameof(SetFrontDoorProjectHandler)}.{nameof(DoWork)}");

            Logger.Trace($"OrganisationId: {OrganisationId}");
            Logger.Trace($"ExternalContactId: {ExternalContactId}");
            Logger.Trace($"FrontDoorProjectId: {FrontDoorProjectId}");
            Logger.Trace($"EntityFieldsParameters: {EntityFieldsParameters}");
            Logger.Trace($"UseHeTablesFromPortal: {UseHeTablesFromPortal}");
            // OrganisationId.Equals("0eb56594-a60b-ef11-9f89-0022481adce0", System.StringComparison.InvariantCultureIgnoreCase)
            if (FeatureFlags.UseNewFrontDoorApiManagement && false)
            { // New frontdoor apim
                var service = CrmServicesFactory.Get<FrontDoorProjectV2.IFrontDoorProjectService>();
                var frontdoorprojectid = service.CreateRecordFromPortal(ExternalContactId, OrganisationId, FrontDoorProjectId, EntityFieldsParameters);
                this.TracingService.Trace("Send Response");
                ExecutionData.SetOutputParameter(invln_setfrontdoorprojectResponse.Fields.invln_frontdoorprojectid, frontdoorprojectid);
            }
            else
            { // old version
                var service = CrmServicesFactory.Get<FrontDoorProjectV1.IFrontDoorProjectService>();
                var useHeTables = !string.IsNullOrEmpty(UseHeTablesFromPortal);
                var frontdoorprojectid = service.CreateRecordFromPortal(ExternalContactId, OrganisationId, FrontDoorProjectId, EntityFieldsParameters, useHeTables);
                this.TracingService.Trace("Send Response");
                ExecutionData.SetOutputParameter(invln_setfrontdoorprojectResponse.Fields.invln_frontdoorprojectid, frontdoorprojectid);
            }
        }
        #endregion
    }
}
