using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.AHP.Plugins.Services.GovNotifyEmail;
using HE.CRM.Common.DtoMapping;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Services.Application
{
    public class ApplicationService : CrmService, IApplicationService
    {
        private readonly IAhpApplicationRepository _applicationRepository;
        private readonly IContactRepository _contactRepository;
        private readonly ISharepointDocumentLocationRepository _sharepointDocumentLocationRepository;
        private readonly ISharepointSiteRepository _sharepointSiteRepository;
        private readonly IAhpApplicationRepository _ahpApplicationRepositoryAdmin;

        private readonly IGovNotifyEmailService _govNotifyEmailService;
        public ApplicationService(CrmServiceArgs args) : base(args)
        {
            _applicationRepository = CrmRepositoriesFactory.Get<IAhpApplicationRepository>();
            _contactRepository = CrmRepositoriesFactory.Get<IContactRepository>();
            _sharepointDocumentLocationRepository = CrmRepositoriesFactory.Get<ISharepointDocumentLocationRepository>();
            _sharepointSiteRepository = CrmRepositoriesFactory.Get<ISharepointSiteRepository>();
            _ahpApplicationRepositoryAdmin = CrmRepositoriesFactory.GetSystem<IAhpApplicationRepository>();

            _govNotifyEmailService = CrmServicesFactory.Get<IGovNotifyEmailService>();
        }

        public void ChangeApplicationStatus(string organisationId, string contactId, string applicationId, int newStatus)
        {
            var additionalFilters = $"<condition attribute=\"invln_schemeid\" operator=\"eq\" value=\"{applicationId}\" />";
            var applications = _applicationRepository.GetApplicationsForOrganisationAndContact(organisationId, contactId, null, additionalFilters);
            if (applications.Any())
            {
                var application = applications.First();
                var applicationToUpdate = new invln_scheme()
                {
                    Id = application.Id,
                    StatusCode = new OptionSetValue(newStatus),
                };
                _applicationRepository.Update(applicationToUpdate);
            }
        }

        public bool CheckIfApplicationExists(string serializedApplication, Guid organisationId)
        {
            var applicationDto = JsonSerializer.Deserialize<AhpApplicationDto>(serializedApplication);
            return _applicationRepository.ApplicationWithGivenNameAndOrganisationExists(applicationDto.name, organisationId);
        }

        public void CheckIfApplicationWithNewNameExists(invln_scheme target, invln_scheme preImage)
        {
            var organisationId = target.invln_organisationid == null ? preImage.invln_organisationid.Id : target.invln_organisationid.Id;
            if ((preImage == null || (preImage != null && preImage.invln_schemename != target.invln_schemename)) &&
                _applicationRepository.ApplicationWithGivenNameAndOrganisationExists(target.invln_schemename, organisationId))
            {
                throw new InvalidPluginExecutionException("Application with new name already exists.");
            }
        }

        public void CreateDocumentLocation(invln_scheme target)
        {
            var documentLocation = _sharepointDocumentLocationRepository.GetByAttribute(nameof(SharePointDocumentLocation.Name).ToLower(), "AHP Application Documents").FirstOrDefault();
            if (documentLocation != null)
            {
                var ahpApplicaitonDocumentToCreate = new SharePointDocumentLocation()
                {
                    RegardingObjectId = target.ToEntityReference(),
                    Name = $"Documents on AHP Application",
                    RelativeUrl = $"{target.invln_applicationid}",
                    ParentSiteOrLocation = documentLocation.ToEntityReference(),
                };
                ahpApplicaitonDocumentToCreate.Id = _sharepointDocumentLocationRepository.Create(ahpApplicaitonDocumentToCreate);
                var homeTypesRelativeUrl = "Home Types";
                var homeTypesFolderToCreate = new SharePointDocumentLocation()
                {
                    Name = homeTypesRelativeUrl,
                    ParentSiteOrLocation = ahpApplicaitonDocumentToCreate.ToEntityReference(),
                    RelativeUrl = homeTypesRelativeUrl,
                };
                _ = _sharepointDocumentLocationRepository.Create(homeTypesFolderToCreate);
            }
            else
            {
                throw new InvalidPluginExecutionException("Document Location record for AHP Application does not exists");
            }
        }
        public string GetFileLocationForAhpApplication(string ahpApplicationId, bool isAbsolute)
        {
            var urlToReturn = string.Empty;
            if (Guid.TryParse(ahpApplicationId, out Guid applicationGuid))
            {
                var relatedDocumentLocation = _sharepointDocumentLocationRepository.GetDocumentLocationRelatedToRecordWithGivenGuid(applicationGuid);
                if (relatedDocumentLocation != null && relatedDocumentLocation.ParentSiteOrLocation != null)
                {
                    TracingService.Trace("related document");
                    urlToReturn = relatedDocumentLocation.RelativeUrl;
                    if (isAbsolute)
                    {
                        var parentDocumentLocation = _sharepointDocumentLocationRepository.GetById(relatedDocumentLocation.ParentSiteOrLocation.Id,
                            new string[] { nameof(SharePointDocumentLocation.RelativeUrl).ToLower(), nameof(SharePointDocumentLocation.ParentSiteOrLocation).ToLower() });
                        if (parentDocumentLocation != null && parentDocumentLocation.ParentSiteOrLocation != null)
                        {
                            TracingService.Trace("parent of related document");
                            urlToReturn = urlToReturn.Insert(0, $"{parentDocumentLocation.RelativeUrl}/");
                            var mainDocumentLocation = _sharepointSiteRepository.GetById(parentDocumentLocation.ParentSiteOrLocation.Id,
                            new string[] { nameof(SharePointSite.AbsoluteURL).ToLower() });
                            if (mainDocumentLocation != null)
                            {
                                TracingService.Trace("main document");
                                urlToReturn = urlToReturn.Insert(0, $"{mainDocumentLocation.AbsoluteURL}/");
                            }
                        }
                    }
                }
            }
            return urlToReturn;
        }

        public List<AhpApplicationDto> GetApplication(string organisationId, string contactId, string fieldsToRetrieve = null, string applicationId = null)
        {
            var listOfApplications = new List<AhpApplicationDto>();
            var additionalFilters = string.Empty;
            if (!string.IsNullOrEmpty(applicationId))
            {
                additionalFilters = $"<condition attribute=\"invln_schemeid\" operator=\"eq\" value=\"{applicationId}\" />";
            }
            string attributes = null;
            if (!string.IsNullOrEmpty(fieldsToRetrieve))
            {
                attributes = GenerateFetchXmlAttributes(fieldsToRetrieve);
            }
            var applications = _applicationRepository.GetApplicationsForOrganisationAndContact(organisationId, contactId, attributes, additionalFilters);
            if (applications.Any())
            {
                var contact = _contactRepository.GetContactViaExternalId(contactId, new string[] { nameof(Contact.FirstName).ToLower(), nameof(Contact.LastName).ToLower() });
                foreach (var application in applications)
                {
                    var applicationDto = AhpApplicationMapper.MapRegularEntityToDto(application, contactId);
                    if (application.invln_lastexternalmodificationby != null)
                    {
                        applicationDto.lastExternalModificationBy = new ContactDto()
                        {
                            firstName = contact.FirstName,
                            lastName = contact.LastName,
                        };
                    }
                    listOfApplications.Add(applicationDto);
                }
            }
            return listOfApplications;
        }

        public Guid SetApplication(string applicationSerialized, string organisationId, string contactId, string fieldsToUpdate = null)
        {
            var application = JsonSerializer.Deserialize<AhpApplicationDto>(applicationSerialized);
            var contact = _contactRepository.GetContactViaExternalId(contactId);
            var applicationMapped = AhpApplicationMapper.MapDtoToRegularEntity(application, contact.Id.ToString(), organisationId);
            if (string.IsNullOrEmpty(application.id))
            {
                applicationMapped.invln_lastexternalmodificationon = DateTime.UtcNow;
                applicationMapped.invln_lastexternalmodificationby = contact.ToEntityReference();
                return _applicationRepository.Create(applicationMapped);
            }
            else
            {
                invln_scheme applicationToUpdateOrCreate;
                if (!string.IsNullOrEmpty(fieldsToUpdate))
                {
                    var fields = fieldsToUpdate?.Split(',');
                    applicationToUpdateOrCreate = new invln_scheme();
                    foreach (var field in fields)
                    {
                        TracingService.Trace($"field name {field}");
                        if (applicationMapped.Contains(field))
                        {
                            TracingService.Trace($"contains");
                            applicationToUpdateOrCreate[field] = applicationMapped[field];
                        }
                    }
                }
                else
                {
                    applicationToUpdateOrCreate = applicationMapped;
                }
                applicationToUpdateOrCreate.Id = new Guid(application.id);
                applicationToUpdateOrCreate.invln_lastexternalmodificationon = DateTime.UtcNow;
                applicationToUpdateOrCreate.invln_lastexternalmodificationby = contact.ToEntityReference();
                _applicationRepository.Update(applicationToUpdateOrCreate);
                return applicationToUpdateOrCreate.Id;
            }
        }

        public void SendReminderEmailForRefferedBackToApplicant(Guid applicationId)
        {
            var application = _ahpApplicationRepositoryAdmin.GetById(applicationId, new string[] { nameof(invln_scheme.invln_contactid).ToLower(), nameof(invln_scheme.invln_lastexternalmodificationby).ToLower() });
            if (application != null)
            {
                if (application.invln_contactid != null)
                {
                    _govNotifyEmailService.SendNotifications_AHP_EXTERNAL_REMINDER_TO_FINALIZE_APPLICATION_REFERRED_BACK(application.ToEntityReference(), application.invln_contactid);
                }
                if (application.invln_lastexternalmodificationby != null &&
                    (application.invln_contactid == null || (application.invln_contactid != null && application.invln_lastexternalmodificationby.Id != application.invln_contactid.Id)))
                {
                    _govNotifyEmailService.SendNotifications_AHP_EXTERNAL_REMINDER_TO_FINALIZE_APPLICATION_REFERRED_BACK(application.ToEntityReference(), application.invln_lastexternalmodificationby);
                }
            }
        }

        public void SendReminderEmailForFinaliseDraftApplication(Guid applicationId)
        {
            var application = _ahpApplicationRepositoryAdmin.GetById(applicationId, new string[] { nameof(invln_scheme.invln_contactid).ToLower() });
            if (application != null && application.invln_contactid != null)
            {
                _govNotifyEmailService.SendNotifications_AHP_EXTERNAL_REMINDER_TO_FINALIZE_DRAFT_APPLICATION(application.ToEntityReference(), application.invln_contactid);
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
    }
}
