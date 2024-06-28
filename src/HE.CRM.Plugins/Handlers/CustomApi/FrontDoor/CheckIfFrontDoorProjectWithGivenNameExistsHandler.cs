using System;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using FrontDoorProjectV1 = HE.CRM.Plugins.Services.FrontDoorProject;
using FrontDoorProjectV2 = HE.CRM.Plugins.Services.FrontDoorProject.V2;

namespace HE.CRM.Plugins.Handlers.CustomApi.FrontDoor
{
    public class CheckIfFrontDoorProjectWithGivenNameExistsHandler : CrmActionHandlerBase<invln_checkiffrontdoorprojectwithgivennameexistsRequest, DataverseContext>
    {
        #region Fields

        private string FrontDoorProjectName => ExecutionData.GetInputParameter<string>(invln_checkiffrontdoorprojectwithgivennameexistsRequest.Fields.invln_frontdoorprojectname);
        private string OrganisationId => ExecutionData.GetInputParameter<string>(invln_checkiffrontdoorprojectwithgivennameexistsRequest.Fields.invln_organisationid);
        private string UseHeTablesFromPortal => ExecutionData.GetInputParameter<string>(invln_checkiffrontdoorprojectwithgivennameexistsRequest.Fields.invln_usehetables);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(FrontDoorProjectName) && !string.IsNullOrEmpty(OrganisationId);
        }

        public override void DoWork()
        {
            Logger.Trace($"{nameof(CheckIfFrontDoorProjectWithGivenNameExistsHandler)}.{nameof(DoWork)}");

            Logger.Trace($"FrontDoorProjectName: {FrontDoorProjectName}");
            Logger.Trace($"OrganisationId: {OrganisationId}");
            Logger.Trace($"UseHeTablesFromPortal: {UseHeTablesFromPortal}");

            if (FeatureFlags.UseNewFrontDoorApiManagement)
            { // New frontdoor apim
                var service = CrmServicesFactory.Get<FrontDoorProjectV2.IFrontDoorProjectService>();

                var organisationIdGuid = Guid.Parse(OrganisationId);
                var frontDoorProjectExists = service.CheckIfFrontDoorProjectWithGivenNameExists(FrontDoorProjectName, organisationIdGuid);

                this.TracingService.Trace("Send Response");
                ExecutionData.SetOutputParameter(invln_checkiffrontdoorprojectwithgivennameexistsResponse.Fields.invln_frontdoorprojectexists, frontDoorProjectExists);
            }
            else
            { // old version
                var service = CrmServicesFactory.Get<FrontDoorProjectV1.IFrontDoorProjectService>();

                var useHeTables = !string.IsNullOrEmpty(UseHeTablesFromPortal);
                var frontDoorProjectExists = service.CheckIfFrontDoorProjectWithGivenNameExists(FrontDoorProjectName, useHeTables, OrganisationId);

                this.TracingService.Trace("Send Response");
                ExecutionData.SetOutputParameter(invln_checkiffrontdoorprojectwithgivennameexistsResponse.Fields.invln_frontdoorprojectexists, frontDoorProjectExists);
            }
        }
        #endregion
    }
}
