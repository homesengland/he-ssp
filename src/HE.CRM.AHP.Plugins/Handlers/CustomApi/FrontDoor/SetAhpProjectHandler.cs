using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.AhpProject;
using HE.CRM.AHP.Plugins.Services.Site;
using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi.FrontDoor
{
    public class SetAhpProjectHandler : CrmActionHandlerBase<invln_createahpprojectRequest, DataverseContext>
    {
        private string externalContactId => ExecutionData.GetInputParameter<string>(invln_createahpprojectRequest.Fields.invln_userid);
        private string organisationId => ExecutionData.GetInputParameter<string>(invln_createahpprojectRequest.Fields.invln_accountid);
        private string consortiumId => ExecutionData.GetInputParameter<string>(invln_createahpprojectRequest.Fields.invln_consortiumid);
        private string heProjectId => ExecutionData.GetInputParameter<string>(invln_createahpprojectRequest.Fields.invln_heprojectid);
        private string ahpProjectName => ExecutionData.GetInputParameter<string>(invln_createahpprojectRequest.Fields.invln_projectname);
        private string ahpProjectDto => ExecutionData.GetInputParameter<string>(invln_createahpprojectRequest.Fields.invln_listofsites);


        public override bool CanWork()
        {
            return externalContactId != null && organisationId != null && heProjectId != null && ahpProjectName != null && JsonSerializer.Deserialize<AhpProjectDto>(ahpProjectDto).ListOfSites != null;
        }

        public override void DoWork()
        {
            this.TracingService.Trace("SetAhpProjectHandler");
            string  ahpProjectId = CrmServicesFactory.Get<IAhpProjectService>().CreateRecordFromPortal(externalContactId, organisationId, heProjectId, ahpProjectName, consortiumId);
            if (ahpProjectId != null && Guid.TryParse(ahpProjectId, out var ahpProjectGuid))
            {
                List<SiteDto> listOfSites = JsonSerializer.Deserialize<AhpProjectDto>(ahpProjectDto).ListOfSites;
                var isSitesCreated = CrmServicesFactory.Get<ISiteService>().CreateRecordsWithAhpProject(listOfSites, ahpProjectGuid, externalContactId, organisationId);
                if (isSitesCreated != null)
                {
                    this.TracingService.Trace("Send Response");
                    ExecutionData.SetOutputParameter(invln_createahpprojectResponse.Fields.invln_ahpprojectid, ahpProjectId.ToString());
                }
            }
        }
    }
}
