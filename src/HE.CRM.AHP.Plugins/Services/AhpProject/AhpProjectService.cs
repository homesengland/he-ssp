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
        private readonly IConsortiumMemberRepository _consortiumMemberRepository;


        public AhpProjectService(CrmServiceArgs args) : base(args)
        {
            _contactRepository = CrmRepositoriesFactory.Get<IContactRepository>();
            _ahpProjectRepository = CrmRepositoriesFactory.Get<IAhpProjectRepository>();
            _siteRepository = CrmRepositoriesFactory.Get<ISiteRepository>();
            _ahpApplicationRepository = CrmRepositoriesFactory.Get<IAhpApplicationRepository>();
            _contactWebroleRepository = CrmRepositoriesFactory.Get<IContactWebroleRepository>();
            _consortiumMemberRepository = CrmRepositoriesFactory.Get<IConsortiumMemberRepository>();
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

        public AhpProjectDto GetAhpProjectWithApplicationsAndSites(string externalContactId, string organisationId, string ahpProjectId, string heProjectId, string consortiumId = null)
        {
            TracingService.Trace("GetAhpProjectWithApplicationsAndSites");
            AhpProjectDto result = null;
            Contact contact = null;
            contact = _contactRepository.GetContactViaExternalId(externalContactId);

            var contactWebRole = CheckContactWebRole(contact, organisationId);

            if (consortiumId == null || (consortiumId != null && contactWebRole == invln_Permission.Limiteduser))
            {
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

            if (consortiumId != null && contactWebRole != invln_Permission.Limiteduser)
            {
                if (IsOrganisationMemberOfConsortium(organisationId, consortiumId) == false)
                {
                    return null;
                }

                var ahpProject = GetAhpProjectForConsortium(ahpProjectId, heProjectId, consortiumId);
                if (ahpProject != null)
                {
                    var listOfApps = _ahpApplicationRepository.GetApplicationsForAhpProject(ahpProject.Id, contactWebRole, contact, new Guid(organisationId), consortiumId);
                    var listOfAppsDto = listOfApps.Select(x => AhpApplicationMapper.MapRegularEntityToDto(x)).ToList();

                    var listOfSites = _siteRepository.GetSitesForAhpProject(ahpProject.Id, contactWebRole, contact, new Guid(organisationId), consortiumId);
                    var listOfSitesAfterChecked = CheckSitesForsConsortium(listOfSites, new Guid(organisationId));
                    var listOfSitesDto = listOfSitesAfterChecked.Select(x => SiteMapper.ToDto(x)).ToList();

                    result = AhpProjectMapper.MapRegularEntityToDto(ahpProject, listOfSitesDto, listOfAppsDto);
                }

                return result;
            }

            TracingService.Trace("Something went wrong? None of the above conditions are met?");
            return result;
        }

        public List<AhpProjectDto> GetAhpProjectsWithSites(string externalContactId, string organisationId, string consortiumId = null)
        {
            TracingService.Trace("GetAhpProjectsWithSites");
            List<AhpProjectDto> result = new List<AhpProjectDto>();
            Contact contact = null;
            contact = _contactRepository.GetContactViaExternalId(externalContactId);

            var contactWebRole = CheckContactWebRole(contact, organisationId);

            if (consortiumId == null || (consortiumId != null && contactWebRole == invln_Permission.Limiteduser))
            {
                var ahpProjects = GetAhpProjectsAndCheckPermission(contactWebRole, contact, organisationId);

                if (ahpProjects != null)
                {
                    ahpProjects.OrderBy(x => x.CreatedOn);
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
                            var SitesDto = AhpProjectMapper.MapRegularEntityToDto(project, listOfSitesDto, null);
                            TracingService.Trace($"Record mapped.");
                            result.Add(SitesDto);
                            TracingService.Trace($"Record added to the list.");
                        }
                    }
                }
            }

            if (consortiumId != null && contactWebRole != invln_Permission.Limiteduser)
            {
                if (IsOrganisationMemberOfConsortium(organisationId, consortiumId) == false)
                {
                    return null;
                }

                var ahpProjects = GetAhpProjectsForConsortium(consortiumId);
                if (ahpProjects != null)
                {
                    ahpProjects.OrderBy(x => x.CreatedOn);
                    TracingService.Trace($"Core records downloaded : {ahpProjects.Count()}");
                    foreach (var project in ahpProjects)
                    {
                        var listOfSites = _siteRepository.GetSitesForAhpProject(project.Id, contactWebRole, contact, new Guid(organisationId), consortiumId);
                        var listOfSitesAfterChecked = CheckSitesForsConsortium(listOfSites, new Guid(organisationId));
                        var listOfSitesDto = listOfSitesAfterChecked.Select(x => SiteMapper.ToDto(x)).ToList();
                        TracingService.Trace($"List of Sites downloaded : {listOfSites.Count()}");
                        var SitesDto = AhpProjectMapper.MapRegularEntityToDto(project, listOfSitesDto, null);
                        TracingService.Trace($"Record mapped.");
                        result.Add(SitesDto);
                        TracingService.Trace($"Record added to the list.");
                    }
                }

            }

            TracingService.Trace("Something went wrong? None of the above conditions are met?");
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

            if (ahpProjectId != null)
            {
                ahpProject = _ahpProjectRepository.GetById(new Guid(ahpProjectId), fieldsAhpProject);
            }
            else if (heProjectId != null)
            {
                ahpProject = _ahpProjectRepository.GetByAttribute(invln_ahpproject.Fields.invln_HeProjectId, new Guid(heProjectId), fieldsAhpProject).First();
            }

            if (ahpProject == null)
            {
                TracingService.Trace("Core record does not exist.");
                return null;
            }
            TracingService.Trace("Core record downloaded");
            ahpProject = CheckAccessToAhpProject(contactWebRole, ahpProject, contact, organisationId);
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

        private List<invln_ahpproject> GetAhpProjectsAndCheckPermission(invln_Permission contactWebRole, Contact contact, string organisationId, string consortiumId = null)
        {
            TracingService.Trace("GetAhpProjectsAndCheckPermission");
            List<invln_ahpproject> ahpProjects = null;
            var fieldsAhpProject = new string[] { invln_ahpproject.Fields.invln_Name, invln_ahpproject.Fields.invln_ahpprojectId, invln_ahpproject.Fields.invln_HeProjectId, invln_ahpproject.Fields.invln_ContactId, invln_ahpproject.Fields.invln_AccountId, invln_ahpproject.Fields.CreatedOn };

            if (contactWebRole == invln_Permission.Limiteduser)
            {
                ahpProjects = _ahpProjectRepository.GetByAttribute(invln_ahpproject.Fields.invln_ContactId, contact.Id, fieldsAhpProject).ToList();
            }
            else
            {
                ahpProjects = _ahpProjectRepository.GetByAttribute(invln_ahpproject.Fields.invln_AccountId, new Guid(organisationId), fieldsAhpProject).ToList();
            }

            if (ahpProjects == null)
            {
                TracingService.Trace("Records do not exist.");
                return null;
            }
            TracingService.Trace("Core records downloaded");

            return ahpProjects;
        }




        private bool IsOrganisationMemberOfConsortium(string organisation, string consortium)
        {
            var consortiumMembers = _consortiumMemberRepository.GetByAttribute(invln_ConsortiumMember.Fields.invln_Consortium, new Guid(consortium)).ToList();
            var consortiumMember = consortiumMembers.Find(x => x.invln_Partner.Id == new Guid(organisation));
            if (consortiumMember == null)
            {
                TracingService.Trace("Organisation is not a member of Consortium");
            }
            else
            {
                TracingService.Trace("Organisation is a member of Consortium");
            }
            return consortiumMember != null;
        }

        private invln_ahpproject GetAhpProjectForConsortium(string ahpProjectId, string heProjectId, string consortiumId)
        {
            TracingService.Trace("GetAhpProjectForConsortium");
            invln_ahpproject ahpProject = null;
            var fieldsAhpProject = new string[] { invln_ahpproject.Fields.invln_Name, invln_ahpproject.Fields.invln_ahpprojectId, invln_ahpproject.Fields.invln_HeProjectId, invln_ahpproject.Fields.invln_ContactId, invln_ahpproject.Fields.invln_AccountId };

            if (ahpProjectId != null)
            {
                ahpProject = _ahpProjectRepository.GetById(new Guid(ahpProjectId), fieldsAhpProject);
            }
            else if (heProjectId != null)
            {
                ahpProject = _ahpProjectRepository.GetByAttribute(invln_ahpproject.Fields.invln_HeProjectId, new Guid(heProjectId), fieldsAhpProject).First();
            }

            if (ahpProject == null)
            {
                TracingService.Trace("Core record does not exist.");
                return null;
            }
            TracingService.Trace("Core record downloaded");

            if (ahpProject.invln_ConsortiumId.Id != new Guid(consortiumId))
            {
                TracingService.Trace("Core record does not belong to specific Consortium.");
                return null;
            }

            return ahpProject;
        }

        private List<invln_Sites> CheckSitesForsConsortium(List<invln_Sites> listOFSites, Guid organisation)
        {
            TracingService.Trace("CheckSitesForsConsortium");
            List<invln_Sites> result = new List<invln_Sites>();
            List<invln_Sites> otherSites = new List<invln_Sites>();
            foreach (var site in listOFSites)
            {
                if (site.invln_developingpartner.Id == organisation || site.invln_ownerofthelandduringdevelopment.Id == organisation || site.invln_Ownerofthehomesaftercompletion.Id == organisation)
                {
                    result.Add(site);
                }
                else
                {
                    otherSites.Add(site);
                }
            }

            var fieldsAhpApp = new string[] { invln_scheme.Fields.invln_DevelopingPartner, invln_scheme.Fields.invln_OwneroftheLand, invln_scheme.Fields.invln_OwneroftheHomes};
            foreach (var site in otherSites)
            {
                var ahpApps = _ahpApplicationRepository.GetByAttribute(invln_scheme.Fields.invln_Site, organisation, fieldsAhpApp);
                foreach (var ahpApp in ahpApps)
                {
                    if(ahpApp.invln_DevelopingPartner.Id == organisation || ahpApp.invln_OwneroftheLand.Id == organisation || ahpApp.invln_OwneroftheHomes.Id == organisation)
                    {
                        result.Add(site);
                        break;
                    }
                }
            }

            return result;
        }

        private List<invln_ahpproject> GetAhpProjectsForConsortium(string consortiumId)
        {
            TracingService.Trace("GetAhpProjectForConsortium");
            List<invln_ahpproject> ahpProjects = null;
            var fieldsAhpProject = new string[] { invln_ahpproject.Fields.invln_Name, invln_ahpproject.Fields.invln_ahpprojectId, invln_ahpproject.Fields.invln_HeProjectId, invln_ahpproject.Fields.invln_ContactId, invln_ahpproject.Fields.invln_AccountId, invln_ahpproject.Fields.CreatedOn };

            ahpProjects = _ahpProjectRepository.GetByAttribute(invln_ahpproject.Fields.invln_ConsortiumId, new Guid(consortiumId), fieldsAhpProject).ToList();

            if (ahpProjects == null)
            {
                TracingService.Trace("Core records does not exist.");
                return null;
            }
            TracingService.Trace("Core records downloaded");

            return ahpProjects;
        }
    }
}
