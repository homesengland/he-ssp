using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
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

        private readonly IAhpStatusChangeRepository _ahpStatusChangeRepository;

        private readonly IGovNotifyEmailService _govNotifyEmailService;

        public ApplicationService(CrmServiceArgs args) : base(args)
        {
            _applicationRepository = CrmRepositoriesFactory.Get<IAhpApplicationRepository>();
            _contactRepository = CrmRepositoriesFactory.Get<IContactRepository>();
            _sharepointDocumentLocationRepository = CrmRepositoriesFactory.Get<ISharepointDocumentLocationRepository>();
            _sharepointSiteRepository = CrmRepositoriesFactory.Get<ISharepointSiteRepository>();
            _ahpApplicationRepositoryAdmin = CrmRepositoriesFactory.GetSystem<IAhpApplicationRepository>();
            _ahpStatusChangeRepository = CrmRepositoriesFactory.Get<IAhpStatusChangeRepository>();
            _govNotifyEmailService = CrmServicesFactory.Get<IGovNotifyEmailService>();
        }

        public void ChangeApplicationStatus(string organisationId, string contactId, string applicationId, int newStatus, string changeReason, bool representationsandwarranties)
        {
            TracingService.Trace($"Service ChangeApplicationStatus");
            var contact = _contactRepository.GetContactViaExternalId(contactId);
            var application = _applicationRepository.GetById(new Guid(applicationId),
                new string[] {
                    nameof(invln_scheme.invln_schemeId).ToLower(),
                    nameof(invln_scheme.invln_schemename).ToLower(),
                    nameof(invln_scheme.invln_ExternalStatus).ToLower(),
                    nameof(invln_scheme.invln_PreviousInternalStatus).ToLower(),
                    nameof(invln_scheme.invln_PreviousExternalStatus).ToLower(),
                    nameof(invln_scheme.StatusCode).ToLower(),
                    nameof(invln_scheme.invln_organisationid).ToLower(),
                    nameof(invln_scheme.invln_contactid).ToLower()
                });

            if (application != null && application.invln_organisationid.Id == new Guid(organisationId) && application.invln_contactid.Id == contact.Id)
            {
                var ahpWithNewStatusCodesAndOtherChanges = new invln_scheme();
                switch (application.invln_ExternalStatus.Value)
                {
                    case (int)invln_ExternalStatusAHP.ReferredBackToApplicant:
                        if (newStatus == (int)invln_ExternalStatusAHP.UnderReview)
                        {
                            ahpWithNewStatusCodesAndOtherChanges.StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.UnderReviewInAssessment);
                            ahpWithNewStatusCodesAndOtherChanges.StateCode = new OptionSetValue((int)invln_schemeState.Active);
                        }
                        else
                        {
                            ahpWithNewStatusCodesAndOtherChanges = MapAhpExternalStatusToInternalAndSetOtherValues(new OptionSetValue(newStatus));
                        }
                        break;

                    case (int)invln_ExternalStatusAHP.OnHold:
                        if (newStatus == application.invln_PreviousExternalStatus.Value)
                        {
                            ahpWithNewStatusCodesAndOtherChanges.StatusCode = new OptionSetValue(application.invln_PreviousInternalStatus.Value);
                            ahpWithNewStatusCodesAndOtherChanges.StateCode = new OptionSetValue(MapAhpStatusCodeToStateCode(application.invln_PreviousInternalStatus.Value));
                        }
                        else
                        {
                            ahpWithNewStatusCodesAndOtherChanges = MapAhpExternalStatusToInternalAndSetOtherValues(new OptionSetValue(newStatus));
                        }
                        break;

                    default:
                        ahpWithNewStatusCodesAndOtherChanges = MapAhpExternalStatusToInternalAndSetOtherValues(new OptionSetValue(newStatus));
                        break;
                }

                var applicationToUpdate = new invln_scheme()
                {
                    Id = application.Id,
                    StatusCode = ahpWithNewStatusCodesAndOtherChanges.StatusCode,
                    StateCode = ahpWithNewStatusCodesAndOtherChanges.StateCode,
                    invln_ExternalStatus = new OptionSetValue(newStatus),
                    invln_PreviousInternalStatus = new OptionSetValue(application.StatusCode.Value),
                };

                if (representationsandwarranties)
                {
                    applicationToUpdate.invln_representationsandwarrantiesconfirmation = representationsandwarranties;
                }

                if (application.invln_PreviousExternalStatus == null)
                {
                    applicationToUpdate.invln_PreviousExternalStatus = new OptionSetValue(application.invln_ExternalStatus.Value);
                }
                else if (application.invln_PreviousExternalStatus.Value != newStatus)
                {
                    applicationToUpdate.invln_PreviousExternalStatus = new OptionSetValue(application.invln_ExternalStatus.Value);
                }
                else
                {
                    applicationToUpdate.invln_PreviousExternalStatus = application.invln_ExternalStatus;
                }

                if (ahpWithNewStatusCodesAndOtherChanges.invln_DateSubmitted != null)
                {
                    applicationToUpdate.invln_DateSubmitted = ahpWithNewStatusCodesAndOtherChanges.invln_DateSubmitted;
                    applicationToUpdate.invln_submitedby = contact.ToEntityReference();
                }

                var ahpStatusChangeToCreate = new invln_AHPStatusChange()
                {
                    invln_Changefrom = application.StatusCode,
                    invln_ChangeSource = new OptionSetValue((int)invln_ChangesourceSet.External),
                    invln_Changeto = ahpWithNewStatusCodesAndOtherChanges.StatusCode,
                    invln_AHPApplication = application.ToEntityReference(),
                    invln_Comment = changeReason,
                    invln_changedby = contact?.ToEntityReference()
                };

                _applicationRepository.Update(applicationToUpdate);
                _ahpStatusChangeRepository.Create(ahpStatusChangeToCreate);
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

        public List<AhpApplicationDto> GetApplication(string organisationId, string contactId = null, string fieldsToRetrieve = null, string applicationId = null)
        {
            var listOfApplications = new List<AhpApplicationDto>();
            var additionalFilters = GetFetchXmlConditionForGivenField(applicationId, nameof(invln_scheme.invln_schemeId).ToLower());

            var contactExternalIdFilter = GetFetchXmlConditionForGivenField(contactId, nameof(Contact.invln_externalid).ToLower());
            contactExternalIdFilter = GenerateFilterMarksForCondition(contactExternalIdFilter);
            string attributes = null;
            if (!string.IsNullOrEmpty(fieldsToRetrieve))
            {
                attributes = GenerateFetchXmlAttributes(fieldsToRetrieve);
            }
            var applications = _applicationRepository.GetApplicationsForOrganisationAndContact(organisationId, contactExternalIdFilter, attributes, additionalFilters);
            if (applications.Any())
            {
                foreach (var application in applications)
                {
                    var contact = _contactRepository.GetById(application.invln_contactid.Id, new string[] { Contact.Fields.FirstName, Contact.Fields.LastName, nameof(Contact.invln_externalid).ToLower() });

                    var applicationDto = AhpApplicationMapper.MapRegularEntityToDto(application, contact.invln_externalid);
                    if (application.invln_lastexternalmodificationby != null)
                    {
                        var lastExternalModificationBy = _contactRepository.GetById(application.invln_lastexternalmodificationby.Id,
                        new string[] { nameof(Contact.FirstName).ToLower(), nameof(Contact.LastName).ToLower() });
                        applicationDto.lastExternalModificationBy = new ContactDto()
                        {
                            firstName = lastExternalModificationBy.FirstName,
                            lastName = lastExternalModificationBy.LastName,
                        };
                    }

                    if (application.invln_submitedby != null)
                    {
                        var submitedBy = _contactRepository.GetById(application.invln_submitedby.Id, new string[] { Contact.Fields.FirstName, Contact.Fields.LastName });
                        applicationDto.lastExternalSubmittedBy = new ContactDto()
                        {
                            firstName = submitedBy.FirstName,
                            lastName = submitedBy.LastName,
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

        public void ChangeExternalStatus(invln_scheme target, invln_scheme preImage)
        {
            if (target.StatusCode != null && preImage.StatusCode != null && target.StatusCode.Value != preImage.StatusCode.Value)
            {
                target.invln_ExternalStatus = MapAhpInternalToExternalStatus(target.StatusCode.Value);
            }
        }

        private invln_scheme MapAhpExternalStatusToInternalAndSetOtherValues(OptionSetValue externalStatus)
        {
            var ahpApplication = new invln_scheme();
            switch (externalStatus.Value)
            {
                case (int)invln_ExternalStatusAHP.Draft:
                    ahpApplication.StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft);
                    ahpApplication.StateCode = new OptionSetValue((int)invln_schemeState.Active);
                    break;

                case (int)invln_ExternalStatusAHP.ApplicationSubmitted:
                    ahpApplication.StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.ApplicationSubmitted);
                    ahpApplication.StateCode = new OptionSetValue((int)invln_schemeState.Active);
                    ahpApplication.invln_DateSubmitted = DateTime.Now;
                    break;

                case (int)invln_ExternalStatusAHP.Approved:
                    ahpApplication.StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Approved);
                    ahpApplication.StateCode = new OptionSetValue((int)invln_schemeState.Active);
                    break;

                case (int)invln_ExternalStatusAHP.ApprovedSubjectToContract:
                    ahpApplication.StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.ApprovedSubjecttoContract);
                    ahpApplication.StateCode = new OptionSetValue((int)invln_schemeState.Active);
                    break;

                case (int)invln_ExternalStatusAHP.Deleted:
                    ahpApplication.StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Deleted);
                    ahpApplication.StateCode = new OptionSetValue((int)invln_schemeState.Inactive);
                    break;

                case (int)invln_ExternalStatusAHP.OnHold:
                    ahpApplication.StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.OnHold);
                    ahpApplication.StateCode = new OptionSetValue((int)invln_schemeState.Active);
                    break;

                case (int)invln_ExternalStatusAHP.ReferredBackToApplicant:
                    ahpApplication.StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.ReferredBackToApplicant);
                    ahpApplication.StateCode = new OptionSetValue((int)invln_schemeState.Active);
                    break;

                case (int)invln_ExternalStatusAHP.Rejected:
                    ahpApplication.StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Rejected);
                    ahpApplication.StateCode = new OptionSetValue((int)invln_schemeState.Active);
                    break;

                case (int)invln_ExternalStatusAHP.RequestedEditing:
                    ahpApplication.StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.RequestedEditing);
                    ahpApplication.StateCode = new OptionSetValue((int)invln_schemeState.Active);
                    break;

                case (int)invln_ExternalStatusAHP.UnderReview:
                    ahpApplication.StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.UnderReviewPendingAssessment);
                    ahpApplication.StateCode = new OptionSetValue((int)invln_schemeState.Active);
                    break;

                case (int)invln_ExternalStatusAHP.Withdrawn:
                    ahpApplication.StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Withdrawn);
                    ahpApplication.StateCode = new OptionSetValue((int)invln_schemeState.Inactive);
                    break;

                default:
                    break;
            }
            return ahpApplication;
        }

        private OptionSetValue MapAhpInternalToExternalStatus(int internalStatus)
        {
            switch (internalStatus)
            {
                case (int)invln_scheme_StatusCode.Draft:
                    return new OptionSetValue((int)invln_ExternalStatusAHP.Draft);

                case (int)invln_scheme_StatusCode.ApplicationSubmitted:
                    return new OptionSetValue((int)invln_ExternalStatusAHP.ApplicationSubmitted);

                case (int)invln_scheme_StatusCode.Deleted:
                    return new OptionSetValue((int)invln_ExternalStatusAHP.Deleted);

                case (int)invln_scheme_StatusCode.OnHold:
                    return new OptionSetValue((int)invln_ExternalStatusAHP.OnHold);

                case (int)invln_scheme_StatusCode.Withdrawn:
                    return new OptionSetValue((int)invln_ExternalStatusAHP.Withdrawn);

                case (int)invln_scheme_StatusCode.UnderReviewPendingAssessment:
                    return new OptionSetValue((int)invln_ExternalStatusAHP.UnderReview);

                case (int)invln_scheme_StatusCode.UnderReviewInAssessment:
                    return new OptionSetValue((int)invln_ExternalStatusAHP.UnderReview);

                case (int)invln_scheme_StatusCode.UnderReviewGoingToSLT:
                    return new OptionSetValue((int)invln_ExternalStatusAHP.UnderReview);

                case (int)invln_scheme_StatusCode.UnderReviewInternallyApproved:
                    return new OptionSetValue((int)invln_ExternalStatusAHP.UnderReview);

                case (int)invln_scheme_StatusCode.InternallyApprovedSubjectToIPQ:
                    return new OptionSetValue((int)invln_ExternalStatusAHP.UnderReview);

                case (int)invln_scheme_StatusCode.InternallyApprovedSubjectToRegulatorSignOff:
                    return new OptionSetValue((int)invln_ExternalStatusAHP.UnderReview);

                case (int)invln_scheme_StatusCode.InternallyApprovedSubjectToIPQAndRegulatorySignOff:
                    return new OptionSetValue((int)invln_ExternalStatusAHP.UnderReview);

                case (int)invln_scheme_StatusCode.InternallyRejected:
                    return new OptionSetValue((int)invln_ExternalStatusAHP.UnderReview);

                case (int)invln_scheme_StatusCode.Rejected:
                    return new OptionSetValue((int)invln_ExternalStatusAHP.Rejected);

                case (int)invln_scheme_StatusCode.RequestedEditing:
                    return new OptionSetValue((int)invln_ExternalStatusAHP.RequestedEditing);

                case (int)invln_scheme_StatusCode.ReferredBackToApplicant:
                    return new OptionSetValue((int)invln_ExternalStatusAHP.ReferredBackToApplicant);

                case (int)invln_scheme_StatusCode.ApprovedSubjecttoContract:
                    return new OptionSetValue((int)invln_ExternalStatusAHP.ApprovedSubjectToContract);

                case (int)invln_scheme_StatusCode.ApprovedEngressmentIssued:
                    return new OptionSetValue((int)invln_ExternalStatusAHP.ApprovedSubjectToContract);

                case (int)invln_scheme_StatusCode.ApprovedContractReceivedBackToHE:
                    return new OptionSetValue((int)invln_ExternalStatusAHP.ApprovedSubjectToContract);

                case (int)invln_scheme_StatusCode.ApprovedContractPassedComplianceChecks:
                    return new OptionSetValue((int)invln_ExternalStatusAHP.ApprovedSubjectToContract);

                case (int)invln_scheme_StatusCode.ApprovedContractExecuted:
                    return new OptionSetValue((int)invln_ExternalStatusAHP.ApprovedSubjectToContract);

                case (int)invln_scheme_StatusCode.Approved:
                    return new OptionSetValue((int)invln_ExternalStatusAHP.Approved);

                default:
                    return null;
            }
        }

        private int MapAhpStatusCodeToStateCode(int statusCode)
        {
            if (statusCode == (int)invln_AHPInternalStatus.Inactive || statusCode == (int)invln_AHPInternalStatus.Withdrawn || statusCode == (int)invln_AHPInternalStatus.Deleted)
            {
                return 1;
            }
            else
            {
                return 0;
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
    }
}
