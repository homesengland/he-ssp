using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Services;
using HE.CRM.Common.Repositories.interfaces;
using HE.CRM.Common.Repositories.Interfaces;
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

        public ConsortiumService(CrmServiceArgs args) : base(args)
        {
            _consortiumRepository = CrmRepositoriesFactory.Get<IConsortiumRepository>();
            _siteRepository = CrmRepositoriesFactory.Get<ISiteRepository>();
            _ahpApplicationRepository = CrmRepositoriesFactory.Get<IAhpApplicationRepository>();
        }

        public bool CheckAccess(Operation operation, RecordType recordtype, string externalUserId, string siteId = null, string applicationId = null, string consortiumId = null, string organizationId = null)
        {
            TracingService.Trace("Get Consortium");
            var consortium = _consortiumRepository.GetById(new Guid(consortiumId), new string[] {
                    invln_Consortium.Fields.invln_LeadPartner});

            bool isLeadPartner = false;
            bool isSidePartner = false;
            bool isAppPartner = false;

            if (applicationId != null)
            {
                TracingService.Trace("Check Access to Application");
                var application = _ahpApplicationRepository.GetById(new Guid(applicationId));
                isLeadPartner = IsConsortiumLeadPartner(consortium, organizationId);
                isAppPartner = IsApplicationPartner(application, organizationId);
            }

            if (siteId != null)
            {
                TracingService.Trace("Check Access to Site");
                isLeadPartner = IsConsortiumLeadPartner(consortium, organizationId);
                isSidePartner = IsOrganizationSitePartner(siteId, organizationId);
            }

            if (isLeadPartner)
                return true;
            if (isSidePartner && operation == Operation.Get && (recordtype == RecordType.Site
                || recordtype == RecordType.Application))
            {
                return true;
            }

            if (isSidePartner && operation == Operation.Get && (recordtype == RecordType.AHPProject
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
            TracingService.Trace("Check web role");
            UserHasAccess(externalUserId, new Guid(organizationId), siteId, applicationId);
            return false;
        }

        private bool IsApplicationPartner(invln_scheme application, string organizationId)
        {
            if (application.invln_DevelopingPartner.Id == new Guid(organizationId) ||
                application.invln_OwneroftheHomes.Id == new Guid(organizationId) ||
                application.invln_OwneroftheLand.Id == new Guid(organizationId))
            {
                return true;
            }
            return false;
        }

        private bool IsOrganizationSitePartner(string siteId, string organizationId)
        {
            TracingService.Trace("IsOrganizationSitePartner");
            var site = _siteRepository.GetById(new Guid(siteId));

            /* if (site.invln_developingpartner != null)
                 if (site.invln_developingpartner.Id == new Guid(organizationId))
                     return true;

             if (site.invln_ownerofthelandduringdevelopment != null)
                 if (site.invln_ownerofthelandduringdevelopment.Id == new Guid(organizationId))
                     return true;

             if (site.invln_Ownerofthehomesaftercompletion != null)
                 if (site.invln_Ownerofthehomesaftercompletion.Id == new Guid(organizationId))
                     return true;*/
            return false;
        }

        private bool IsConsortiumLeadPartner(invln_Consortium consortium, string organizationId)
        {
            TracingService.Trace("IsConsortiumLeadPartner");
            TracingService.Trace($"{consortium.Id}");
            TracingService.Trace($"{consortium.invln_LeadPartner.Id}");
            if (consortium != null)
            {
                if (consortium.invln_LeadPartner.Id == new Guid(organizationId))
                    return true;
            }
            return false;
        }

        private bool UserHasAccess(string externalUserId, Guid organizationId, string siteId = null, string applicationId = null)
        {
            var contact = CrmRepositoriesFactory.Get<IContactRepository>().GetContactViaExternalId(externalUserId);
            List<invln_portalpermissionlevel> ppl = CrmRepositoriesFactory.Get<IPortalPermissionRepository>().GetByAccountAndContact(organizationId, contact.Id);
            int role = GetRole(ppl);
            bool accessToAction = false;
            if (siteId != null)
            {
                accessToAction = hasUserHavePermitionToProvideOperation(Operation.Get, role, siteId);
            }

            if (applicationId != null)
            {
                accessToAction = hasUserHavePermitionToProvideOperation(Operation.Get, role, null, applicationId);
            }

            return accessToAction;
        }

        private bool hasUserHavePermitionToProvideOperation(Operation operation, int role, string siteId = null, string applicationId = null)
        {

            if (role == (int)invln_Permission.Admin)
                return true;
            if (role == (int)invln_Permission.Enhanced)
                return true;
            if (role == (int)invln_Permission.Inputonly && operation != Operation.Submit)
                return true;
            if (role == (int)invln_Permission.Viewonly && operation == Operation.Get)
                return true;
            if (role == (int)invln_Permission.Limiteduser)
            {
                if (siteId != null)
                {
                    var site = _siteRepository.GetById(new Guid(siteId));
                    if (site.invln_CreatedByContactId == null)
                    {

                    }
                }

                if (applicationId != null)
                {
                    var application = _ahpApplicationRepository.GetById(new Guid(applicationId));
                    //  if (application.invln_contactid.id == )
                }
            }

            return false;
        }

        private int GetRole(List<invln_portalpermissionlevel> ppl)
        {
            if (ppl.Any(x => x.invln_Permission.Value == (int)invln_Permission.Admin))
                return (int)invln_Permission.Admin;
            if (ppl.Any(x => x.invln_Permission.Value == (int)invln_Permission.Enhanced))
                return (int)invln_Permission.Admin;
            if (ppl.Any(x => x.invln_Permission.Value == (int)invln_Permission.Inputonly))
                return (int)invln_Permission.Admin;
            if (ppl.Any(x => x.invln_Permission.Value == (int)invln_Permission.Viewonly))
                return (int)invln_Permission.Admin;
            if (ppl.Any(x => x.invln_Permission.Value == (int)invln_Permission.Limiteduser))
                return (int)invln_Permission.Admin;
            return -1;
        }
    }
}
