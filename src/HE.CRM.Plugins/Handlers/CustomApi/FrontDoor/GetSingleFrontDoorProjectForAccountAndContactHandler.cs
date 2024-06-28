using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using FrontDoorProjectV1 = HE.CRM.Plugins.Services.FrontDoorProject;
using FrontDoorProjectV2 = HE.CRM.Plugins.Services.FrontDoorProject.V2;

namespace HE.CRM.Plugins.Handlers.CustomApi.FrontDoor
{
    public class GetSingleFrontDoorProjectForAccountAndContactHandler : CrmActionHandlerBase<invln_getsinglefrontdoorprojectRequest, DataverseContext>
    {
        #region Fields
        private string OrganisationId => ExecutionData.GetInputParameter<string>(invln_getsinglefrontdoorprojectRequest.Fields.invln_organisationid);
        private string ExternalContactId => ExecutionData.GetInputParameter<string>(invln_getsinglefrontdoorprojectRequest.Fields.invln_userid);
        private string FrontDoorProjectId => ExecutionData.GetInputParameter<string>(invln_getsinglefrontdoorprojectRequest.Fields.invln_frontdoorprojectid);
        private string FieldsToRetrieve => ExecutionData.GetInputParameter<string>(invln_getsinglefrontdoorprojectRequest.Fields.invln_fieldstoretrieve);
        private string IncludeInactive => ExecutionData.GetInputParameter<string>(invln_getsinglefrontdoorprojectRequest.Fields.invln_includeinactive);
        private string UseHeTablesFromPortal => ExecutionData.GetInputParameter<string>(invln_getsinglefrontdoorprojectRequest.Fields.invln_usehetables);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(OrganisationId) && !string.IsNullOrEmpty(FrontDoorProjectId);
        }

        public override void DoWork()
        {
            Logger.Trace($"{nameof(GetSingleFrontDoorProjectForAccountAndContactHandler)}.{nameof(DoWork)}");

            Logger.Trace($"OrganisationId: {OrganisationId}");
            Logger.Trace($"FrontDoorProjectId: {FrontDoorProjectId}");
            Logger.Trace($"ExternalContactId: {ExternalContactId}");
            Logger.Trace($"FieldsToRetrieve: {FieldsToRetrieve}");
            Logger.Trace($"IncludeInactive: {IncludeInactive}");
            Logger.Trace($"UseHeTablesFromPortal: {UseHeTablesFromPortal}");

            var organisationIdGuid = Guid.Parse(OrganisationId);
            var frontDoorProjectIdGuid = Guid.Parse(FrontDoorProjectId);

            //var useFrontDoorProjectV2 = false;
            //// temporary workaround
            //if (Guid.Parse(OrganisationId) == Guid.Parse("0eb56594-a60b-ef11-9f89-0022481adce0"))
            //{
            //    Logger.Trace("use FrontDoorProjectV2");
            //    useFrontDoorProjectV2 = true;
            //}

            if (FeatureFlags.UseNewFrontDoorApiManagement)
            { // New frontdoor apim
                var service = CrmServicesFactory.Get<FrontDoorProjectV2.IFrontDoorProjectService>();
                var project = service.GetFrontDoorProject(organisationIdGuid, ExternalContactId, frontDoorProjectIdGuid, IncludeInactive);

                var result = new List<HE.Common.IntegrationModel.PortalIntegrationModel.FrontDoorProjectDto>();
                if (project != null)
                {
                    result.Add(project);
                };

                var frontDoorProjectDtoListSerialized = JsonSerializer.Serialize(result);
                Logger.Trace(frontDoorProjectDtoListSerialized);
                ExecutionData.SetOutputParameter(invln_getsinglefrontdoorprojectResponse.Fields.invln_retrievedfrontdoorprojectfields, frontDoorProjectDtoListSerialized);
            }
            else
            { // old version
                var useHeTables = !string.IsNullOrEmpty(UseHeTablesFromPortal);

                var service = CrmServicesFactory.Get<FrontDoorProjectV1.IFrontDoorProjectService>();
                var frontDoorProjectsDtoList = service.GetFrontDoorProjects(OrganisationId, useHeTables, ExternalContactId, FieldsToRetrieve, FrontDoorProjectId, IncludeInactive);

                this.TracingService.Trace("Send Response");
                if (frontDoorProjectsDtoList != null)
                {
                    var frontDoorProjectDtoListSerialized = JsonSerializer.Serialize(frontDoorProjectsDtoList);
                    ExecutionData.SetOutputParameter(invln_getsinglefrontdoorprojectResponse.Fields.invln_retrievedfrontdoorprojectfields, frontDoorProjectDtoListSerialized);
                }

                Logger.Trace("end");
            }

            #endregion
        }
    }
}
