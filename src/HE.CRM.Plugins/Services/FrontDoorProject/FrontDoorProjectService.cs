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

        public List<FrontDoorProjectDto> GetFrontDoorProjectsForAccountAndContact(string externalContactId, string accountId, string frontDoorProjectId = null, string fieldsToRetrieve = null)
        {
            List<FrontDoorProjectDto> entityCollection = new List<FrontDoorProjectDto>();
            if (Guid.TryParse(accountId, out Guid accountGuid))
            {
                var contact = _contactRepository.GetContactViaExternalId(externalContactId);
                var role = _webroleRepository.GetContactWebRole(contact.Id, ((int)invln_Portal1.Loans).ToString());
                List<invln_FrontDoorProjectPOC> frontDoorProjectsForAccountAndContact;
                if (role.Any(x => x.Contains("pl.invln_permission") && ((OptionSetValue)((AliasedValue)x["pl.invln_permission"]).Value).Value == (int)invln_Permission.Admin) && frontDoorProjectId == null)
                {
                    TracingService.Trace("admin");
                    frontDoorProjectsForAccountAndContact = _frontDoorProjectRepository.GetAccountFrontDoorProjects(accountGuid);
                }
                else
                {
                    TracingService.Trace("regular user, not admin");
                    string attributes = null;
                    if (!string.IsNullOrEmpty(fieldsToRetrieve))
                    {
                        attributes = GenerateFetchXmlAttributes(fieldsToRetrieve);
                    }
                    frontDoorProjectsForAccountAndContact = _frontDoorProjectRepository.GetFrontDoorProjectForOrganisationAndContact(accountGuid, externalContactId, frontDoorProjectId, attributes);
                }
                this.TracingService.Trace("GetFrontDoorProjectForOrganisationAndContact");
                this.TracingService.Trace($"{frontDoorProjectsForAccountAndContact.Count}");
                foreach (var element in frontDoorProjectsForAccountAndContact)
                {
                    List<FrontDoorProjectSiteDto> frontDoorProjectSiteDtoList = new List<FrontDoorProjectSiteDto>();
                    this.TracingService.Trace($"Front Door Project id {element.Id}");
                    this.TracingService.Trace("GetFrontDoorProjectSiteRelatedToFrontDoorProject");
                    var frontDoorProjectSiteList = _frontDoorProjectSiteRepository.GetSiteRelatedToFrontDoorProject(element.ToEntityReference());

                    foreach (var site in frontDoorProjectSiteList)
                    {
                        this.TracingService.Trace("MapFrontDoorProjectSiteToDto");
                        frontDoorProjectSiteDtoList.Add(FrontDoorProjectSiteMapper.MapFrontDoorProjectSiteToDto(site));
                    }

                    this.TracingService.Trace("MapFrontDoorProjectToDto");
                    Contact frontDoorProjectContact = null;
                    if (element.invln_ContactId != null)
                    {
                        frontDoorProjectContact = this._contactRepository.GetById(element.invln_ContactId.Id, new string[]
                        {
                            nameof(Contact.EMailAddress1).ToLower(),
                            nameof(Contact.FirstName).ToLower(),
                            nameof(Contact.LastName).ToLower(),
                            nameof(Contact.invln_externalid).ToLower(),
                            nameof(Contact.Telephone1).ToLower(),
                        });
                    }
                    this.TracingService.Trace("MapRegularEntityToDto");
                    entityCollection.Add(FrontDoorProjectMapper.MapRegularEntityToDto(element, frontDoorProjectSiteDtoList, externalContactId, frontDoorProjectContact));
                }
            }
            return entityCollection;
        }

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
                this.TracingService.Trace("Update invln_FrontDoorProjectPOC");
                frontdoorprojectGUID = projectId;
                frontDoorProjecToCreate.Id = projectId;
                _frontDoorProjectRepository.Update(frontDoorProjecToCreate);
            }
            else
            {
                if (frontDoorProjecToCreate.invln_ContactId == null)
                {
                    frontDoorProjecToCreate.invln_ContactId = requestContact.ToEntityReference();
                }

                this.TracingService.Trace("Create FrontDoorProject");
                frontdoorprojectGUID = _frontDoorProjectRepository.Create(frontDoorProjecToCreate);
            }

            // Site Creator
            if (frontDoorProjectFromPortal.FrontDoorSiteList != null && frontDoorProjectFromPortal.FrontDoorSiteList.Count > 0)
            {
                this.TracingService.Trace($"FrontDoorSiteList.Count {frontDoorProjectFromPortal.FrontDoorSiteList.Count}");
                foreach (var siteDto in frontDoorProjectFromPortal.FrontDoorSiteList)
                {
                    this.TracingService.Trace("loop begin");
                    var siteToCreate = FrontDoorProjectSiteMapper.MapFrontDoorProjectSiteDtoToRegularEntity(siteDto, frontdoorprojectGUID.ToString());
                    this.TracingService.Trace("create");
                    if (!String.IsNullOrEmpty(siteDto.SiteId) && Guid.TryParse(siteDto.SiteId, out Guid result))
                    {
                        _frontDoorProjectSiteRepository.Update(siteToCreate);
                    }
                    else
                    {
                        _frontDoorProjectSiteRepository.Create(siteToCreate);
                    }
                    this.TracingService.Trace("after create record");
                }
            }
            return frontdoorprojectGUID.ToString();
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
    }
}
