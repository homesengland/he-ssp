using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Services;
using HE.CRM.Common.Repositories.interfaces;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace HE.CRM.AHP.Plugins.Services.Consortium
{
    public class ConsortiumService : CrmService, IConsortiumService
    {
        public enum Operation
        {
            Get = 1,
            Set = 2,
            Update = 3,
            Submit = 4,
        }

        public enum RecordType
        {
            Site = 1,
            Application = 2,
            AHPProject = 3,
        }

        private readonly IConsortiumRepository _consortiumRepository;
        private readonly ISiteRepository _siteRepository;
        private readonly IAhpApplicationRepository _ahpApplicationRepository;
        private readonly IAhpProjectRepository _ahpProjectRepository;

        public ConsortiumService(CrmServiceArgs args) : base(args)
        {
            _consortiumRepository = CrmRepositoriesFactory.Get<IConsortiumRepository>();
            _siteRepository = CrmRepositoriesFactory.Get<ISiteRepository>();
            _ahpApplicationRepository = CrmRepositoriesFactory.Get<IAhpApplicationRepository>();
            _ahpProjectRepository = CrmRepositoriesFactory.Get<IAhpProjectRepository>();
        }

        public bool CheckAccess(Operation operation, RecordType recordtype, string externalUserId, string siteId = null, string applicationId = null, string consortiumId = null, string organizationId = null, string ahpProject = null)
        {
            TracingService.Trace("Get Consortium");
            if (consortiumId != null)
            {
                var consortium = _consortiumRepository.GetById(new Guid(consortiumId), new string[] {
                    invln_Consortium.Fields.invln_LeadPartner});

                bool isLeadPartner = false;
                bool isSitePartner = false;
                bool isAppPartner = false;

                if (ahpProject != null)
                {

                    var sites = _siteRepository.GetByAttribute(invln_Sites.Fields.invln_AHPProjectId, new Guid(ahpProject)).ToList();
                    isSitePartner = sites.Any(x => IsOrganizationSitePartner(x.Id.ToString(), organizationId);

                    foreach (var site in sites)
                    {
                        var applications = _ahpApplicationRepository.GetByAttribute(invln_scheme.Fields.invln_Site, site.Id);
                        isAppPartner = applications.Any(x => IsApplicationPartner(x, organizationId));
                        if (isAppPartner)
                        {
                            break;
                        }
                    }
                }

                if (applicationId != null)
                {
                    TracingService.Trace("Check Access to Application");
                    var application = _ahpApplicationRepository.GetById(new Guid(applicationId),
                        new string[] {invln_scheme.Fields.invln_DevelopingPartner, invln_scheme.Fields.invln_OwneroftheHomes,
                    invln_scheme.Fields.invln_OwneroftheLand});
                    isLeadPartner = IsConsortiumLeadPartner(consortium, organizationId);
                    isAppPartner = IsApplicationPartner(application, organizationId);
                }

                if (siteId != null)
                {
                    TracingService.Trace("Check Access to Site");
                    isLeadPartner = IsConsortiumLeadPartner(consortium, organizationId);
                    isSitePartner = IsOrganizationSitePartner(siteId, organizationId);
                }

                if (isLeadPartner)
                    return true;
                if (isSitePartner && operation == Operation.Get && (recordtype == RecordType.Site
                    || recordtype == RecordType.Application))
                {
                    return true;
                }

                if (isSitePartner && operation == Operation.Get && (recordtype == RecordType.AHPProject
                    || recordtype == RecordType.Application))
                {
                    return true;
                }

                if (isAppPartner && operation == Operation.Get && (recordtype == RecordType.Site
                    || recordtype == RecordType.Application))
                {
                    return true;
                }

                if (isAppPartner && operation == Operation.Get && (recordtype == RecordType.AHPProject
                    || recordtype == RecordType.Application))
                {
                    return true;
                }
            }

            TracingService.Trace("Check web role");
            return UserHasAccess(externalUserId, new Guid(organizationId), siteId, applicationId, ahpProject);
        }

        private bool IsApplicationPartner(invln_scheme application, string organizationId)
        {
            if (application.invln_DevelopingPartner != null &&
                application.invln_DevelopingPartner.Id == new Guid(organizationId))
            {
                return true;
            }
            if (application.invln_OwneroftheHomes != null &&
                application.invln_OwneroftheHomes.Id == new Guid(organizationId))
            {
                return true;
            }
            if (application.invln_OwneroftheLand != null &&
                application.invln_OwneroftheLand.Id == new Guid(organizationId))
            {
                return true;
            }
            return false;
        }

        private bool IsOrganizationSitePartner(string siteId, string organizationId)
        {
            TracingService.Trace("IsOrganizationSitePartner");
            var site = _siteRepository.GetById(new Guid(siteId),
                new string[] {invln_Sites.Fields.invln_developingpartner, invln_Sites.Fields.invln_ownerofthelandduringdevelopment,
                invln_Sites.Fields.invln_Ownerofthehomesaftercompletion});

            if (site.invln_developingpartner != null)
                if (site.invln_developingpartner.Id == new Guid(organizationId))
                    return true;

            if (site.invln_ownerofthelandduringdevelopment != null)
                if (site.invln_ownerofthelandduringdevelopment.Id == new Guid(organizationId))
                    return true;

            if (site.invln_Ownerofthehomesaftercompletion != null)
                if (site.invln_Ownerofthehomesaftercompletion.Id == new Guid(organizationId))
                    return true;

            TracingService.Trace("IsOrganizationSitePartner return false");
            return false;
        }

        private bool IsConsortiumLeadPartner(invln_Consortium consortium, string organizationId)
        {
            TracingService.Trace("IsConsortiumLeadPartner");
            if (consortium != null)
            {
                if (consortium.invln_LeadPartner.Id == new Guid(organizationId))
                {
                    TracingService.Trace("IsConsortiumLeadPartner return true");
                    return true;
                }

            }
            TracingService.Trace("IsConsortiumLeadPartner return false");
            return false;
        }

        private bool UserHasAccess(string externalUserId, Guid organizationId, string siteId = null, string applicationId = null, string ahpProject = null)
        {
            TracingService.Trace("Get Contact");
            var contact = CrmRepositoriesFactory.Get<IContactRepository>().GetContactViaExternalId(externalUserId);
            TracingService.Trace("Get Portal Permition Level");
            List<invln_portalpermissionlevel> ppl = CrmRepositoriesFactory.Get<IPortalPermissionRepository>().GetByAccountAndContact(organizationId, contact.Id);
            int role = GetRole(ppl);
            bool accessToAction = false;
            if (siteId != null)
            {
                TracingService.Trace("Check For site");
                accessToAction = HasUserHavePermissionToProvideOperation(Operation.Get, role, siteId, null, contact.ToEntityReference());
            }

            if (applicationId != null)
            {
                TracingService.Trace("Check For application");
                accessToAction = HasUserHavePermissionToProvideOperation(Operation.Get, role, null, applicationId, contact.ToEntityReference());
            }

            return accessToAction;
        }

        private bool HasUserHavePermissionToProvideOperation(Operation operation, int role, string siteId = null, string applicationId = null, EntityReference contactId = null, string ahpProject = null)
        {
            if (role == (int)invln_Permission.Admin)
            {
                var ahpProjects = _ahpProjectRepository.GetByAttribute(invln_ahpproject.Fields.invln_ContactId, contactId.Id).ToList();
                if (ahpProjects != null)
                {
                    return ahpProjects.Any(x => x.invln_ContactId != null && x.invln_ContactId.Id == contactId.Id);
                }
            }
            return true;
            if (role == (int)invln_Permission.Enhanced)
            {
                var ahpProjects = _ahpProjectRepository.GetByAttribute(invln_ahpproject.Fields.invln_ContactId, contactId.Id).ToList();
                if (ahpProjects != null)
                {
                    return ahpProjects.Any(x => x.invln_AccountId != null && x.invln_AccountId.Id == contactId.Id);
                }
            }
            if (role == (int)invln_Permission.Inputonly && operation != Operation.Submit)
                return true;
            if (role == (int)invln_Permission.Viewonly && operation == Operation.Get)
                return true;
            if (role == (int)invln_Permission.Limiteduser)
            {
                if (siteId != null)
                {
                    var site = _siteRepository.GetById(new Guid(siteId));
                    if (site.invln_CreatedByContactId != null)
                    {
                        if (site.invln_CreatedByContactId.Equals(contactId))
                        {
                            return true;
                        }
                    }
                }

                if (applicationId != null)
                {
                    var application = _ahpApplicationRepository.GetById(new Guid(applicationId), invln_scheme.Fields.invln_contactid);
                    if (application.invln_contactid.Equals(contactId))
                        return true;
                }

            }

            return false;
        }

        private int GetRole(List<invln_portalpermissionlevel> ppl)
        {
            if (ppl.Any(x => x.invln_Permission.Value == (int)invln_Permission.Admin))
                return (int)invln_Permission.Admin;
            if (ppl.Any(x => x.invln_Permission.Value == (int)invln_Permission.Enhanced))
                return (int)invln_Permission.Enhanced;
            if (ppl.Any(x => x.invln_Permission.Value == (int)invln_Permission.Inputonly))
                return (int)invln_Permission.Inputonly;
            if (ppl.Any(x => x.invln_Permission.Value == (int)invln_Permission.Viewonly))
                return (int)invln_Permission.Viewonly;
            if (ppl.Any(x => x.invln_Permission.Value == (int)invln_Permission.Limiteduser))
                return (int)invln_Permission.Limiteduser;
            return -1;
        }
    }
}
