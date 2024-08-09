using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.Consortium;
using HE.CRM.Common.DtoMapping;
using HE.CRM.Common.Repositories.Interfaces;
using static HE.CRM.AHP.Plugins.Services.Consortium.ConsortiumService;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi.FrontDoor
{
    public class GetSiteApplicationAllocationHandler : CrmActionHandlerBase<invln_getsiteapplicationsallocationsRequest, DataverseContext>
    {
        private string ExternalUserId => ExecutionData.GetInputParameter<string>(invln_getsiteapplicationsallocationsRequest.Fields.invln_userid);
        private Guid OrganizationId => ExecutionData.GetInputParameter<Guid>(invln_getsiteapplicationsallocationsRequest.Fields.invln_accountid);
        private Guid SiteId => ExecutionData.GetInputParameter<Guid>(invln_getsiteapplicationsallocationsRequest.Fields.invln_siteid);

        private readonly ISiteRepository _siteRepository;
        private readonly IConsortiumService _consortiumService;
        private readonly IAhpApplicationRepository _ahpApplicationRepository;
        private readonly IContactRepository _contactRepository;
        private readonly IAhpProjectRepository _projectRepository;
        private readonly IContactWebroleRepository _contactWebroleRepository;

        public GetSiteApplicationAllocationHandler(ISiteRepository siteRepository, IConsortiumService consortiumService, IAhpApplicationRepository ahpApplicationRepository, IContactRepository contactRepository, IAhpProjectRepository projectRepository, IContactWebroleRepository contactWebroleRepository)
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

            return ExternalUserId != null && OrganizationId != null && SiteId != null;
        }

        public override void DoWork()
        {
            string consortiumId = null;
            if (_consortiumService.GetConsortiumIdForSite(SiteId) != Guid.Empty)
            {
                consortiumId = _consortiumService.GetConsortiumIdForSite(SiteId).ToString();
            }

            TracingService.Trace("GetSiteApplicationHandler Do Work");
            if (!_consortiumService.CheckAccess(Operation.Get, RecordType.Site, ExternalUserId, SiteId.ToString(), null, consortiumId, OrganizationId.ToString()))
                return;
            TracingService.Trace("Get Site");
            var site = _siteRepository.GetById(new Guid(SiteId.ToString()));
            TracingService.Trace("Filter Application");
            var applications = _ahpApplicationRepository.GetByAttribute(invln_scheme.Fields.invln_Site, site.Id)
                                .Where(x => x.invln_ExternalStatus.Value != (int)invln_ExternalStatusAHP.Deleted &&
                                x.invln_contactid != null && x.invln_isallocation == false && x.invln_IsLatestVersion == true).ToList()
                                .OrderByDescending(x => x.invln_lastexternalmodificationon).ToList();

            var filteredApplication = new List<invln_scheme>();
            filteredApplication = RecordsFiltering(applications, site, consortiumId);

            TracingService.Trace("Filter Allocation");
            var allocations = _ahpApplicationRepository.GetByAttribute(invln_scheme.Fields.invln_Site, site.Id)
                                .Where(x => x.invln_ExternalStatus.Value != (int)invln_ExternalStatusAHP.Deleted &&
                                x.invln_contactid != null && x.invln_isallocation == true && x.invln_IsLatestVersion == true).ToList()
                                .OrderByDescending(x => x.invln_lastexternalmodificationon).ToList();

            var filteredAllocations = new List<invln_scheme>();
            filteredAllocations = RecordsFiltering(allocations, site, consortiumId);

            var siteApplicationAllocationDto = SiteApplicationAllocationMapper.MapRegularEntityToDto(site, filteredApplication, filteredAllocations);
            ExecutionData.SetOutputParameter(invln_getsiteapplicationsallocationsResponse.Fields.invln_siteapplicationallocation, JsonSerializer.Serialize(siteApplicationAllocationDto));
        }

        private List<invln_scheme> RecordsFiltering(List<invln_scheme> applications, invln_Sites site, string consortiumId)
        {
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
                var application = new List<invln_scheme> { app };
                var applicationsDict = application.ToDictionary(k => k.invln_contactid);
                var webroleList = _contactWebroleRepository.GetListOfUsersWithoutLimitedRole(OrganizationId.ToString());

                var webroleDict = webroleList.ToDictionary(k => k.invln_Contactid);
                foreach (var ap in application)
                {
                    if (webroleDict.ContainsKey(ap.invln_contactid) ||
                        _consortiumService.CheckAccess(ConsortiumService.Operation.Get, ConsortiumService.RecordType.Application,
                            contact.invln_externalid, null, ap.Id.ToString(), consortiumId, OrganizationId.ToString(), null))
                    {
                        filteredApplication.Add(ap);
                    }
                }
            }
            return filteredApplication;
        }
    }
}
