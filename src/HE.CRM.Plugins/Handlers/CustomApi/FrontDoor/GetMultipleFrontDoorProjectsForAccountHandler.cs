using System;
using System.Collections.Generic;
using System.Text.Json;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.Helpers;
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

            var useFrontDoorProjectV2 = true;
            // temporary
            try
            {
                if (Guid.Parse(OrganisationId) == Guid.Parse("0eb56594-a60b-ef11-9f89-0022481adce0"))
                {
                    Logger.Trace("use FrontDoorProjectV2");
                    useFrontDoorProjectV2 = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }

            var frontDoorProjectsDtoList = new List<FrontDoorProjectDto>();
            if (useFrontDoorProjectV2)
            {
                var service = CrmServicesFactory.Get<FrontDoorProjectV2.IFrontDoorProjectService>();
                frontDoorProjectsDtoList = service.GetFrontDoorProjects(organisationIdGuid);
                Logger.Trace($"FronDoor projects contains {frontDoorProjectsDtoList.Count} records.");
            }
            else
            {
                var service = CrmServicesFactory.Get<FrontDoorProjectV1.IFrontDoorProjectService>();
                frontDoorProjectsDtoList = service.GetFrontDoorProjects(OrganisationId, useHeTables, ExternalContactId, FieldsToRetrieve);
            }

            Logger.Trace("Send Response");
            if (frontDoorProjectsDtoList != null)
            {
                var frontDoorProjectDtoListSerialized = JsonSerializer.Serialize(frontDoorProjectsDtoList);
                ExecutionData.SetOutputParameter(invln_getmultiplefrontdoorprojectsResponse.Fields.invln_frontdoorprojects, frontDoorProjectDtoListSerialized);
            }
        }

        #endregion
    }
}
