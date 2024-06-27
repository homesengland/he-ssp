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
    public class GetSiteApplicationHandler : CrmActionHandlerBase<invln_siteapplicationGetRequest, DataverseContext>
    {
        private string ExternalUserId => ExecutionData.GetInputParameter<string>(invln_siteapplicationGetRequest.Fields.invln_userid);
        private string OrganizationId => ExecutionData.GetInputParameter<string>(invln_siteapplicationGetRequest.Fields.invln_organizationid);
        private string SiteId => ExecutionData.GetInputParameter<string>(invln_siteapplicationGetRequest.Fields.invln_siteid);
        private string ConsortiumId => ExecutionData.GetInputParameter<string>(invln_siteapplicationGetRequest.Fields.invln_consortiumid);

        private readonly ISiteRepository _siteRepository;
        private readonly IConsortiumService _consortiumService;
        private readonly IAhpApplicationRepository _ahpApplicationRepository;
        private readonly IContactRepository _contactRepository;
        private readonly IAhpProjectRepository _projectRepository;
        private readonly IContactWebroleRepository _contactWebroleRepository;

        public GetSiteApplicationHandler(ISiteRepository siteRepository,
                IConsortiumService consortiumService, IAhpApplicationRepository ahpApplicationRepository, IContactRepository contactRepository, IAhpProjectRepository projectRepository, IContactWebroleRepository contactWebroleRepository)
        {
            _siteRepository = siteRepository;
            _consortiumService = consortiumService;
            _ahpApplicationRepository = ahpApplicationRepository;
            _contactRepository = contactRepository;
            _projectRepository = projectRepository;
            _contactWebroleRepository = contactWebroleRepository;
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
            TracingService.Trace("Get Site");
            var site = _siteRepository.GetById(new Guid(SiteId));
            TracingService.Trace("Filter Application");
            var applications = _ahpApplicationRepository.GetByAttribute(invln_scheme.Fields.invln_Site, site.Id)
                                .Where(x => x.invln_ExternalStatus.Value != (int)invln_ExternalStatusAHP.Deleted &&
                                x.invln_contactid != null).ToList()
                                .OrderByDescending(x => x.invln_lastexternalmodificationon).ToList();

            var filteredApplication = new List<invln_scheme>();
            TracingService.Trace($"Iteration by application no: {applications.Count}");
            foreach (var app in applications)
            {
                TracingService.Trace($"Excluding records from the list, which are for a Limited User.");
                var contact = _contactRepository.GetById(app.invln_contactid.Id, new string[] { Contact.Fields.FirstName, Contact.Fields.LastName, nameof(Contact.invln_externalid).ToLower() });
                if (app.invln_Site != null)
                {
                    TracingService.Trace($"Get Site");
                    if (site.invln_AHPProjectId != null)
                    {
                        TracingService.Trace($"Get Project");
                        var ahpProject = _projectRepository.GetById(site.invln_AHPProjectId.Id, invln_ahpproject.Fields.invln_ConsortiumId);
                        if (ahpProject != null && ahpProject.invln_ConsortiumId != null)
                        {
                            TracingService.Trace($"Set value - consortium");
                            consortiumId = ahpProject.invln_ConsortiumId.Id.ToString();
                        }
                    }
                }
                var applicationn = new List<invln_scheme>
                    {
                        app
                    };
                var applicationsDict = applicationn.ToDictionary(k => k.invln_contactid);
                var webroleList = _contactWebroleRepository.GetListOfUsersWithoutLimitedRole(OrganizationId);
                TracingService.Trace($"WebroleList count : {webroleList.Count}");
                var webroleDict = webroleList.ToDictionary(k => k.invln_Contactid);
                var d1 = applicationsDict
                    .Where(x => webroleDict.ContainsKey(x.Key) ||
                    _consortiumService.CheckAccess(ConsortiumService.Operation.Get, ConsortiumService.RecordType.Application,
                    contact.invln_externalid, null, x.Value.Id.ToString(), consortiumId, OrganizationId, null))
                    .ToDictionary(x => x.Key, x => x.Value);
                filteredApplication.AddRange(d1.Values.ToList());
            }
            var siteApplicationDto = SiteApplicationMapper.MapRegularEntityToDto(site, filteredApplication);
            ExecutionData.SetOutputParameter(invln_getsiteapplicationsResponse.Fields.invln_siteapplication, JsonSerializer.Serialize(siteApplicationDto));
        }
    }
}
