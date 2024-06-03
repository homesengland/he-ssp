using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Handlers.AHPApplication;
using HE.CRM.AHP.Plugins.Services.Consortium;
using HE.CRM.Common.DtoMapping;
using HE.CRM.Common.Repositories.interfaces;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Crm.Sdk.Messages;
using static System.Net.Mime.MediaTypeNames;
using static HE.CRM.AHP.Plugins.Services.Consortium.ConsortiumService;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi.FrontDoor
{
    public class GetSiteApplicationHandler : CrmActionHandlerBase<invln_getsiteapplicationsRequest, DataverseContext>
    {
        private string ExternalUserId => ExecutionData.GetInputParameter<string>(invln_getsiteapplicationsRequest.Fields.invln_userid);
        private string OrganizationId => ExecutionData.GetInputParameter<string>(invln_getsiteapplicationsRequest.Fields.invln_organizationid);
        private string SiteId => ExecutionData.GetInputParameter<string>(invln_getsiteapplicationsRequest.Fields.invln_siteid);
        private string ConsortiumId => ExecutionData.GetInputParameter<string>(invln_getsiteapplicationsRequest.Fields.invln_consortiumid);

        private readonly ISiteRepository _siteRepository;
        private readonly IConsortiumService _consortiumService;
        private readonly IAhpApplicationRepository _ahpApplicationRepository;

        public GetSiteApplicationHandler(ISiteRepository siteRepository,
                IConsortiumService consortiumService, IAhpApplicationRepository ahpApplicationRepository)
        {
            _siteRepository = siteRepository;
            _consortiumService = consortiumService;
            _ahpApplicationRepository = ahpApplicationRepository;
        }

        public override bool CanWork()
        {
            TracingService.Trace("GetSiteApplicationHandler Can Work");
            TracingService.Trace($"ExternalUserId: {ExternalUserId}");
            TracingService.Trace($"OrganizationId: {OrganizationId}");
            TracingService.Trace($"SiteId: {SiteId}");
            TracingService.Trace($"ConsortiumId: {ConsortiumId}");

            return !string.IsNullOrEmpty(ExternalUserId) && !string.IsNullOrEmpty(OrganizationId)
                    && !string.IsNullOrEmpty(SiteId);
        }

        public override void DoWork()
        {
            string consortiumId = null;
            if (!string.IsNullOrEmpty(ConsortiumId))
            {
                consortiumId = ConsortiumId;
            }

            TracingService.Trace("GetSiteApplicationHandler Do Work");
            if (!_consortiumService.CheckAccess(Operation.Get, RecordType.Site, ExternalUserId, SiteId, null, consortiumId, OrganizationId))
                return;

            var site = _siteRepository.GetById(new Guid(SiteId));
            var applications = _ahpApplicationRepository.GetByAttribute(invln_scheme.Fields.invln_Site, site.Id)
                .OrderByDescending(x => x.invln_lastexternalmodificationon);
            var filteredApplication = new List<invln_scheme>();
            foreach (var application in applications)
            {
                TracingService.Trace("Start loop for application");
                if (_consortiumService.CheckAccess(Operation.Get, RecordType.Application, ExternalUserId, null, application.Id.ToString(), consortiumId, OrganizationId))
                {
                    filteredApplication.Add(application);
                }
            }
            var siteApplicationDto = SiteApplicationMapper.MapRegularEntityToDto(site, filteredApplication);
            ExecutionData.SetOutputParameter(invln_getsiteapplicationsResponse.Fields.invln_siteapplication, JsonSerializer.Serialize(siteApplicationDto));
        }
    }
}
