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
using System.Security.Policy;
using HE.CRM.AHP.Plugins.Services.Consortium;

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
        private readonly IHeLocalAuthorityRepository _heLocalAuthorityRepository;
        private readonly IConsortiumRepository _consortiumRepository;
        private readonly IConsortiumService _consortiumService;

        public AhpProjectService(CrmServiceArgs args) : base(args)
        {
            _contactRepository = CrmRepositoriesFactory.Get<IContactRepository>();
            _ahpProjectRepository = CrmRepositoriesFactory.Get<IAhpProjectRepository>();
            _siteRepository = CrmRepositoriesFactory.Get<ISiteRepository>();
            _ahpApplicationRepository = CrmRepositoriesFactory.Get<IAhpApplicationRepository>();
            _contactWebroleRepository = CrmRepositoriesFactory.Get<IContactWebroleRepository>();
            _consortiumMemberRepository = CrmRepositoriesFactory.Get<IConsortiumMemberRepository>();
            _heLocalAuthorityRepository = CrmRepositoriesFactory.Get<IHeLocalAuthorityRepository>();
            _consortiumRepository = CrmRepositoriesFactory.Get<IConsortiumRepository>();
            _consortiumService = CrmServicesFactory.Get<IConsortiumService>();
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
                TracingService.Trace("No Consortium or Consortium with Limiteduser");
                var ahpProject = GetAhpProjectAndCheckPermission(contactWebRole, externalContactId, organisationId, ahpProjectId, heProjectId);

                if (ahpProject != null)
                {
                    var listOfApps = _ahpApplicationRepository.GetApplicationsForAhpProject(ahpProject.Id, contactWebRole, contact, new Guid(organisationId), false, false, consortiumId);
                    var listOfAppsDto = listOfApps.Select(x => AhpApplicationMapper.MapRegularEntityToDto(x)).ToList();

                    var listOfSites = _siteRepository.GetSitesForAhpProject(ahpProject.Id, contactWebRole, contact, new Guid(organisationId), consortiumId);
                    var listOfSitesDto = listOfSites.Select(x => SiteMapper.ToDto(x, GetHeLaForSite(x.invln_HeLocalAuthorityId?.Id.ToString()))).ToList();

                    result = AhpProjectMapper.MapRegularEntityToDto(ahpProject, listOfSitesDto, listOfAppsDto);
                }
                return result;
            }

            if (consortiumId != null && contactWebRole != invln_Permission.Limiteduser)
            {
                TracingService.Trace("Consortium without Limiteduser");
                if (IsOrganisationMemberOfConsortium(organisationId, consortiumId) == false)
                {
                    return null;
                }
                var isALeadPartner = IsOrganisationALeadPartnerOfConsortium(organisationId, consortiumId);
                TracingService.Trace($"Is Organisation a Lead Partner Of Consortium: {isALeadPartner}");

                var ahpProject = GetAhpProjectForConsortium(ahpProjectId, heProjectId, consortiumId);
                if (ahpProject != null)
                {
                    var listOfSites = _siteRepository.GetSitesForAhpProject(ahpProject.Id, contactWebRole, contact, new Guid(organisationId), consortiumId);
                    var isSitePartner = false;
                    foreach (var site in listOfSites)
                    {
                        if (site.invln_developingpartner?.Id == new Guid(organisationId) || site.invln_ownerofthelandduringdevelopment?.Id == new Guid(organisationId) || site.invln_Ownerofthehomesaftercompletion?.Id == new Guid(organisationId))
                        {
                            isSitePartner = true;
                        }
                    }

                    var listOfApps = _ahpApplicationRepository.GetApplicationsForAhpProject(ahpProject.Id, contactWebRole, contact, new Guid(organisationId), isALeadPartner, isSitePartner, consortiumId);
                    TracingService.Trace($"List of Application downloaded. no: {listOfApps.Count}");

                    var filteredList = listOfApps.Where(x => _consortiumService.CheckAccess(ConsortiumService.Operation.Get, ConsortiumService.RecordType.Application,
                        externalContactId, null, x.Id.ToString(), consortiumId, organisationId, null));

                    var listOfAppsDto = filteredList.Select(x => AhpApplicationMapper.MapRegularEntityToDto(x)).ToList();
                    TracingService.Trace($"Records mapped.");

                    TracingService.Trace($"List of Sites downloaded. no: {listOfSites.Count}");
                    var listOfSitesAfterChecked = CheckSitesForsConsortium(listOfSites, new Guid(organisationId), isALeadPartner);
                    var listOfSitesDto = listOfSitesAfterChecked.Select(x => SiteMapper.ToDto(x, GetHeLaForSite(x.invln_HeLocalAuthorityId?.Id.ToString()))).ToList();
                    TracingService.Trace($"Records mapped.");

                    result = AhpProjectMapper.MapRegularEntityToDto(ahpProject, listOfSitesDto, listOfAppsDto);

                }

                return result;
            }

            TracingService.Trace("Something went wrong? None of the above conditions are met?");
            return result;
        }

        public PagedResponseDto<AhpProjectDto> GetAhpProjectsWithSites(string externalContactId, string organisationId, string consortiumId = null, PagingRequestDto paging = null)
        {
            TracingService.Trace("GetAhpProjectsWithSites");

            PagedResponseDto<AhpProjectDto> result = new PagedResponseDto<AhpProjectDto>();
            Contact contact = null;
            contact = _contactRepository.GetContactViaExternalId(externalContactId);

            var contactWebRole = CheckContactWebRole(contact, organisationId);

            if (consortiumId == null || (consortiumId != null && contactWebRole == invln_Permission.Limiteduser))
            {
                TracingService.Trace("No Consortium or Consortium with Limiteduser");
                var ahpProjectsDto = GetAhpProjectsAndCheckPermission(contactWebRole, contact, externalContactId, organisationId, consortiumId, paging, externalContactId);

                if (ahpProjectsDto != null)
                {
                    foreach (var ahpProjectDto in ahpProjectsDto.items)
                    {
                        var listOfSites = _siteRepository.GetSitesForAhpProject(new Guid(ahpProjectDto.AhpProjectId), contactWebRole, contact, new Guid(organisationId), consortiumId);
                        var listOfSitesDto = listOfSites.Select(x => SiteMapper.ToDto(x, GetHeLaForSite(x.invln_HeLocalAuthorityId?.Id.ToString()))).ToList();
                        TracingService.Trace($"List of Sites downloaded.");

                        ahpProjectDto.ListOfSites = listOfSitesDto;
                        TracingService.Trace($"Record added to the list.");
                    }
                    result = ahpProjectsDto;
                }
                return result;
            }

            if (consortiumId != null && contactWebRole != invln_Permission.Limiteduser)
            {
                TracingService.Trace("Consortium without Limiteduser");
                if (IsOrganisationMemberOfConsortium(organisationId, consortiumId) == false)
                {
                    return null;
                }
                var isALeadPartner = IsOrganisationALeadPartnerOfConsortium(organisationId, consortiumId);
                TracingService.Trace($"Is Organisation a Lead Partner Of Consortium: {isALeadPartner}");

                var ahpProjectsDto = GetAhpProjectsForConsortium(consortiumId, paging, externalContactId, organisationId);
                if (ahpProjectsDto != null)
                {
                    foreach (var ahpProjectDto in ahpProjectsDto.items)
                    {
                        TracingService.Trace($"List of Sites searching.");
                        var listOfSites = _siteRepository.GetSitesForAhpProject(new Guid(ahpProjectDto.AhpProjectId), contactWebRole, contact, new Guid(organisationId), consortiumId);
                        TracingService.Trace($"List of Sites downloaded.");
                        var listOfSitesAfterChecked = CheckSitesForsConsortium(listOfSites, new Guid(organisationId), isALeadPartner);
                        var listOfSitesDto = listOfSitesAfterChecked.Select(x => SiteMapper.ToDto(x, GetHeLaForSite(x.invln_HeLocalAuthorityId?.Id.ToString()))).ToList();
                        TracingService.Trace($"Records mapped.");

                        ahpProjectDto.ListOfSites = listOfSitesDto;
                        TracingService.Trace($"Record added to the list.");
                    }
                    result = ahpProjectsDto;
                }
                return result;
            }

            TracingService.Trace("Something went wrong? None of the above conditions are met?");
            return result;
        }

        private invln_Permission CheckContactWebRole(Contact contact, string organisationId)
        {
            TracingService.Trace("CheckContactWebRole");
            var contactWebRole = invln_Permission.Limiteduser;
            if (_contactWebroleRepository.IsContactHaveSelectedWebRoleForOrganisation(contact.Id, new Guid(organisationId), invln_Permission.Limiteduser))
            {
                contactWebRole = invln_Permission.Limiteduser;
                TracingService.Trace("WebRole Limiteduser");
            };
            if (_contactWebroleRepository.IsContactHaveSelectedWebRoleForOrganisation(contact.Id, new Guid(organisationId), invln_Permission.Admin))
            {
                contactWebRole = invln_Permission.Admin;
                TracingService.Trace("WebRole other than Limiteduser");
            };
            TracingService.Trace("WebRole checked");
            return contactWebRole;
        }

        private invln_ahpproject GetAhpProjectAndCheckPermission(invln_Permission contactWebRole, string externalContactId, string organisationId, string ahpProjectId, string heProjectId, string consortiumId = null)
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
                ahpProject = _ahpProjectRepository.GetByAttribute(invln_ahpproject.Fields.invln_HeProjectId, new Guid(heProjectId), fieldsAhpProject).FirstOrDefault();
            }

            if (ahpProject == null)
            {
                TracingService.Trace("Core record does not exist.");
                return null;
            }
            TracingService.Trace("Core record downloaded");
            ahpProject = CheckAccessToAhpProject(contactWebRole, ahpProject, externalContactId, organisationId, consortiumId);

            return ahpProject;
        }

        private invln_ahpproject CheckAccessToAhpProject(invln_Permission contactWebRole, invln_ahpproject ahpProject, string externalContactId, string organisationId, string consortiumId = null)
        {
            if (_consortiumService.CheckAccess(ConsortiumService.Operation.Get, ConsortiumService.RecordType.AHPProject,
                externalContactId, null, null, consortiumId, organisationId, ahpProject.Id.ToString()))
            {
                TracingService.Trace("Access to Core record checked");
                return ahpProject;
            }
            TracingService.Trace("No Access to Core record");
            return null;
        }

        private PagedResponseDto<AhpProjectDto> GetAhpProjectsAndCheckPermission(invln_Permission contactWebRole, Contact contact, string externalContactId, string organisationId, string consortiumId = null, PagingRequestDto paging = null, string externalId = null)
        {
            TracingService.Trace("GetAhpProjectsAndCheckPermission");
            PagedResponseDto<AhpProjectDto> result = new PagedResponseDto<AhpProjectDto>();
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

            ahpProjects = ahpProjects.OrderByDescending(x => x.CreatedOn).ToList();
            TracingService.Trace($"Core records OrderBy.");

            ahpProjects.RemoveAll(x => CheckAccessToAhpProject(contactWebRole, x, externalContactId, organisationId) == null);

            List<invln_ahpproject> filteredProject = new List<invln_ahpproject>();
            foreach (var project in ahpProjects)
            {
                if (_consortiumService.CheckAccess(ConsortiumService.Operation.Get, ConsortiumService.RecordType.AHPProject,
                    externalId, null, null, consortiumId, organisationId, project.Id.ToString()))
                    filteredProject.Add(project);
            }

            if (paging != null)
            {
                TracingService.Trace($"Paging found.");
                result.paging = paging;
                result.totalItemsCount = filteredProject.Count();
                var pageSize = paging.pageSize;
                var pagenumber = paging.pageNumber;
                var startIndex = (pageSize * pagenumber) - pageSize;
                var countrRange = startIndex + pageSize <= (result.totalItemsCount - 1) ? pageSize : result.totalItemsCount - startIndex;

                var ahpProjectsPage = filteredProject.GetRange(startIndex, countrRange);

                var ahpProjectsPageDto = ahpProjectsPage.Select(x => AhpProjectMapper.MapRegularEntityToDto(x)).ToList();
                TracingService.Trace($"Records mapped.");

                result.items = ahpProjectsPageDto;
            }
            else
            {
                var ahpProjectsDto = ahpProjects.Select(x => AhpProjectMapper.MapRegularEntityToDto(x)).ToList();
                TracingService.Trace($"Record mapped.");

                result.items = ahpProjectsDto;
            }

            TracingService.Trace($"Return AhpProjects.");
            return result;
        }

        private bool IsOrganisationMemberOfConsortium(string organisation, string consortium)
        {
            var consortiumCRM = _consortiumRepository.GetById(new Guid(consortium), new string[] { invln_Consortium.Fields.invln_LeadPartner });
            if (consortiumCRM == null)
            {
                TracingService.Trace("Consortium does not exist");
                return false;
            }

            var isOrganisationLeadPartenr = consortiumCRM.invln_LeadPartner.Id == new Guid(organisation);
            var consortiumMembers = _consortiumMemberRepository.GetByAttribute(invln_ConsortiumMember.Fields.invln_Consortium, new Guid(consortium)).ToList();
            invln_ConsortiumMember consortiumMember = null;
            if (consortiumMembers != null)
            {
                TracingService.Trace($"ConsortiumMember downloaded.");
                consortiumMember = consortiumMembers.Find(x => x.invln_Partner.Id == new Guid(organisation));
            }

            if (isOrganisationLeadPartenr == false && consortiumMember == null)
            {
                TracingService.Trace("Organisation is not a member of Consortium");
                return false;
            }

            TracingService.Trace("Organisation is a member of Consortium");
            return true;
        }

        private bool IsOrganisationALeadPartnerOfConsortium(string organisation, string consortium)
        {
            var consortiumCRM = _consortiumRepository.GetById(new Guid(consortium), new string[] { invln_Consortium.Fields.invln_LeadPartner });
            if (consortiumCRM == null)
            {
                TracingService.Trace("Consortium does not exist");
                return false;
            }

            return consortiumCRM.invln_LeadPartner.Id == new Guid(organisation);
        }

        private invln_ahpproject GetAhpProjectForConsortium(string ahpProjectId, string heProjectId, string consortiumId)
        {
            TracingService.Trace("GetAhpProjectForConsortium");
            invln_ahpproject ahpProject = null;
            var fieldsAhpProject = new string[] { invln_ahpproject.Fields.invln_Name, invln_ahpproject.Fields.invln_ahpprojectId, invln_ahpproject.Fields.invln_HeProjectId, invln_ahpproject.Fields.invln_ContactId, invln_ahpproject.Fields.invln_AccountId, invln_ahpproject.Fields.invln_ConsortiumId };

            if (ahpProjectId != null)
            {
                ahpProject = _ahpProjectRepository.GetById(new Guid(ahpProjectId), fieldsAhpProject);
            }
            else if (heProjectId != null)
            {
                ahpProject = _ahpProjectRepository.GetByAttribute(invln_ahpproject.Fields.invln_HeProjectId, new Guid(heProjectId), fieldsAhpProject).FirstOrDefault();
            }

            if (ahpProject == null)
            {
                TracingService.Trace("Core record does not exist.");
                return null;
            }
            TracingService.Trace("Core record downloaded");

            if (ahpProject.invln_ConsortiumId?.Id != new Guid(consortiumId))
            {
                TracingService.Trace("Core record does not belong to specific Consortium.");
                return null;
            }

            return ahpProject;
        }

        private List<invln_Sites> CheckSitesForsConsortium(List<invln_Sites> listOFSites, Guid organisation, bool isALeadPartner)
        {
            TracingService.Trace("CheckSitesForsConsortium");
            if (isALeadPartner)
            {
                TracingService.Trace("Sites Fors Consortium Checked");
                return listOFSites;
            }

            List<invln_Sites> result = new List<invln_Sites>();
            List<invln_Sites> otherSites = new List<invln_Sites>();
            foreach (var site in listOFSites)
            {
                if (site.invln_developingpartner?.Id == organisation || site.invln_ownerofthelandduringdevelopment?.Id == organisation || site.invln_Ownerofthehomesaftercompletion?.Id == organisation)
                {
                    result.Add(site);
                }
                else
                {
                    otherSites.Add(site);
                }
            }

            var fieldsAhpApp = new string[] { invln_scheme.Fields.invln_DevelopingPartner, invln_scheme.Fields.invln_OwneroftheLand, invln_scheme.Fields.invln_OwneroftheHomes };
            foreach (var site in otherSites)
            {
                var ahpApps = _ahpApplicationRepository.GetByAttribute(invln_scheme.Fields.invln_Site, site.Id, fieldsAhpApp);
                foreach (var ahpApp in ahpApps)
                {
                    if (ahpApp.invln_DevelopingPartner?.Id == organisation || ahpApp.invln_OwneroftheLand?.Id == organisation || ahpApp.invln_OwneroftheHomes?.Id == organisation)
                    {
                        result.Add(site);
                        break;
                    }
                }
            }
            TracingService.Trace("Sites Fors Consortium Checked");
            return result;
        }

        private PagedResponseDto<AhpProjectDto> GetAhpProjectsForConsortium(string consortiumId, PagingRequestDto paging, string externalId, string organisationId)
        {
            TracingService.Trace("GetAhpProjectsForConsortium");
            PagedResponseDto<AhpProjectDto> result = new PagedResponseDto<AhpProjectDto>();
            var fieldsAhpProject = new string[] { invln_ahpproject.Fields.invln_Name, invln_ahpproject.Fields.invln_ahpprojectId, invln_ahpproject.Fields.invln_HeProjectId, invln_ahpproject.Fields.invln_ContactId, invln_ahpproject.Fields.invln_AccountId, invln_ahpproject.Fields.CreatedOn };

            var ahpProjects = _ahpProjectRepository.GetByAttribute(invln_ahpproject.Fields.invln_ConsortiumId, new Guid(consortiumId), fieldsAhpProject).ToList();

            if (ahpProjects == null)
            {
                TracingService.Trace("Core records does not exist.");
                return null;
            }
            TracingService.Trace("Core records downloaded");

            ahpProjects = ahpProjects.OrderByDescending(x => x.CreatedOn).ToList();
            List<invln_ahpproject> filteredProject = new List<invln_ahpproject>();
            foreach (var project in ahpProjects)
            {
                if (_consortiumService.CheckAccess(ConsortiumService.Operation.Get, ConsortiumService.RecordType.AHPProject,
                    externalId, null, null, consortiumId, organisationId, project.Id.ToString()))
                    filteredProject.Add(project);
            }

            TracingService.Trace($"Core records OrderBy.");
            if (paging != null)
            {
                TracingService.Trace($"Paging found.");
                result.paging = paging;
                result.totalItemsCount = filteredProject.Count();
                var pageSize = paging.pageSize;
                var pagenumber = paging.pageNumber;
                var startIndex = (pageSize * pagenumber) - pageSize;
                var countrRange = startIndex + pageSize <= (result.totalItemsCount - 1) ? pageSize : result.totalItemsCount - startIndex;

                var ahpProjectsPage = filteredProject.GetRange(startIndex, countrRange);

                var ahpProjectsPageDto = ahpProjectsPage.Select(x => AhpProjectMapper.MapRegularEntityToDto(x)).ToList();
                TracingService.Trace($"Records mapped.");

                result.items = ahpProjectsPageDto;
            }
            else
            {
                var ahpProjectsDto = ahpProjects.Select(x => AhpProjectMapper.MapRegularEntityToDto(x)).ToList();
                TracingService.Trace($"Records mapped.");

                result.items = ahpProjectsDto;
            }

            TracingService.Trace($"Return AhpProjects for Consortium.");
            return result;
        }

        private he_LocalAuthority GetHeLaForSite(string id = null)
        {
            if (id == null)
            {
                return null;
            }
            return _heLocalAuthorityRepository.GetById(new Guid(id), new string[] { nameof(he_LocalAuthority.he_LocalAuthorityId).ToLower(), nameof(he_LocalAuthority.he_Name).ToLower(), nameof(he_LocalAuthority.he_GSSCode).ToLower() });

        }
    }
}
