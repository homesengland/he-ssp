using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.DtoMapping;
using HE.CRM.Common.Repositories.interfaces;
using HE.CRM.Common.Repositories.Interfaces;
using HE.CRM.Model.CrmSerializedParameters;
using HE.CRM.Plugins.Services.GovNotifyEmail;
using HE.CRM.Plugins.Services.LoanApplication;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Xml.Linq;

namespace HE.CRM.Plugins.Services.FrontDoorProject
{
    public class FrontDoorProjectService : CrmService, IFrontDoorProjectService
    {

        #region Fields
        private readonly IFrontDoorProjectRepository _frontDoorProjectRepository;
        private readonly IContactRepository _contactRepository;
        private readonly ILocalAuthorityRepository _localAuthorityRepository;
        #endregion

        #region Constructors
        public FrontDoorProjectService(CrmServiceArgs args) : base(args)
        {
            _frontDoorProjectRepository = CrmRepositoriesFactory.Get<IFrontDoorProjectRepository>();
            _contactRepository = CrmRepositoriesFactory.Get<IContactRepository>();
            _localAuthorityRepository = CrmRepositoriesFactory.Get<ILocalAuthorityRepository>();
        }
        #endregion


        public string CreateRecordFromPortal(string externalContactId, string organisationId, string frontDoorProjectId, string entityFieldsParameters)
        {
            Guid frontdoorprojectGUID = Guid.NewGuid();

            this.TracingService.Trace("entityFieldsParameters:" + entityFieldsParameters);
            FrontDoorProjectDto frontDoorProjectFromPortal = JsonSerializer.Deserialize<FrontDoorProjectDto>(entityFieldsParameters);
            if (frontDoorProjectFromPortal.LocalAuthorityCode != null)
            {
                frontDoorProjectFromPortal.LocalAuthority = _localAuthorityRepository.GetLocalAuthorityWithGivenOnsCode(frontDoorProjectFromPortal.LocalAuthorityCode)?.Id.ToString();
            }

            //THIS IS CONTACT WHO IS SENDING MESSAGE 
            var requestContact = _contactRepository.GetContactViaExternalId(externalContactId);
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
            return frontdoorprojectGUID.ToString();
        }

        public List<FrontDoorProjectDto> GetFrontDoorProjects(string organisationId, string externalContactId = null, string fieldsToRetrieve = null, string frontDoorProjectId = null, string includeInactive = null)
        {
            this.TracingService.Trace("GetFrontDoorProjects");
            var listOfFrontDoorProjects = new List<FrontDoorProjectDto>();
            var organisationCondition = GetFetchXmlConditionForGivenField(organisationId, nameof(invln_FrontDoorProjectPOC.invln_AccountId).ToLower());
            var contactExternalIdFilter = GetFetchXmlConditionForGivenField(externalContactId, nameof(Contact.invln_externalid).ToLower());
            contactExternalIdFilter = GenerateFilterMarksForCondition(contactExternalIdFilter);
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
            return listOfFrontDoorProjects;
        }

        public bool CheckIfFrontDoorProjectWithGivenNameExists(string frontDoorProjectName, string organisationId)
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

        public bool DeactivateFrontDoorProject(string frontDoorProjectId)
        {
            var frontDoorProject = _frontDoorProjectRepository.GetById(new Guid(frontDoorProjectId), new string[] { nameof(invln_FrontDoorProjectPOC.invln_FrontDoorProjectPOCId).ToLower() });
            _frontDoorProjectRepository.SetState(frontDoorProject, invln_FrontDoorProjectPOCState.Inactive, invln_FrontDoorProjectPOC_StatusCode.Inactive);
            var frontDoorProjectAfter = _frontDoorProjectRepository.GetById(new Guid(frontDoorProjectId), new string[] { nameof(invln_FrontDoorProjectPOC.StateCode).ToLower() });
            return frontDoorProjectAfter.StateCode.Value == (int)invln_FrontDoorProjectPOCState.Inactive;
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
    }
}
