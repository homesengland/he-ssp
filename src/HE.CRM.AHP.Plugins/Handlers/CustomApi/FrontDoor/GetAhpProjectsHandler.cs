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
    public class GetAhpProjectsHandler : CrmActionHandlerBase<invln_getahpprojectsRequest, DataverseContext>
    {
        private string externalContactId => ExecutionData.GetInputParameter<string>(invln_getahpprojectsRequest.Fields.invln_userid);
        private string organisationId => ExecutionData.GetInputParameter<string>(invln_getahpprojectsRequest.Fields.invln_accountid);
        private string consortiumId => ExecutionData.GetInputParameter<string>(invln_getahpprojectsRequest.Fields.invln_consortiumid);
        private string pagingRequest => ExecutionData.GetInputParameter<string>(invln_getahpprojectsRequest.Fields.invln_pagingrequest);

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
            if (consortiumId != null && !Guid.TryParse(consortiumId, out Guid consortiumGuid))
            {
                TracingService.Trace("consortiumId is not Guid");
                return false;
            }

            return externalContactId != null && organisationId != null;
        }

        public override void DoWork()
        {
            TracingService.Trace("GetAhpProjectHandler");

            PagingRequestDto paging = null;
            if (pagingRequest != null)
            {
                paging = JsonSerializer.Deserialize<PagingRequestDto>(pagingRequest);
            }
            PagedResponseDto<AhpProjectDto> ahpProjectDto = CrmServicesFactory.Get<IAhpProjectService>().GetAhpProjectsWithSites(externalContactId, organisationId, consortiumId, paging);

            this.TracingService.Trace("Send Response");
            ExecutionData.SetOutputParameter(invln_getahpprojectsResponse.Fields.invln_listOfAhpProjects, JsonSerializer.Serialize(ahpProjectDto));
        }
    }
}
