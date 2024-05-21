using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Services;
using HE.CRM.Common.dtomapping;
using HE.CRM.Common.Repositories.Interfaces;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using Microsoft.Xrm.Sdk;
using System.IdentityModel.Metadata;
using Microsoft.Xrm.Sdk.Query;
using HE.CRM.Common.DtoMapping;

namespace HE.CRM.AHP.Plugins.Services.AhpProject
{
    public class AhpProjectService : CrmService, IAhpProjectService
    {

        private readonly IContactRepository _contactRepository;
        private readonly IAhpProjectRepository _ahpProjectRepository;
        private readonly ISiteRepository _siteRepository;
        private readonly IAhpApplicationRepository _ahpApplicationRepository;
        private readonly IContactWebroleRepository _contactWebroleRepository;


        public AhpProjectService(CrmServiceArgs args) : base(args)
        {
            _contactRepository = CrmRepositoriesFactory.Get<IContactRepository>();
            _ahpProjectRepository = CrmRepositoriesFactory.Get<IAhpProjectRepository>();
            _siteRepository = CrmRepositoriesFactory.Get<ISiteRepository>();
            _ahpApplicationRepository = CrmRepositoriesFactory.Get<IAhpApplicationRepository>();
            _contactWebroleRepository = CrmRepositoriesFactory.Get<IContactWebroleRepository>();
        }

        public string CreateRecordFromPortal(string externalContactId, string organisationId, string heProjectId, string ahpProjectName, string consortiumId)
        {
            TracingService.Trace("CreateRecordFromPortal");
            Contact createdByContact = null;
            if (!string.IsNullOrEmpty(externalContactId))
            {
                createdByContact = _contactRepository.GetContactViaExternalId(externalContactId);
            }

            var entity = AhpProjectMapper.ToEntity(createdByContact, organisationId, heProjectId, ahpProjectName, consortiumId);
            var id = _ahpProjectRepository.Create(entity);
            return id.ToString();
        }

        public AhpProjectDto GetAhpProjectWithApplicationsAndSites(string externalContactId, string organisationId, string ahpProjectId, string heProjectId, string consortiumId)
        {
            TracingService.Trace("GetAhpProjectWithApplicationsAndSites");
            AhpProjectDto result = null;
            Contact contact = null;
            contact = _contactRepository.GetContactViaExternalId(externalContactId);

            var contactWebRole = CheckContactWebRole(contact, organisationId);
            var ahpProject = GetAhpProjectAndCheckPermission(contactWebRole, contact, organisationId, ahpProjectId, heProjectId);

            if (ahpProject != null)
            {
                var listOfApps = _ahpApplicationRepository.GetApplicationsForAhpProject(ahpProject.Id, contactWebRole, contact, new Guid(organisationId), consortiumId);
                var listOfAppsDto = listOfApps.Select(x => AhpApplicationMapper.MapRegularEntityToDto(x)).ToList();

                var listOfSites = _siteRepository.GetSitesForAhpProject(ahpProject.Id, contactWebRole, contact, new Guid(organisationId), consortiumId);
                var listOfSitesDto = listOfSites.Select(x => SiteMapper.ToDto(x)).ToList();

                result = AhpProjectMapper.MapRegularEntityToDto(ahpProject, listOfSitesDto, listOfAppsDto);
            }
            return result;

        }

        public List<AhpProjectDto> GetAhpProjectsWithSites(string externalContactId, string organisationId, string consortiumId)
        {
            TracingService.Trace("GetAhpProjectsWithSites");
            List<AhpProjectDto> result = null;
            Contact contact = null;
            contact = _contactRepository.GetContactViaExternalId(externalContactId);

            var contactWebRole = CheckContactWebRole(contact, organisationId);

            result = GetAhpProjectsAndCheckPermission(contactWebRole, contact, organisationId);

            return result;
        }

        private invln_Permission CheckContactWebRole(Contact contact, string organisationId)
        {
            var contactWebRole = invln_Permission.Limiteduser;
            if (_contactWebroleRepository.IsContactHaveSelectedWebRoleForOrganisation(contact.Id, new Guid(organisationId), invln_Permission.Limiteduser))
            {
                contactWebRole = invln_Permission.Limiteduser;
            };
            if (!_contactWebroleRepository.IsContactHaveSelectedWebRoleForOrganisation(contact.Id, new Guid(organisationId), invln_Permission.Limiteduser))
            {
                contactWebRole = invln_Permission.Admin;
            };
            TracingService.Trace("WebRole checked");
            return contactWebRole;
        }

        private invln_ahpproject GetAhpProjectAndCheckPermission(invln_Permission contactWebRole, Contact contact, string organisationId, string ahpProjectId, string heProjectId)
        {
            TracingService.Trace("GetAhpProjectAndCheckPermission");
            invln_ahpproject ahpProject = null;
            var fieldsAhpProject = new string[] { invln_ahpproject.Fields.invln_Name, invln_ahpproject.Fields.invln_ahpprojectId, invln_ahpproject.Fields.invln_HeProjectId, invln_ahpproject.Fields.invln_ContactId, invln_ahpproject.Fields.invln_AccountId };

            if (ahpProjectId != null && Guid.TryParse(ahpProjectId, out Guid ahpProjectGuid))
            {
                ahpProject = _ahpProjectRepository.GetById(ahpProjectGuid, fieldsAhpProject);
            }
            else if (heProjectId != null && Guid.TryParse(heProjectId, out Guid heProjectGuid))
            {
                ahpProject = _ahpProjectRepository.GetByAttribute(invln_ahpproject.Fields.invln_HeProjectId, heProjectGuid, fieldsAhpProject).First();
            }
            TracingService.Trace("Core record downloaded");

            if (ahpProject != null)
            {
                ahpProject = CheckAccessToAhpProject(contactWebRole, ahpProject, contact, organisationId);
            }
            else
            {
                TracingService.Trace("Core record does not exist.");
                return null;
            }
            TracingService.Trace("Access to Core record checked");

            return ahpProject;
        }

        private invln_ahpproject CheckAccessToAhpProject(invln_Permission contactWebRole, invln_ahpproject ahpProject, Contact contact, string organisationId)
        {
            if (contactWebRole == invln_Permission.Limiteduser && ahpProject.invln_ContactId.Id != contact.Id)
            {
                TracingService.Trace("The user does not have access to AhpProject");
                return null;
            }
            if (contactWebRole != invln_Permission.Limiteduser && ahpProject.invln_AccountId.Id != new Guid(organisationId))
            {
                TracingService.Trace("The user does not have access to AhpProject. Contact does not belong to organization");
                return null;
            }
            TracingService.Trace("Access to Core record checked");
            return ahpProject;
        }

        private List<AhpProjectDto> GetAhpProjectsAndCheckPermission(invln_Permission contactWebRole, Contact contact, string organisationId, string consortiumId = null)
        {
            TracingService.Trace("GetAhpProjectsAndCheckPermission");
            List<invln_ahpproject> ahpProjects = null;
            List<AhpProjectDto> ahpProjectsDto = new List<AhpProjectDto>();

            var fieldsAhpProject = new string[] { invln_ahpproject.Fields.invln_Name, invln_ahpproject.Fields.invln_ahpprojectId, invln_ahpproject.Fields.invln_HeProjectId, invln_ahpproject.Fields.invln_ContactId, invln_ahpproject.Fields.invln_AccountId };

            if (contactWebRole == invln_Permission.Limiteduser)
            {
                ahpProjects = _ahpProjectRepository.GetByAttribute(invln_ahpproject.Fields.invln_ContactId, contact.Id, fieldsAhpProject).ToList();
            }
            else
            {
                ahpProjects = _ahpProjectRepository.GetByAttribute(invln_ahpproject.Fields.invln_AccountId, new Guid(organisationId), fieldsAhpProject).ToList();
            }
            
            if (ahpProjects != null)
            {
                TracingService.Trace($"Core records downloaded : {ahpProjects.Count()}");
                foreach (var project in ahpProjects)
                {
                    if (CheckAccessToAhpProject(contactWebRole, project, contact, organisationId) == null)
                    {
                        TracingService.Trace("Access to AhpProject checked");
                        ahpProjects.Remove(project);
                    }
                    else
                    {
                        var listOfSites = _siteRepository.GetSitesForAhpProject(project.Id, contactWebRole, contact, new Guid(organisationId), consortiumId);
                        var listOfSitesDto = listOfSites.Select(x => SiteMapper.ToDto(x)).ToList();
                        TracingService.Trace($"List of Sites downloaded : {listOfSites.Count()}");
                        TracingService.Trace($"**** : {project.Id}  / {project.invln_Name}");
                        var rec = AhpProjectMapper.MapRegularEntityToDto(project, listOfSitesDto, null);
                        TracingService.Trace($"Record mapped.");
                        ahpProjectsDto.Add(rec);
                        TracingService.Trace($"Record added to the list.");
                    }
                }
            }
            else
            {
                TracingService.Trace("Records do not exist.");
                return null;
            }
            return ahpProjectsDto;
        }
    }
}
