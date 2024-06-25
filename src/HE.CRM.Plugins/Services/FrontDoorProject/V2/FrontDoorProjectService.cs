using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using DataverseModel;
using HE.Base.Log;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.Api;
using HE.CRM.Common.Api.Auth;
using HE.CRM.Common.Api.Auth.AzureAd;
using HE.CRM.Common.DtoMapping;
using HE.CRM.Common.Repositories.Interfaces;
using HE.CRM.Common.Services.Interfaces;
using HE.CRM.Plugins.Handlers.SiteDetails;
using HE.CRM.Plugins.Models.Frontdoor.Mappers;
using HE.CRM.Plugins.Models.FrontDoor;
using HE.CRM.Plugins.Models.FrontDoor.Contract.Responses;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Plugins.Services.FrontDoorProject.V2
{
    public class FrontDoorProjectService : CrmService, IFrontDoorProjectService
    {

        #region Fields

        private readonly ApiHttpClient _httpClient;

        private readonly IFrontDoorProjectRepository _frontDoorProjectRepository;
        private readonly IContactRepository _contactRepository;
        private readonly ILocalAuthorityRepository _localAuthorityRepository;

        private readonly IHeProjectRepository _heProjectRepository;
        private readonly IHeLocalAuthorityRepository _heLocalAuthorityRepository;

        private readonly IGetProjectsResponseMapperService _getProjectsResponseMapperService;

        #endregion

        #region Constructors
        public FrontDoorProjectService(CrmServiceArgs args,
            IGetProjectsResponseMapperService getProjectsResponseMapperService) : base(args)
        {
            _httpClient = CreateHttpClient(
                CrmRepositoriesFactory.Get<ISecretVariableRepository>(),
                CrmServicesFactory.Get<IAzureAdTokenProviderFactory>());

            _frontDoorProjectRepository = CrmRepositoriesFactory.Get<IFrontDoorProjectRepository>();
            _contactRepository = CrmRepositoriesFactory.Get<IContactRepository>();
            _localAuthorityRepository = CrmRepositoriesFactory.Get<ILocalAuthorityRepository>();

            _heProjectRepository = CrmRepositoriesFactory.Get<IHeProjectRepository>();
            _heLocalAuthorityRepository = CrmRepositoriesFactory.Get<IHeLocalAuthorityRepository>();

            _getProjectsResponseMapperService = getProjectsResponseMapperService;
        }
        #endregion


        public string CreateRecordFromPortal(string externalContactId, string organisationId, string frontDoorProjectId, string entityFieldsParameters, bool useHeTables)
        {
            Guid frontdoorprojectGUID = Guid.NewGuid();
            this.TracingService.Trace("entityFieldsParameters:" + entityFieldsParameters);
            FrontDoorProjectDto frontDoorProjectFromPortal = JsonSerializer.Deserialize<FrontDoorProjectDto>(entityFieldsParameters);
            var requestContact = _contactRepository.GetContactViaExternalId(externalContactId);

            if (useHeTables)
            {
                if (frontDoorProjectFromPortal.LocalAuthorityCode != null)
                {
                    var localAuthorityGUID = _heLocalAuthorityRepository.GetLocalAuthorityWithGivenCode(frontDoorProjectFromPortal.LocalAuthorityCode)?.Id;
                    if (localAuthorityGUID != null)
                    {
                        frontDoorProjectFromPortal.LocalAuthority = localAuthorityGUID.ToString();
                    }
                    else
                    {
                        frontDoorProjectFromPortal.LocalAuthority = null;
                    }
                }
                else
                {
                    frontDoorProjectFromPortal.LocalAuthority = null;
                }

                var frontDoorProjecToCreate = FrontDoorProjectMapper.MapDtoToProjectEntity(frontDoorProjectFromPortal, requestContact, organisationId);

                // Update Or Create a Project Record
                if (!string.IsNullOrEmpty(frontDoorProjectId) && Guid.TryParse(frontDoorProjectId, out Guid projectId))
                {
                    this.TracingService.Trace("Update Project");
                    frontdoorprojectGUID = projectId;
                    frontDoorProjecToCreate.Id = projectId;
                    _heProjectRepository.Update(frontDoorProjecToCreate);
                    this.TracingService.Trace("After update record");
                }
                else
                {
                    this.TracingService.Trace("Create Project");
                    frontdoorprojectGUID = _heProjectRepository.Create(frontDoorProjecToCreate);
                    this.TracingService.Trace("After create record");
                }
            }
            else
            {
                if (frontDoorProjectFromPortal.LocalAuthorityCode != null)
                {
                    var localAuthorityGUID = _localAuthorityRepository.GetLocalAuthorityWithGivenOnsCode(frontDoorProjectFromPortal.LocalAuthorityCode)?.Id;
                    if (localAuthorityGUID != null)
                    {
                        frontDoorProjectFromPortal.LocalAuthority = localAuthorityGUID.ToString();
                    }
                    else
                    {
                        frontDoorProjectFromPortal.LocalAuthority = null;
                    }
                }
                else
                {
                    frontDoorProjectFromPortal.LocalAuthority = null;
                }

                var frontDoorProjecToCreate = FrontDoorProjectMapper.MapDtoToRegularEntity(frontDoorProjectFromPortal, requestContact, organisationId);

                // Update Or Create a FrontDoorProject Record
                if (!string.IsNullOrEmpty(frontDoorProjectId) && Guid.TryParse(frontDoorProjectId, out Guid projectId))
                {
                    this.TracingService.Trace("Update FrontDoorProjectPOC");
                    frontdoorprojectGUID = projectId;
                    frontDoorProjecToCreate.Id = projectId;
                    _frontDoorProjectRepository.Update(frontDoorProjecToCreate);
                    this.TracingService.Trace("After update record");
                }
                else
                {
                    this.TracingService.Trace("Create FrontDoorProject");
                    frontdoorprojectGUID = _frontDoorProjectRepository.Create(frontDoorProjecToCreate);
                    this.TracingService.Trace("After create record");
                }
            }

            return frontdoorprojectGUID.ToString();
        }

        public List<FrontDoorProjectDto> GetFrontDoorProjects(Guid organisationId)
        {
            Logger.Trace($"{nameof(FrontDoorProjectService)}.{nameof(GetFrontDoorProjects)}");

            //var externalContactIdGuid = string.IsNullOrEmpty(externalContactId) ? Guid.Empty : Guid.Parse(externalContactId);

            var baseProjects = _httpClient.Send<GetProjectsResponse>(
                FrontDoorApiUrls.GetProjects(organisationId),
                HttpMethod.Get);

            Logger.Trace($"GetProjectsResponse contains {baseProjects.Count} records.");

            // Show only 'Open' projects
            var projects = baseProjects.Where(x => x.RecordStatus == 134370000).ToList();

            var myProjects = new GetProjectsResponse();

            myProjects.AddRange(projects);
            return _getProjectsResponseMapperService.Map(myProjects).ToList();
        }

        public FrontDoorProjectDto GetFrontDoorProject(Guid organisationId, string externalContactId, Guid frontDoorProjectId, string includeInactive = null)
        {
            Logger.Trace($"{nameof(FrontDoorProjectService)}.{nameof(GetFrontDoorProject)}");

            var externalContactIdGuid = string.IsNullOrEmpty(externalContactId) ? Guid.Empty : Guid.Parse(externalContactId);

            var project = _httpClient.Send<GetProjectResponse>(
                FrontDoorApiUrls.GetProject(frontDoorProjectId),
                HttpMethod.Get);

            //if (!string.IsNullOrEmpty(externalContactId))
            //{
            //    var contact = _contactRepository.GetContactViaExternalId(externalContactId, new string[] { });
            //    var contactId = contact != null ? contact.Id : Guid.Empty;

            //    myProjects.AddRange(projects.Where(x => x.PortalOwnerId == contactId));

            //    return _getProjectsResponseMapperService.Map(myProjects).ToList();
            //}

            return _getProjectsResponseMapperService.Map(new GetProjectsResponse { project }).First();
        }

        public List<FrontDoorProjectDto> GetFrontDoorProjects_Old(string organisationId, bool useHeTables, string externalContactId = null, string fieldsToRetrieve = null, string frontDoorProjectId = null, string includeInactive = null)
        {
            this.TracingService.Trace($"GetFrontDoorProjects   useHeTables:  {useHeTables}");
            var listOfFrontDoorProjects = new List<FrontDoorProjectDto>();
            var contactExternalIdFilter = GetFetchXmlConditionForGivenField(externalContactId, nameof(Contact.invln_externalid).ToLower());
            contactExternalIdFilter = GenerateFilterMarksForCondition(contactExternalIdFilter);

            if (useHeTables)
            {
                var organisationCondition = GetFetchXmlConditionForGivenField(organisationId, nameof(he_Pipeline.he_Account).ToLower());
                var frontDoorProjectCondition = GetFetchXmlConditionForGivenField(frontDoorProjectId, nameof(he_Pipeline.he_PipelineId).ToLower());

                string recordStatusCondition = GetFetchXmlConditionForGivenField(((int)he_Pipeline_he_RecordStatus.Open).ToString(), nameof(he_Pipeline.he_RecordStatus).ToLower());
                if (!string.IsNullOrEmpty(includeInactive) && includeInactive == "true")
                {
                    recordStatusCondition = null;
                }

                var frontDoorProjects = _heProjectRepository.GetHeProject(organisationCondition, contactExternalIdFilter, frontDoorProjectCondition, recordStatusCondition);
                if (frontDoorProjects.Any())
                {
                    foreach (var frontDoorProject in frontDoorProjects)
                    {
                        var contact = _contactRepository.GetById(frontDoorProject.he_portalowner.Id, new string[] { nameof(Contact.FirstName).ToLower(), nameof(Contact.LastName).ToLower(), nameof(Contact.invln_externalid).ToLower(), nameof(Contact.EMailAddress1).ToLower(), nameof(Contact.Telephone1).ToLower() });
                        he_LocalAuthority localauthority = new he_LocalAuthority();
                        if (frontDoorProject.he_projectbelocated != null)
                        {
                            localauthority = _heLocalAuthorityRepository.GetById(frontDoorProject.he_projectbelocated.Id, new string[] { nameof(he_LocalAuthority.he_LocalAuthorityId).ToLower(), nameof(he_LocalAuthority.he_Name).ToLower(), nameof(he_LocalAuthority.he_GSSCode).ToLower() });
                        }
                        var frontDoorProjecDto = FrontDoorProjectMapper.MapHeProjectEntityToDto(frontDoorProject, contact, localauthority);
                        listOfFrontDoorProjects.Add(frontDoorProjecDto);
                    }
                }
            }
            else
            {
                var organisationCondition = GetFetchXmlConditionForGivenField(organisationId, nameof(invln_FrontDoorProjectPOC.invln_AccountId).ToLower());
                string attributes = null;
                if (!string.IsNullOrEmpty(fieldsToRetrieve))
                {
                    attributes = GenerateFetchXmlAttributes(fieldsToRetrieve);
                }
                var frontDoorProjectCondition = GetFetchXmlConditionForGivenField(frontDoorProjectId, nameof(invln_FrontDoorProjectPOC.invln_FrontDoorProjectPOCId).ToLower());

                string statecodeCondition = GetFetchXmlConditionForGivenField("0", nameof(invln_FrontDoorProjectPOC.StateCode).ToLower());
                if (!string.IsNullOrEmpty(includeInactive) && includeInactive == "true")
                {
                    statecodeCondition = null;
                }

                var frontDoorProjects = _frontDoorProjectRepository.GetFrontDoorProjectForOrganisationAndContact(organisationCondition, contactExternalIdFilter, attributes, frontDoorProjectCondition, statecodeCondition);
                if (frontDoorProjects.Any())
                {
                    foreach (var frontDoorProject in frontDoorProjects)
                    {
                        var contact = _contactRepository.GetById(frontDoorProject.invln_ContactId.Id, new string[] { nameof(Contact.FirstName).ToLower(), nameof(Contact.LastName).ToLower(), nameof(Contact.invln_externalid).ToLower(), nameof(Contact.EMailAddress1).ToLower(), nameof(Contact.Telephone1).ToLower() });
                        invln_localauthority localauthority = new invln_localauthority();
                        if (frontDoorProject.invln_LocalAuthorityId != null)
                        {
                            localauthority = _localAuthorityRepository.GetById(frontDoorProject.invln_LocalAuthorityId.Id, new string[] { nameof(invln_localauthority.invln_localauthorityId).ToLower(), nameof(invln_localauthority.invln_localauthorityname).ToLower(), nameof(invln_localauthority.invln_onscode).ToLower() });
                        }
                        var frontDoorProjecDto = FrontDoorProjectMapper.MapRegularEntityToDto(frontDoorProject, contact, localauthority);
                        listOfFrontDoorProjects.Add(frontDoorProjecDto);
                    }
                }
            }

            return listOfFrontDoorProjects;
        }

        public bool CheckIfFrontDoorProjectWithGivenNameExists(string frontDoorProjectName, bool useHeTables, string organisationId)
        {
            if (useHeTables)
            {
                if (!string.IsNullOrEmpty(organisationId) && Guid.TryParse(organisationId, out Guid orgId))
                {
                    return _heProjectRepository.CheckIfHeProjectWithGivenNameExists(frontDoorProjectName, orgId);
                }
                else
                {
                    return _heProjectRepository.CheckIfHeProjectWithGivenNameExists(frontDoorProjectName, Guid.Empty);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(organisationId) && Guid.TryParse(organisationId, out Guid orgId))
                {
                    return _frontDoorProjectRepository.CheckIfFrontDoorProjectWithGivenNameExists(frontDoorProjectName, orgId);
                }
                else
                {
                    return _frontDoorProjectRepository.CheckIfFrontDoorProjectWithGivenNameExists(frontDoorProjectName, Guid.Empty);
                }
            }
        }

        public bool DeactivateFrontDoorProject(string frontDoorProjectId, bool useHeTables)
        {
            if (useHeTables)
            {
                // Update a he_RecordStaus on Project Record
                var frontDoorProject = _heProjectRepository.GetById(new Guid(frontDoorProjectId), new string[] { nameof(he_Pipeline.he_PipelineId).ToLower() });
                frontDoorProject.he_RecordStatus = new OptionSetValue((int)he_Pipeline_he_RecordStatus.Loancreated);
                this.TracingService.Trace("Update a he_RecordStaus on Project Record");
                _heProjectRepository.Update(frontDoorProject);
                this.TracingService.Trace("After update a he_RecordStaus on Project Record");
                var frontDoorProjectAfter = _heProjectRepository.GetById(new Guid(frontDoorProjectId), new string[] { nameof(he_Pipeline.he_RecordStatus).ToLower() });
                return frontDoorProjectAfter.he_RecordStatus.Value == (int)he_Pipeline_he_RecordStatus.Loancreated;
            }
            else
            {
                var frontDoorProject = _frontDoorProjectRepository.GetById(new Guid(frontDoorProjectId), new string[] { nameof(invln_FrontDoorProjectPOC.invln_FrontDoorProjectPOCId).ToLower() });
                _frontDoorProjectRepository.SetState(frontDoorProject, invln_FrontDoorProjectPOCState.Inactive, invln_FrontDoorProjectPOC_StatusCode.Inactive);
                var frontDoorProjectAfter = _frontDoorProjectRepository.GetById(new Guid(frontDoorProjectId), new string[] { nameof(invln_FrontDoorProjectPOC.StateCode).ToLower() });
                return frontDoorProjectAfter.StateCode.Value == (int)invln_FrontDoorProjectPOCState.Inactive;
            }
        }

        private string GenerateFetchXmlAttributes(string fieldsToRetrieve)
        {
            var fields = fieldsToRetrieve.Split(',');
            var generatedAttribuesFetchXml = "";
            if (fields.Length > 0)
            {
                foreach (var field in fields)
                {
                    generatedAttribuesFetchXml += $"<attribute name=\"{field}\" />";
                }
            }
            return generatedAttribuesFetchXml;
        }

        private string GetFetchXmlConditionForGivenField(string fieldValue, string fieldName)
        {
            if (!string.IsNullOrEmpty(fieldValue))
            {
                return $"<condition attribute=\"{fieldName}\" operator=\"eq\" value=\"{fieldValue}\" />";
            }
            return string.Empty;
        }

        private string GenerateFilterMarksForCondition(string condition)
        {
            if (!string.IsNullOrEmpty(condition))
            {
                return $"<filter>{condition}</filter>";
            }
            return string.Empty;
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        private static ApiHttpClient CreateHttpClient(
            ISecretVariableRepository secretVariableRepository,
            IAzureAdTokenProviderFactory tokenProviderFactory)
        {
            var secrets = secretVariableRepository.GetMultiple(
                EnvironmentVariables.FrontDoorApiBaseUrl,
                EnvironmentVariables.AzureAd.TenantId,
                EnvironmentVariables.AzureAd.ClientId,
                EnvironmentVariables.AzureAd.ClientSecret,
                EnvironmentVariables.AzureAd.Scope
            );

            var azureAdAuthConfig = new AzureAdAuthConfig
            {
                TenantId = secrets.First(x => x.invln_Name == EnvironmentVariables.AzureAd.TenantId).invln_Value,
                ClientId = secrets.First(x => x.invln_Name == EnvironmentVariables.AzureAd.ClientId).invln_Value,
                ClientSecret = secrets.First(x => x.invln_Name == EnvironmentVariables.AzureAd.ClientSecret).invln_Value,
                Scope = secrets.First(x => x.invln_Name == EnvironmentVariables.AzureAd.Scope).invln_Value,
            };

            var tokenProvider = tokenProviderFactory.Create(azureAdAuthConfig);
            var frontDoorApiUrl = secrets.First(x => x.invln_Name == EnvironmentVariables.FrontDoorApiBaseUrl).invln_Value;
            var httpClient = new HttpClient(new BearerTokenAuthorizationHandler(tokenProvider))
            {
                BaseAddress = new Uri(frontDoorApiUrl)
            };

            return new ApiHttpClient(httpClient);
        }
    }
}
