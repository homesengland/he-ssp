using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.Api.FrontDoor;
using HE.CRM.Common.Api.FrontDoor.Contract.Responses;
using HE.CRM.Common.Repositories.Interfaces;
using HE.CRM.Plugins.Models.Frontdoor.Mappers;

namespace HE.CRM.Plugins.Services.FrontDoorProject.V2
{
    public class FrontDoorProjectService : CrmService, IFrontDoorProjectService
    {

        #region Fields

        private readonly IFrontDoorApiClient _frontDoorApiClient;

        private readonly IContactRepository _contactRepository;

        private readonly IHeProjectRepository _heProjectRepository;
        private readonly IHeLocalAuthorityRepository _heLocalAuthorityRepository;

        private readonly IGetProjectsResponseMapperService _getProjectsResponseMapperService;
        private readonly IGetProjectResponseMapperService _getProjectResponseMapperService;

        private readonly IContactWebroleRepository _contactWebroleRepository;

        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            //DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

        #endregion

        #region Constructors
        public FrontDoorProjectService(CrmServiceArgs args,
            IGetProjectsResponseMapperService getProjectsResponseMapperService,
            IGetProjectResponseMapperService getProjectResponseMapperService) : base(args)
        {
            _frontDoorApiClient = CrmServicesFactory.Get<IFrontDoorApiClient>();

            _contactRepository = CrmRepositoriesFactory.Get<IContactRepository>();

            _heProjectRepository = CrmRepositoriesFactory.Get<IHeProjectRepository>();
            _heLocalAuthorityRepository = CrmRepositoriesFactory.Get<IHeLocalAuthorityRepository>();

            _getProjectsResponseMapperService = getProjectsResponseMapperService;
            _getProjectResponseMapperService = getProjectResponseMapperService;

            _contactWebroleRepository = CrmRepositoriesFactory.Get<IContactWebroleRepository>();
        }
        #endregion


        public string CreateRecordFromPortal(string externalContactId, string organisationId, string frontDoorProjectId, string entityFieldsParameters)
        {
            Logger.Trace($"FrontDoorProject.V2.{nameof(GetFrontDoorProjects)}");

            var frontDoorProjectFromPortal = JsonSerializer.Deserialize<FrontDoorProjectDto>(entityFieldsParameters, _jsonSerializerOptions);
            Logger.Trace($"frontDoorProjectFromPortal.Local auth code = {frontDoorProjectFromPortal.LocalAuthorityCode}");
            var requestContact = _contactRepository.GetContactViaExternalId(externalContactId);

            var requestObj = HE.CRM.Common.Api.FrontDoor.Mappers.SaveProjectRequestMapper.Map(frontDoorProjectFromPortal, requestContact.Id);
            var request = JsonSerializer.Serialize(requestObj, _jsonSerializerOptions);
            Logger.Trace($"request: {request}");
            var response = _frontDoorApiClient.SaveProject(frontDoorProjectFromPortal, requestContact.Id);

            return response.Result;
        }

        public List<FrontDoorProjectDto> GetFrontDoorProjects(Guid organisationId, string externalContactId = null)
        {
            Logger.Trace($"FrontDoorProject.V2.{nameof(GetFrontDoorProjects)}");

            var baseProjects = _frontDoorApiClient.GetProjects(organisationId);

            Logger.Trace($"GetProjectsResponse contains {baseProjects?.Count} records.");

            if (baseProjects == null || !baseProjects.Any())
            {
                return new List<FrontDoorProjectDto>();
            }

            // Filter projects by record status 'Open'
            var projects = baseProjects.Where(x => x.RecordStatus == (int)he_Pipeline_he_RecordStatus.Open
                && !x.FrontDoorDecision.Contains(134370002));

            // Filter projects by owner if passed external user id
            if (!string.IsNullOrWhiteSpace(externalContactId))
            {
                var currentUser = _contactRepository.GetContactViaExternalId(externalContactId, new string[] { Contact.Fields.Id });
                projects = projects.Where(x => x.PortalOwnerId == currentUser.Id);
                Logger.Trace($"Projects count : {projects.Count()}");
            }

            if (string.IsNullOrWhiteSpace(externalContactId))
            {
                Logger.Trace("Excluding records from the list, which are for a Limited User.");
                Logger.Trace($"Projects count : {projects.Count()}");
                var filteredProjects = new List<GetProjectResponse>();

                var webroleList = _contactWebroleRepository.GetListOfUsersWithoutLimitedRole(organisationId.ToString());
                Logger.Trace($"WebroleList count : {webroleList.Count}");
                var webroleDict = webroleList.ToDictionary(k => k.invln_Contactid.Id);

                foreach (var projectResponse in projects)
                {
                    if (webroleDict.ContainsKey(projectResponse.PortalOwnerId))
                    {
                        filteredProjects.Add(projectResponse);
                    }
                }
                Logger.Trace($"Filtered Projects count : {filteredProjects.Count()}");
                projects = filteredProjects;
            }

            var myProjects = new GetProjectsResponse();

            myProjects.AddRange(projects);
            return _getProjectsResponseMapperService.Map(myProjects).ToList();
        }

        public FrontDoorProjectDto GetFrontDoorProject(Guid organisationId, string externalContactId, Guid frontDoorProjectId, string includeInactive = null)
        {
            Logger.Trace($"FrontDoorProject.V2.{nameof(GetFrontDoorProject)}");

            var project = _frontDoorApiClient.GetProject(frontDoorProjectId);

            if (project == null)
            {
                return null;
            }

            if (project.OrganisationId != organisationId)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(externalContactId))
            {
                var currentUser = _contactRepository.GetContactViaExternalId(externalContactId, new string[] { Contact.Fields.Id });
                if (project.PortalOwnerId != currentUser.Id)
                {
                    return null;
                }
            }

            if (string.IsNullOrEmpty(includeInactive) || includeInactive != "true")
            {
                if (project.RecordStatus.Value != (int)he_Pipeline_he_RecordStatus.Open)
                {
                    return null;
                }
            }

            var contactIds = new List<Guid>()
            {
                project.PortalOwnerId,
                project.FrontDoorProjectContact.ContactId
            };

            var contactsExternalIdMap = _contactRepository.GetContactsMapExternalIds(contactIds.Distinct());

            return _getProjectResponseMapperService.Map(project, contactsExternalIdMap);
        }

        public bool CheckIfFrontDoorProjectWithGivenNameExists(string frontDoorProjectName, Guid organisationId)
        {
            Logger.Trace($"FrontDoorProject.V2.{nameof(CheckIfFrontDoorProjectWithGivenNameExists)}");

            return _frontDoorApiClient.CheckProjectExists(organisationId, frontDoorProjectName);
        }

        public bool DeactivateFrontDoorProject(Guid frontDoorProjectId)
        {
            Logger.Trace($"FrontDoorProject.V2.{nameof(DeactivateFrontDoorProject)}");

            var result = _frontDoorApiClient.DeactivateProject(frontDoorProjectId);
            Logger.Trace($"DeactivateProject result: {result}");
            return true;
        }
    }
}
