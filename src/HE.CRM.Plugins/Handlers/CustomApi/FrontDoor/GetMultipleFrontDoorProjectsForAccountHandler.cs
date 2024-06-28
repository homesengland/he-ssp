using System;
using System.Collections.Generic;
using System.Text.Json;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using FrontDoorProjectV1 = HE.CRM.Plugins.Services.FrontDoorProject;
using FrontDoorProjectV2 = HE.CRM.Plugins.Services.FrontDoorProject.V2;


namespace HE.CRM.Plugins.Handlers.CustomApi.FrontDoor
{
    public class GetMultipleFrontDoorProjectsForAccountHandler : CrmActionHandlerBase<invln_getmultiplefrontdoorprojectsRequest, DataverseContext>
    {
        #region Fields
        private string OrganisationId => ExecutionData.GetInputParameter<string>(invln_getmultiplefrontdoorprojectsRequest.Fields.invln_organisationid);
        private string ExternalContactId => ExecutionData.GetInputParameter<string>(invln_getmultiplefrontdoorprojectsRequest.Fields.inlvn_userid);
        private string FieldsToRetrieve => ExecutionData.GetInputParameter<string>(invln_getmultiplefrontdoorprojectsRequest.Fields.invln_fieldstoretrieve);
        private string UseHeTablesFromPortal => ExecutionData.GetInputParameter<string>(invln_getmultiplefrontdoorprojectsRequest.Fields.invln_usehetables);
        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return OrganisationId != null;
        }

        public override void DoWork()
        {
            Logger.Trace($"{nameof(GetMultipleFrontDoorProjectsForAccountHandler)}.{nameof(DoWork)}");

            Logger.Trace($"OrganisationId: {OrganisationId}");
            Logger.Trace($"ExternalContactId: {ExternalContactId}");
            Logger.Trace($"FieldsToRetrieve: {FieldsToRetrieve}");
            Logger.Trace($"UseHeTablesFromPortal: {UseHeTablesFromPortal}");

            var useHeTables = !string.IsNullOrEmpty(UseHeTablesFromPortal);

            var organisationIdGuid = Guid.Parse(OrganisationId);

            var frontDoorProjectsDtoList = new List<FrontDoorProjectDto>();
            if (FeatureFlags.UseNewFrontDoorApiManagement)
            { // New frontdoor apim
                var service = CrmServicesFactory.Get<FrontDoorProjectV2.IFrontDoorProjectService>();
                frontDoorProjectsDtoList = service.GetFrontDoorProjects(organisationIdGuid, ExternalContactId);
                Logger.Trace($"FrontDoor projects contains {frontDoorProjectsDtoList.Count} records.");
            }
            else
            { // old version
                var service = CrmServicesFactory.Get<FrontDoorProjectV1.IFrontDoorProjectService>();
                frontDoorProjectsDtoList = service.GetFrontDoorProjects(OrganisationId, useHeTables, ExternalContactId, FieldsToRetrieve);
            }

            Logger.Trace("Send Response");
            var frontDoorProjectDtoListSerialized = JsonSerializer.Serialize(frontDoorProjectsDtoList);

            ExecutionData.SetOutputParameter(invln_getmultiplefrontdoorprojectsResponse.Fields.invln_frontdoorprojects, frontDoorProjectDtoListSerialized);
        }

        #endregion
    }
}
