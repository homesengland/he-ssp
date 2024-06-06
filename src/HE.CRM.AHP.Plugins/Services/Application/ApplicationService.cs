using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text.Json;
using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.AHP.Plugins.Repositories;
using HE.CRM.AHP.Plugins.Services.GovNotifyEmail;
using HE.CRM.Common.DtoMapping;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

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
        private readonly ISiteRepository _siteRepository;
        private readonly IHeLocalAuthorityRepository _heLocalAuthorityRepository;
        private readonly IGovNotifyEmailService _govNotifyEmailService;
        private readonly IAhpProjectRepository _projectRepository;

        public ApplicationService(CrmServiceArgs args) : base(args)
        {
            _applicationRepository = CrmRepositoriesFactory.Get<IAhpApplicationRepository>();
            _contactRepository = CrmRepositoriesFactory.Get<IContactRepository>();
            _sharepointDocumentLocationRepository = CrmRepositoriesFactory.Get<ISharepointDocumentLocationRepository>();
            _sharepointSiteRepository = CrmRepositoriesFactory.Get<ISharepointSiteRepository>();
            _ahpApplicationRepositoryAdmin = CrmRepositoriesFactory.GetSystem<IAhpApplicationRepository>();
            _ahpStatusChangeRepository = CrmRepositoriesFactory.Get<IAhpStatusChangeRepository>();
            _siteRepository = CrmRepositoriesFactory.Get<ISiteRepository>();
            _heLocalAuthorityRepository = CrmRepositoriesFactory.Get<IHeLocalAuthorityRepository>();
            _govNotifyEmailService = CrmServicesFactory.Get<IGovNotifyEmailService>();
            _projectRepository = CrmRepositoriesFactory.Get<IAhpProjectRepository>();
        }

        public void ChangeApplicationStatus(string organisationId, string contactId, string applicationId, int newStatus, string changeReason, bool representationsandwarranties)
        {
            Logger.Trace($"Service ChangeApplicationStatus");
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

            if (application != null && application.invln_organisationid.Id == new Guid(organisationId))
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
            if (Guid.TryParse(ahpApplicationId, out var applicationGuid))
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
            TracingService.Trace("GetApplication");
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
                    invln_ahpproject ahpProject = null;
                    if (application.invln_Site != null)
                    {
                        var site = _siteRepository.GetById(application.invln_Site.Id, invln_Sites.Fields.invln_AHPProjectId);
                        if (site.invln_AHPProjectId != null)
                        {
                            ahpProject = _projectRepository.GetById(site.invln_AHPProjectId.Id, invln_ahpproject.Fields.invln_HeProjectId);
                        }
                    }

                    var applicationDto = AhpApplicationMapper.MapRegularEntityToDto(application, contact.invln_externalid, ahpProject);
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

            he_LocalAuthority localAuthority = null;
            if (Guid.TryParse(application.siteId, out var siteGuid))
            {
                var site = _siteRepository.GetById(siteGuid, invln_Sites.Fields.invln_HeLocalAuthorityId);
                if (site.invln_HeLocalAuthorityId != null)
                {
                    localAuthority = _heLocalAuthorityRepository.GetById(site.invln_HeLocalAuthorityId.Id, new string[] { he_LocalAuthority.Fields.he_growthmanager, he_LocalAuthority.Fields.he_growthhub });
                }
            }

            var applicationMapped = AhpApplicationMapper.MapDtoToRegularEntity(application, contact.Id.ToString(), organisationId, localAuthority);
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
                            if (field == "invln_representationsandwarrantiesconfirmation")
                            {
                                if (applicationMapped.invln_representationsandwarrantiesconfirmation.HasValue)
                                {
                                    applicationToUpdateOrCreate.invln_representationsandwarrantiesconfirmation = applicationMapped.invln_representationsandwarrantiesconfirmation;
                                }
                            }
                            else
                            {
                                applicationToUpdateOrCreate[field] = applicationMapped[field];
                            }
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

                case (int)invln_scheme_StatusCode.UnderReviewGoingToBidClinic:
                    return new OptionSetValue((int)invln_ExternalStatusAHP.UnderReview);

                case (int)invln_scheme_StatusCode.UnderReviewGoingToCMEModeration:
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

        public void GrantCalculate(invln_scheme application)
        {
            Logger.Trace($"ApplicationService.GrantCalculate: {application.Id}");
            var grantbenchmarkRepository = CrmRepositoriesFactory.GetSystem<IGrantBenchmarkRepository>();
            var sitesRepository = CrmRepositoriesFactory.GetSystemBase<invln_Sites, DataverseContext>();

            if (application.invln_fundingrequired == null)
            {
                throw new Exception("invln_scheme.invln_fundingrequired is empty");
            }

            if (!application.invln_noofhomes.HasValue || application.invln_noofhomes.Value < 1)
            {
                throw new Exception("invln_scheme.invln_noofhomes is empty");
            }

            if (application.invln_expectedacquisitioncost == null && application.invln_actualacquisitioncost == null)
            {
                throw new Exception("invln_scheme.invln_expectedacquisitioncost and invln_scheme.invln_actualacquisitioncost are empty");
            }

            if (application.invln_expectedoncosts == null)
            {
                throw new Exception("invln_scheme.invln_expectedoncosts is empty");
            }

            if (application.invln_expectedonworks == null)
            {
                throw new Exception("invln_scheme.invln_expectedonworks is empty");
            }

            if (application.invln_Site == null)
            {
                throw new Exception("invln_scheme.invln_Site is empty");
            }

            if (application.invln_Tenure == null)
            {
                throw new Exception("invln_scheme.invln_Tenure is empty");
            }

            var fundingRequired = application.invln_fundingrequired.Value;
            var noOfHomes = application.invln_noofhomes.Value;

            var acquisitionCost = application.invln_expectedacquisitioncost ?? application.invln_actualacquisitioncost;

            var expectedOnCosts = application.invln_expectedoncosts.Value;
            var expectedOnWorks = application.invln_expectedonworks.Value;

            var grantPerUnit = fundingRequired / noOfHomes;

            var grantasaoftotalschemecosts = fundingRequired / (acquisitionCost.Value + expectedOnCosts + expectedOnWorks) * 100;

            var site = sitesRepository.GetById(application.invln_Site.Id,
                invln_Sites.Fields.invln_GovernmentOfficeRegion);

            if (site.invln_GovernmentOfficeRegion == null)
            {
                throw new Exception($"Site {site.Id} has no set invln_GovernmentOfficeRegion");
            }

            var homeTypes = GetHomeTypes(application.Id);

            var areHousingForDisabledVulnerableOrOlderPeople = homeTypes
                .Any(x => x.invln_typeofhousing.Value == (int)invln_Typeofhousing.Housingfordisabledandvulnerablepeople || x.invln_typeofhousing.Value == (int)invln_Typeofhousing.Housingforolderpeople);

            var tenure = MapApplicationTenureToRegionalBenchmarkTenure((invln_Tenure)application.invln_Tenure.Value, areHousingForDisabledVulnerableOrOlderPeople);

            var grantBenchmarks = grantbenchmarkRepository.GetGrantBenchmarks(
                tenure,
                site.invln_GovernmentOfficeRegion.Value,
                GrantBenchmarkRepository.DefaultColumns
            );

            var grantBenchmarkTable5 = grantBenchmarks.Single(x => x.invln_BenchmarkTable.Value == (int)invln_BenchmarkTable.Table5RegionalBenchmarkGrantPerUnit);
            if (grantBenchmarkTable5.invln_benchmarkgpu == null)
            {
                throw new Exception($"Grant benchmark with id '{grantBenchmarkTable5.Id}' has empty invln_benchmarkgpu field!");
            }

            var regionalBenchmarkGrantPerUnit = grantBenchmarkTable5.invln_benchmarkgpu;
            var regionalBenchmarkAgainstTheGrantPerUnit = grantPerUnit / regionalBenchmarkGrantPerUnit.Value * 100;
            var workCostM2 = CalculateWorksCostM2(application, homeTypes);

            var grantBenchmarkTable1 = grantBenchmarks.Single(x => x.invln_BenchmarkTable.Value == (int)invln_BenchmarkTable.Table1AreaaveragesforGrantasofTotalSchemeCosts);
            if (!grantBenchmarkTable1.invln_BenchmarkValuePercentage.HasValue)
            {
                throw new Exception($"Grant benchmark with id '{grantBenchmarkTable1.Id}' has empty invln_BenchmarkValuePercentage field!");
            }

            var grantAsPercentageOfTotalSchemeCosts = grantasaoftotalschemecosts / grantBenchmarkTable1.invln_BenchmarkValuePercentage.Value;

            var grantBenchmarkTable2 = grantBenchmarks.Single(x => x.invln_BenchmarkTable.Value == (int)invln_BenchmarkTable.Table2Areaaveragesforworkscostsperm2);
            if (grantBenchmarkTable2.invln_benchmarkgpu == null)
            {
                throw new Exception($"Grant benchmark with id '{grantBenchmarkTable2.Id}' has empty invln_benchmarkgpu field!");
            }

            var worksM2AsPercentageOfAreaAvg = workCostM2.HasValue ? (decimal?)workCostM2.Value / grantBenchmarkTable2.invln_benchmarkgpu.Value : null;

            var grantBenchmarkTable3 = grantBenchmarks.Single(x => x.invln_BenchmarkTable.Value == (int)invln_BenchmarkTable.Table3Areaaveragesforruralgrantperunit);
            if (grantBenchmarkTable3.invln_benchmarkgpu == null)
            {
                throw new Exception($"Grant benchmark with id '{grantBenchmarkTable3.Id}' has empty invln_benchmarkgpu field!");
            }

            var areHousingForOlderPeoplesWithAllFeaturesHousing = homeTypes
                .Any(x => new OptionSetValue((int)invln_typeofolderpeopleshousing.Housingforolderpeoplewithallspecialdesignfeatures).Equals(x.invln_typeofolderpeopleshousing));

            var arePurposeDesignedForDisabledTypeOfHousing = homeTypes
                .Any(x => new OptionSetValue((int)invln_typeofhousingfordisabledvulnerable.Purposedesignedhousingfordisabledandvulnerablepeoplewithaccesstosupport).Equals(x.invln_typeofhousingfordisabledvulnerablepeople)
                    || new OptionSetValue((int)invln_typeofhousingfordisabledvulnerable.Purposedesignedsupportedhousingfordisabledandvulnerablepeople).Equals(x.invln_typeofhousingfordisabledvulnerablepeople));

            var grantBenchmarkTable4 = grantBenchmarks.Single(x => x.invln_BenchmarkTable.Value == (int)invln_BenchmarkTable.Table4Areaaveragesforsupportedhousinggrantperunit);
            if (grantBenchmarkTable4.invln_benchmarkgpu == null)
            {
                throw new Exception($"Grant benchmark with id '{grantBenchmarkTable4.Id}' has empty invln_benchmarkgpu field!");
            }

            var isSupportedGpuAsPercentageOfAreaAverage = false;
            if ((application.invln_Tenure.Value == (int)invln_Tenure.Affordablerent || application.invln_Tenure.Value == (int)invln_Tenure.Socialrent) &&
                areHousingForDisabledVulnerableOrOlderPeople &&
                (areHousingForOlderPeoplesWithAllFeaturesHousing || arePurposeDesignedForDisabledTypeOfHousing))
            {
                isSupportedGpuAsPercentageOfAreaAverage = true;
            }

            var deliveryPahses = GetMilesoneClaimDatesForDeliveryPhases(application.Id);
            var earliestStartMilestoneClaimDate = deliveryPahses.Min(x => x.invln_startonsitemilestoneclaimdate);
            if (!earliestStartMilestoneClaimDate.HasValue)
            {
                earliestStartMilestoneClaimDate = deliveryPahses.Min(x => x.invln_completionmilestoneclaimdate);
            }

            var latestCompletionMilestoneClaimDate = deliveryPahses.Max(x => x.invln_completionmilestoneclaimdate);

            var sosScore = CalculateScore(earliestStartMilestoneClaimDate);
            var compScore = CalculateScore(latestCompletionMilestoneClaimDate);

            _ahpApplicationRepositoryAdmin.Update(new invln_scheme()
            {
                Id = application.Id,
                invln_grantperunit = new Money(grantPerUnit),
                invln_grantasaoftotalschemecosts = grantasaoftotalschemecosts,
                invln_RegionalBenchmarkGrantPerUnit = regionalBenchmarkGrantPerUnit,
                invln_regionalbenchmarkagainstthegrantperunit = regionalBenchmarkAgainstTheGrantPerUnit,
                invln_WorkssCostsm2 = workCostM2.HasValue ? new Money(workCostM2.Value) : null,
                invln_grantasapercentageoftotalschemecosts = grantAsPercentageOfTotalSchemeCosts,
                invln_worksm2asapercentageofareaavg = worksM2AsPercentageOfAreaAvg.HasValue ? worksM2AsPercentageOfAreaAvg : null,
                invln_gpuaspercentageofareaaverage = application.invln_Rural == true ? grantPerUnit / grantBenchmarkTable3.invln_benchmarkgpu.Value : (decimal?)null,
                invln_supportedgpuaspercentageofareaaverage = isSupportedGpuAsPercentageOfAreaAverage ? grantPerUnit / grantBenchmarkTable4.invln_benchmarkgpu.Value : (decimal?)null,
                invln_SoSScore = sosScore,
                invln_CompScore = compScore
            });
        }

        private IEnumerable<invln_HomeType> GetHomeTypes(Guid applicationId)
        {
            Logger.Trace($"GetHomeTypes applicationId='{applicationId}'");
            var homeTypesRepository = CrmRepositoriesFactory.GetSystemBase<invln_HomeType, DataverseContext>();

            var homeTypesColumns = new string[] {
                    invln_HomeType.Fields.invln_numberofhomeshometype,
                    invln_HomeType.Fields.invln_floorarea,
                    invln_HomeType.Fields.invln_typeofhousing,
                    invln_HomeType.Fields.invln_typeofhousingfordisabledvulnerablepeople,
                    invln_HomeType.Fields.invln_typeofolderpeopleshousing
            };
            return homeTypesRepository.GetByAttribute(invln_HomeType.Fields.invln_application, applicationId, homeTypesColumns);
        }

        private decimal? CalculateWorksCostM2(invln_scheme application, IEnumerable<invln_HomeType> homeTypes)
        {
            if (!homeTypes.Any())
            {
                Logger.Warn($"Could not find any home types for application '{application.Id}'");
                return null;
            }
            var sumFloorArea = homeTypes.Select(x => x.invln_numberofhomeshometype * x.invln_floorarea).Sum();
            var calculationResult = application.invln_expectedonworks.Value / sumFloorArea.Value;
            return calculationResult;
        }

        private invln_Tenurechoice MapApplicationTenureToRegionalBenchmarkTenure(invln_Tenure ahpApplicationTenure, bool areHousingForDisabledVulnerableOlderPeople)
        {
            if (ahpApplicationTenure == invln_Tenure.Sharedownership ||
                ahpApplicationTenure == invln_Tenure.OPSO ||
                ahpApplicationTenure == invln_Tenure.HOLD)
            {
                return invln_Tenurechoice.Sharedownership;
            }

            if (ahpApplicationTenure == invln_Tenure.Renttobuy)
            {
                return invln_Tenurechoice.Renttobuy;
            }

            if (ahpApplicationTenure == invln_Tenure.Affordablerent)
            {
                if (areHousingForDisabledVulnerableOlderPeople)
                {
                    return invln_Tenurechoice.Specialistrent;
                }

                return invln_Tenurechoice.Affordablerent;
            }

            if (ahpApplicationTenure == invln_Tenure.Socialrent)
            {
                if (areHousingForDisabledVulnerableOlderPeople)
                {
                    return invln_Tenurechoice.Specialistrent;
                }

                return invln_Tenurechoice.Socialrent;
            }

            throw new Exception($"Unknown tenure value: {ahpApplicationTenure}");
        }

        private IEnumerable<invln_DeliveryPhase> GetMilesoneClaimDatesForDeliveryPhases(Guid applicationId)
        {
            var deliveryPhaseRepository = CrmRepositoriesFactory.GetSystemBase<invln_DeliveryPhase, DataverseContext>();

            var query = new QueryExpression(invln_DeliveryPhase.EntityLogicalName)
            {
                TopCount = 100,
                ColumnSet = new ColumnSet(
                    invln_DeliveryPhase.Fields.invln_startonsitemilestoneclaimdate,
                    invln_DeliveryPhase.Fields.invln_completionmilestoneclaimdate),
                Criteria = new FilterExpression(LogicalOperator.And)
                {
                    Conditions =
                    {
                        new ConditionExpression
                        {
                            AttributeName = invln_DeliveryPhase.Fields.invln_Application,
                            Operator = ConditionOperator.Equal,
                            Values = { applicationId }
                        }
                    }
                }
            };

            return deliveryPhaseRepository.RetrieveAll(query).Entities.Select(e => e.ToEntity<invln_DeliveryPhase>());
        }

        private decimal? CalculateScore(DateTime? date)
        {
            if (date == null)
            {
                return null;
            }

            if (date >= new DateTime(2023, 4, 1) && date <= new DateTime(2023, 12, 31))
            {
                return 10m;
            }
            else if (date >= new DateTime(2024, 1, 1) && date <= new DateTime(2024, 12, 31))
            {
                return 8m;
            }
            else if (date >= new DateTime(2025, 1, 1) && date <= new DateTime(2025, 12, 31))
            {
                return 6m;
            }
            else if (date >= new DateTime(2026, 1, 1) && date <= new DateTime(2026, 3, 31))
            {
                return 4m;
            }
            else
            {
                return null;
            }
        }
    }
}
