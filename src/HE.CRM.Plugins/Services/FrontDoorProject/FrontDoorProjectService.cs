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
        private readonly IFrontDoorProjectSiteRepository _frontDoorProjectSiteRepository;
        private readonly IContactRepository _contactRepository;
        private readonly IWebRoleRepository _webroleRepository;
        #endregion

        #region Constructors
        public FrontDoorProjectService(CrmServiceArgs args) : base(args)
        {

            _frontDoorProjectRepository = CrmRepositoriesFactory.Get<IFrontDoorProjectRepository>();
            _frontDoorProjectSiteRepository = CrmRepositoriesFactory.Get<IFrontDoorProjectSiteRepository>();
            _contactRepository = CrmRepositoriesFactory.Get<IContactRepository>();
            _webroleRepository = CrmRepositoriesFactory.Get<IWebRoleRepository>();
        }
        #endregion


        public string CreateRecordFromPortal(string externalContactId, string organisationId, string frontDoorProjectId, string entityFieldsParameters)
        {
            Guid frontdoorprojectGUID = Guid.NewGuid();

            this.TracingService.Trace("entityFieldsParameters:" + entityFieldsParameters);
            FrontDoorProjectDto frontDoorProjectFromPortal = JsonSerializer.Deserialize<FrontDoorProjectDto>(entityFieldsParameters);

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

        public List<FrontDoorProjectDto> GetFrontDoorProjects(string organisationId, string externalContactId = null, string fieldsToRetrieve = null, string frontDoorProjectId = null)
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

            var frontDoorProjects = _frontDoorProjectRepository.GetFrontDoorProjectForOrganisationAndContact(organisationCondition, contactExternalIdFilter, attributes, frontDoorProjectCondition);
            if (frontDoorProjects.Any())
            {
                foreach (var frontDoorProject in frontDoorProjects)
                {
                    var contact = _contactRepository.GetById(frontDoorProject.invln_ContactId.Id, new string[] { nameof(Contact.FirstName).ToLower(), nameof(Contact.LastName).ToLower(), nameof(Contact.invln_externalid).ToLower(), nameof(Contact.EMailAddress1).ToLower(), nameof(Contact.Telephone1).ToLower() });
                    var frontDoorProjecDto = FrontDoorProjectMapper.MapRegularEntityToDto(frontDoorProject, contact);
                    listOfFrontDoorProjects.Add(frontDoorProjecDto);
                }
            }
            return listOfFrontDoorProjects;
        }
        public bool CheckIfFrontDoorProjectWithGivenNameExists(string frontDoorProjectName)
        {
            return _frontDoorProjectRepository.CheckIfFrontDoorProjectWithGivenNameExists(frontDoorProjectName);
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
