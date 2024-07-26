using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.AHP.Plugins.Services.AhpProject;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi.FrontDoor
{
    public class GetAhpProjectV2Handler : CrmActionHandlerBase<invln_getahpproject_v2Request, DataverseContext>
    {
        private string externalContactId => ExecutionData.GetInputParameter<string>(invln_getahpproject_v2Request.Fields.invln_userid);
        private string organisationId => ExecutionData.GetInputParameter<string>(invln_getahpproject_v2Request.Fields.invln_accountid);
        private string ahpProjectId => ExecutionData.GetInputParameter<string>(invln_getahpproject_v2Request.Fields.invln_ahpprojectid);
        private string heProjectId => ExecutionData.GetInputParameter<string>(invln_getahpproject_v2Request.Fields.invln_heprojectid);
        private string consortiumId => ExecutionData.GetInputParameter<string>(invln_getahpproject_v2Request.Fields.invln_consortiumid);

        public override bool CanWork()
        {
            if (externalContactId == null)
            {
                TracingService.Trace("externalContactId is Empty");
                return false;
            }
            if (!Guid.TryParse(organisationId, out Guid organisationGuid))
            {
                TracingService.Trace("organisationId is not Guid");
                return false;
            }
            if (ahpProjectId != null && !Guid.TryParse(ahpProjectId, out Guid ahpProjectGuid))
            {
                TracingService.Trace("ahpProjectId is not Guid");
                return false;
            }
            if (heProjectId != null && !Guid.TryParse(heProjectId, out Guid heProjectGuid))
            {
                TracingService.Trace("heProjectId is not Guid");
                return false;
            }
            if (consortiumId != null && !Guid.TryParse(consortiumId, out Guid consortiumGuid))
            {
                TracingService.Trace("consortiumId is not Guid");
                return false;
            }

            return externalContactId != null && organisationId != null && (ahpProjectId != null || heProjectId != null);
        }

        public override void DoWork()
        {
            TracingService.Trace("GetAhpProjectHandler");
            if (externalContactId != null)
            {
                TracingService.Trace($"* externalContactId : {externalContactId}");
            };
            if (organisationId != null)
            {
                TracingService.Trace($"* organisationId : {organisationId}");
            };
            if (ahpProjectId != null)
            {
                TracingService.Trace($"* ahpProjectId : {ahpProjectId}");
            };
            if (heProjectId != null)
            {
                TracingService.Trace($"* heProjectId : {heProjectId}");
            };
            if (consortiumId != null)
            {
                TracingService.Trace($"* consortiumId: {consortiumId}");
            };

            var useV2Version = true; // Version contain Allocations
            AhpProjectDto ahpProjectDto = CrmServicesFactory.Get<IAhpProjectService>().GetAhpProjectWithApplicationsAndSitesAndAlloctions(useV2Version, externalContactId, organisationId, ahpProjectId, heProjectId, consortiumId);

            this.TracingService.Trace("Send Response");
            ExecutionData.SetOutputParameter(invln_getahpproject_v2Response.Fields.invln_ahpProjectApplications, JsonSerializer.Serialize(ahpProjectDto));
        }
    }
}
