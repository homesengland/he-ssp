using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
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
        public ApplicationService(CrmServiceArgs args) : base(args)
        {
            _applicationRepository = CrmRepositoriesFactory.Get<IAhpApplicationRepository>();
            _contactRepository = CrmRepositoriesFactory.Get<IContactRepository>();
            _sharepointDocumentLocationRepository = CrmRepositoriesFactory.Get<ISharepointDocumentLocationRepository>();
            _sharepointSiteRepository = CrmRepositoriesFactory.Get<ISharepointSiteRepository>();
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

        public bool CheckIfApplicationExists(string serializedApplication)
        {
            var applicationDto = JsonSerializer.Deserialize<AhpApplicationDto>(serializedApplication);
            return _applicationRepository.ApplicationWithGivenNameExists(applicationDto.name);
        }

        public void CheckIfApplicationWithNewNameExists(invln_scheme target, invln_scheme preImage)
        {
            if ((preImage == null || (preImage != null && preImage.invln_schemename != target.invln_schemename)) && _applicationRepository.ApplicationWithGivenNameExists(target.invln_schemename))
            {
                throw new InvalidPluginExecutionException("Application with new name already exists.");
            }
        }

        public void CreateDocumentLocation(invln_scheme target)
        {
            var documentLocation = _sharepointDocumentLocationRepository.GetByAttribute(nameof(SharePointDocumentLocation.Name).ToLower(), "AHP Application Documents").FirstOrDefault();
            if (documentLocation != null)
            {
                foreach(var attr in documentLocation.Attributes)
                {
                    TracingService.Trace($"attr: {attr.Key}, {attr.Value}");
                }
                TracingService.Trace("afer");
                var mainDocumentLocation = _sharepointSiteRepository.GetById(documentLocation.ParentSiteOrLocation.Id, new string[] { nameof(SharePointSite.AbsoluteURL).ToLower() });
                if(mainDocumentLocation != null)
                {
                    var relativeUrl = $"{target.invln_applicationid}";
                    var absoluteUrl = $"{mainDocumentLocation.AbsoluteURL}/{relativeUrl}";
                    var ahpApplicaitonDocumentToCreate = new SharePointDocumentLocation()
                    {
                        RegardingObjectId = target.ToEntityReference(),
                        Name = $"Documents on AHP Application",
                        RelativeUrl = relativeUrl,
                        ParentSiteOrLocation = documentLocation.ToEntityReference(),
                        AbsoluteURL = absoluteUrl,
                    };
                    ahpApplicaitonDocumentToCreate.Id = _sharepointDocumentLocationRepository.Create(ahpApplicaitonDocumentToCreate);
                    var homeTypesRelativeUrl = "Home Types";
                    var homeTypesFolderToCreate = new SharePointDocumentLocation()
                    {
                        Name = "Home Types",
                        ParentSiteOrLocation = ahpApplicaitonDocumentToCreate.ToEntityReference(),
                        RelativeUrl = homeTypesRelativeUrl,
                        AbsoluteURL = $"{absoluteUrl}/{homeTypesRelativeUrl}",
                    };
                    _ = _sharepointDocumentLocationRepository.Create(homeTypesFolderToCreate);
                }
                else
                {
                    throw new InvalidPluginExecutionException("Sharepoint Site does not exists");
                }
            }
            else
            {
                throw new InvalidPluginExecutionException("Document Location record for AHP Application does not exists");
            }
        }
        public string GetFileLocationForAhpApplication(string ahpApplicationId)
        {
            if (Guid.TryParse(ahpApplicationId, out Guid applicationGuid))
            {
                var relatedDocumentLocation = _sharepointDocumentLocationRepository.GetDocumentLocationRelatedToRecordWithGivenGuid(applicationGuid);
                if (relatedDocumentLocation != null)
                {
                    return relatedDocumentLocation.AbsoluteURL;
                }
            }
            return string.Empty;
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
                    var applicationDto = AhpApplicationMapper.MapRegularEntityToDto(application);
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
