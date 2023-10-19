using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.DtoMapping;
using HE.CRM.Common.Repositories.interfaces;
using HE.CRM.Common.Repositories.Interfaces;
using HE.CRM.Model.CrmSerializedParameters;
using HE.CRM.Plugins.Services.GovNotifyEmail;
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

namespace HE.CRM.Plugins.Services.LoanApplication
{
    public class LoanApplicationService : CrmService, ILoanApplicationService
    {
        #region Fields

        private readonly ILoanApplicationRepository _loanApplicationRepository;
        private readonly ISiteDetailsRepository _siteDetailsRepository;
        private readonly IContactRepository _contactRepository;
        private readonly IWebRoleRepository _webroleRepository;
        private readonly ILoanStatusChangeRepository _loanStatusChangeRepository;
        private readonly ISharepointDocumentLocationRepository _sharepointDocumentLocationRepository;

        private readonly ILoanApplicationRepository _loanApplicationRepositoryAdmin;
        private readonly INotificationSettingRepository _notificationSettingRepositoryAdmin;
        private readonly IGovNotifyEmailRepository _govNotifyEmailRepositoryAdmin;
        private readonly IEnvironmentVariableRepository _environmentVariableRepositoryAdmin;
        private readonly ISystemUserRepository _systemUserRepositoryAdmin;
        private readonly ITeamRepository _teamRepositoryAdmin;

        private readonly IGovNotifyEmailService _govNotifyEmailService;

        #endregion

        #region Constructors

        public LoanApplicationService(CrmServiceArgs args) : base(args)
        {
            _loanApplicationRepository = CrmRepositoriesFactory.Get<ILoanApplicationRepository>();
            _siteDetailsRepository = CrmRepositoriesFactory.Get<ISiteDetailsRepository>();
            _contactRepository = CrmRepositoriesFactory.Get<IContactRepository>();
            _webroleRepository = CrmRepositoriesFactory.Get<IWebRoleRepository>();
            _loanStatusChangeRepository = CrmRepositoriesFactory.Get<ILoanStatusChangeRepository>();
            _sharepointDocumentLocationRepository = CrmRepositoriesFactory.Get<ISharepointDocumentLocationRepository>();

            _loanApplicationRepositoryAdmin = CrmRepositoriesFactory.GetSystem<ILoanApplicationRepository>();
            _notificationSettingRepositoryAdmin = CrmRepositoriesFactory.GetSystem<INotificationSettingRepository>();
            _govNotifyEmailRepositoryAdmin = CrmRepositoriesFactory.GetSystem<IGovNotifyEmailRepository>();
            _environmentVariableRepositoryAdmin = CrmRepositoriesFactory.GetSystem<IEnvironmentVariableRepository>();
            _systemUserRepositoryAdmin = CrmRepositoriesFactory.GetSystem<ISystemUserRepository>();
            _teamRepositoryAdmin = CrmRepositoriesFactory.GetSystem<ITeamRepository>();

            _govNotifyEmailService = CrmServicesFactory.Get<IGovNotifyEmailService>();
        }

        #endregion

        #region Public Methods

        public string GetLoanApplicationsForAccountAndContact(string externalContactId, string accountId, string loanApplicationId = null, string fieldsToRetrieve = null)
        {
            List<LoanApplicationDto> entityCollection = new List<LoanApplicationDto>();
            if (Guid.TryParse(accountId, out Guid accountGuid))
            {
                var contact = _contactRepository.GetContactViaExternalId(externalContactId);
                var role = _webroleRepository.GetContactWebRole(contact.Id, ((int)invln_Portal1.Loans).ToString());
                List<invln_Loanapplication> loanApplicationsForAccountAndContact;
                if (role.Any(x => x.Contains("pl.invln_permission") && ((OptionSetValue)((AliasedValue)x["pl.invln_permission"]).Value).Value == (int)invln_Permission.Accountadministrator) && loanApplicationId == null)
                {
                    TracingService.Trace("admin");
                    loanApplicationsForAccountAndContact = _loanApplicationRepository.GetAccountLoans(accountGuid);
                }
                else
                {
                    TracingService.Trace("regular user, not admin");
                    string attributes = null;
                    if (!string.IsNullOrEmpty(fieldsToRetrieve))
                    {
                        attributes = GenerateFetchXmlAttributes(fieldsToRetrieve);
                    }
                    loanApplicationsForAccountAndContact = _loanApplicationRepository.GetLoanApplicationsForGivenAccountAndContact(accountGuid, externalContactId, loanApplicationId, attributes);
                }
                this.TracingService.Trace("GetLoanApplicationsForGivenAccountAndContact");
                this.TracingService.Trace($"{loanApplicationsForAccountAndContact.Count}");
                foreach (var element in loanApplicationsForAccountAndContact)
                {
                    List<SiteDetailsDto> siteDetailsDtoList = new List<SiteDetailsDto>();
                    this.TracingService.Trace($"Loan application id {element.Id}");
                    this.TracingService.Trace("GetSiteDetailRelatedToLoanApplication");
                    var siteDetailsList = _siteDetailsRepository.GetSiteDetailRelatedToLoanApplication(element.ToEntityReference());
                    if (siteDetailsList != null)
                    {
                        foreach (var siteDetail in siteDetailsList)
                        {
                            this.TracingService.Trace("MapSiteDetailsToDto");
                            siteDetailsDtoList.Add(SiteDetailsDtoMapper.MapSiteDetailsToDto(siteDetail));
                        }
                    }
                    this.TracingService.Trace("MapLoanApplicationToDto");
                    Contact loanApplicationContact = null;
                    if (element.invln_Contact != null)
                    {
                        loanApplicationContact = this._contactRepository.GetById(element.invln_Contact.Id, new string[]
                        {
                            nameof(Contact.EMailAddress1).ToLower(),
                            nameof(Contact.FirstName).ToLower(),
                            nameof(Contact.LastName).ToLower(),
                            nameof(Contact.invln_externalid).ToLower(),
                            nameof(Contact.Telephone1).ToLower(),
                        });
                    }

                    entityCollection.Add(LoanApplicationDtoMapper.MapLoanApplicationToDto(element, siteDetailsDtoList, externalContactId, loanApplicationContact));
                }
            }

            this.TracingService.Trace("Serialize");
            return JsonSerializer.Serialize(entityCollection);
        }

        public void ChangeLoanApplicationStatusOnOwnerChange(invln_Loanapplication target, invln_Loanapplication preImage, invln_Loanapplication postImage)
        {
            if (preImage?.StatusCode.Value == (int)invln_Loanapplication_StatusCode.ApplicationSubmitted && preImage.OwnerId.Id != postImage.OwnerId.Id)
            {
                _loanApplicationRepository.Update(new invln_Loanapplication()
                {
                    Id = target.Id,
                    invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.ApplicationUnderReview),
                    StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.ApplicationUnderReview),
                });

                var loanStatusChangeToCreate = new invln_Loanstatuschange()
                {
                    invln_changefrom = new OptionSetValue((int)invln_InternalStatus.ApplicationSubmitted),
                    invln_changesource = new OptionSetValue((int)invln_ChangesourceSet.Automated),
                    invln_changeto = new OptionSetValue((int)invln_InternalStatus.ApplicationUnderReview),
                    invln_Loanapplication = target.ToEntityReference()
                };

                _loanStatusChangeRepository.Create(loanStatusChangeToCreate);
            }
        }

        public string CreateRecordFromPortal(string contactExternalId, string accountId, string loanApplicationId, string loanApplicationPayload)
        {
            this.TracingService.Trace("PAYLOAD:" + loanApplicationPayload);

            LoanApplicationDto loanApplicationFromPortal = JsonSerializer.Deserialize<LoanApplicationDto>(loanApplicationPayload);
            //THIS IS CONTACT WHO IS SENDING MESSAGE
            var requestContact = _contactRepository.GetContactViaExternalId(contactExternalId);

            //Update Contact on Loan Application
            Contact loanApplicationContact = null;
            if (loanApplicationFromPortal?.LoanApplicationContact?.ContactExternalId != null)
            {
                //THIS IS CONTACT FOR WHICH LOAN IS CREATED
                var contactExternalid = loanApplicationFromPortal?.LoanApplicationContact?.ContactExternalId ?? contactExternalId;
                loanApplicationContact = _contactRepository.GetContactViaExternalId(contactExternalid);
                _contactRepository.Update(new Contact()
                {
                    Id = loanApplicationContact.Id,
                    FirstName = loanApplicationFromPortal.LoanApplicationContact.ContactFirstName,
                    LastName = loanApplicationFromPortal.LoanApplicationContact.ContactLastName,
                    EMailAddress1 = loanApplicationFromPortal.LoanApplicationContact.ContactEmail,
                    Telephone1 = loanApplicationFromPortal.LoanApplicationContact.ContactTelephoneNumber,
                });
            }

            //Number of sites
            int numberOfSites = 0;
            if (loanApplicationFromPortal.siteDetailsList != null && loanApplicationFromPortal.siteDetailsList.Count > 0)
            {
                numberOfSites = loanApplicationFromPortal.siteDetailsList.Count;
            }

            this.TracingService.Trace("Setting up invln_Loanapplication");
            loanApplicationFromPortal.numberOfSites = numberOfSites.ToString();

            var loanApplicationToCreate = LoanApplicationDtoMapper.MapLoanApplicationDtoToRegularEntity(loanApplicationFromPortal, loanApplicationContact, accountId);
            loanApplicationToCreate.invln_lastmmodificationdate = DateTime.UtcNow;
            loanApplicationToCreate.invln_lastchangebyid = requestContact.ToEntityReference();
            Guid loanApplicationGuid = Guid.NewGuid();
            if (!string.IsNullOrEmpty(loanApplicationId) && Guid.TryParse(loanApplicationId, out Guid loanAppId))
            {
                this.TracingService.Trace("Update invln_Loanapplication");
                loanApplicationGuid = loanAppId;
                loanApplicationToCreate.Id = loanAppId;
                _loanApplicationRepository.Update(loanApplicationToCreate);
            }
            else
            {
                if (loanApplicationToCreate.invln_Contact == null)
                {
                    loanApplicationToCreate.invln_Contact = requestContact.ToEntityReference();
                }

                this.TracingService.Trace("Create invln_Loanapplication");
                loanApplicationToCreate.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.Draft);
                loanApplicationGuid = _loanApplicationRepository.Create(loanApplicationToCreate);
            }

            if (loanApplicationFromPortal.siteDetailsList != null && loanApplicationFromPortal.siteDetailsList.Count > 0)
            {
                this.TracingService.Trace($"siteDetailsList.Count {loanApplicationFromPortal.siteDetailsList.Count}");
                foreach (var siteDetail in loanApplicationFromPortal.siteDetailsList)
                {
                    this.TracingService.Trace("loop begin");
                    var siteDetailToCreate = SiteDetailsDtoMapper.MapSiteDetailsDtoToRegularEntity(siteDetail, loanApplicationGuid.ToString());
                    this.TracingService.Trace("create");
                    if (!String.IsNullOrEmpty(siteDetail.siteDetailsId) && Guid.TryParse(siteDetail.siteDetailsId, out Guid result))
                    {
                        _siteDetailsRepository.Update(siteDetailToCreate);
                    }
                    else
                    {
                        _siteDetailsRepository.Create(siteDetailToCreate);
                    }
                    this.TracingService.Trace("after create record");
                }
            }

            return loanApplicationGuid.ToString();
        }

        public void ChangeLoanApplicationExternalStatus(int externalStatus, string loanApplicationId, string changeReason)
        {
            TracingService.Trace($"loan id {loanApplicationId}");
            TracingService.Trace($"new external status {externalStatus}");
            if (Guid.TryParse(loanApplicationId, out Guid loanId) && externalStatus != null)
            {

                var retrievedLoanApplication = _loanApplicationRepository.GetById(loanId, new string[] { nameof(invln_Loanapplication.invln_ExternalStatus).ToLower(), nameof(invln_Loanapplication.StatusCode).ToLower() });
                if (externalStatus == retrievedLoanApplication.invln_ExternalStatus.Value)
                {
                    return;
                }

                var loanWithNewStatusCodes = MapExternalStatusToInternal(new OptionSetValue(externalStatus));
                var loanToUpdate = new invln_Loanapplication()
                {
                    Id = loanId,
                    StatusCode = loanWithNewStatusCodes.StatusCode,
                    StateCode = loanWithNewStatusCodes.StateCode,
                    invln_ExternalStatus = new OptionSetValue(externalStatus),
                };

                var loanStatusChangeToCreate = new invln_Loanstatuschange()
                {
                    invln_changefrom = retrievedLoanApplication.StatusCode,
                    invln_changesource = new OptionSetValue((int)invln_ChangesourceSet.External),
                    invln_changeto = loanWithNewStatusCodes.StatusCode,
                    invln_Loanapplication = loanToUpdate.ToEntityReference()
                };

                if (!string.IsNullOrEmpty(changeReason))
                {
                    loanToUpdate.invln_statuschangereason = changeReason;
                }
                TracingService.Trace("update loan application");
                _loanApplicationRepository.Update(loanToUpdate);
                _ = _loanStatusChangeRepository.Create(loanStatusChangeToCreate);
            }
        }


        public void UpdateLoanApplication(string loanApplicationId, string loanApplication, string fieldsToUpdate, string accountId, string contactExternalId)
        {
            if (Guid.TryParse(loanApplicationId, out Guid applicationId))
            {
                var deserializedLoanApplication = JsonSerializer.Deserialize<LoanApplicationDto>(loanApplication);
                var contactExternalid = deserializedLoanApplication?.LoanApplicationContact?.ContactExternalId ?? contactExternalId;
                var contact = _contactRepository.GetContactViaExternalId(contactExternalid);
                var loanApplicationMapped = LoanApplicationDtoMapper.MapLoanApplicationDtoToRegularEntity(deserializedLoanApplication, contact, accountId);
                var loanApplicationToUpdate = new invln_Loanapplication();
                if (string.IsNullOrEmpty(fieldsToUpdate))
                {
                    loanApplicationToUpdate = loanApplicationMapped;
                }
                else
                {
                    var fields = fieldsToUpdate.Split(',');
                    if (fields.Length > 0)
                    {
                        foreach (var field in fields)
                        {
                            if (string.Equals(field.ToLower(), nameof(invln_Loanapplication.invln_ExternalStatus).ToLower()))
                            {
                                var retrievedLoanApplicationStatus = _loanApplicationRepository.GetById(applicationId, new string[] { nameof(invln_Loanapplication.invln_ExternalStatus).ToLower() }).invln_ExternalStatus;
                                int oldStatus = retrievedLoanApplicationStatus != null ? retrievedLoanApplicationStatus.Value : 0;
                                CheckIfExternalStatusCanBeChanged(oldStatus, loanApplicationMapped.invln_ExternalStatus.Value);
                            }
                            TracingService.Trace($"field name {field}");
                            loanApplicationToUpdate[field] = loanApplicationMapped[field];
                        }
                    }
                }
                loanApplicationToUpdate.Id = applicationId;
                loanApplicationToUpdate.invln_lastmmodificationdate = DateTime.UtcNow;
                loanApplicationToUpdate.invln_lastchangebyid = contact.ToEntityReference();

                _loanApplicationRepository.Update(loanApplicationToUpdate);
            }
        }

        public void DeleteLoanApplication(string loanApplicationId)
        {
            if (Guid.TryParse(loanApplicationId, out Guid applicationId))
            {
                var loanApplicationToUpdate = new invln_Loanapplication()
                {
                    Id = applicationId,
                    invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.Withdrawn),
                    StateCode = new OptionSetValue(1),
                };
                _loanApplicationRepository.Update(loanApplicationToUpdate);

                var relatedSiteDetails = _siteDetailsRepository.GetSiteDetailRelatedToLoanApplication(loanApplicationToUpdate.ToEntityReference());
                if (relatedSiteDetails != null && relatedSiteDetails.Count > 0)
                {
                    foreach (var siteDetail in relatedSiteDetails)
                    {
                        var siteDetailToUpdate = new invln_SiteDetails()
                        {
                            Id = siteDetail.Id,
                            StateCode = new OptionSetValue(1),
                        };
                        _siteDetailsRepository.Update(siteDetailToUpdate);
                    }
                }
            }
        }

        public void CheckIfOwnerCanBeChanged(invln_Loanapplication target, invln_Loanapplication preImage)
        {
            TracingService.Trace($"preImage: value {preImage.StatusCode.Value}, other {preImage.StatusCode}");
            TracingService.Trace($"target: value {target.StatusCode?.Value}, other {target.StatusCode}");
            if (((preImage.StatusCode.Value == (int)invln_Loanapplication_StatusCode.Draft && target.StatusCode == null) || (target.StatusCode != null && target.StatusCode.Value == (int)invln_Loanapplication_StatusCode.Draft)) && target.OwnerId.Id != preImage.OwnerId.Id && target.OwnerId.LogicalName != Team.EntityLogicalName)
            {
                throw new InvalidPluginExecutionException("Cannot change owner when status is set to draft");
            }
        }

        public void SetFieldsWhenChangingStatusFromDraft(invln_Loanapplication target, invln_Loanapplication preImage)
        {
            if ((preImage.StatusCode != null && target.StatusCode != null && preImage.StatusCode.Value == (int)invln_Loanapplication_StatusCode.Draft
                && target.StatusCode.Value == (int)invln_Loanapplication_StatusCode.ApplicationSubmitted) ||
                (preImage.invln_ExternalStatus != null && target.invln_ExternalStatus != null && preImage.invln_ExternalStatus.Value == (int)invln_ExternalStatus.Draft
                && target.invln_ExternalStatus.Value == (int)invln_ExternalStatus.ApplicationSubmitted))
            {
                var relatedSiteDetails = _siteDetailsRepository.GetSiteDetailRelatedToLoanApplication(target.ToEntityReference());
                if (relatedSiteDetails != null && relatedSiteDetails.Count > 0)
                {
                    var projectName = string.Empty;
                    var numberOfHomes = 0;
                    var isFirst = true;
                    foreach (var siteDetail in relatedSiteDetails)
                    {
                        if (isFirst)
                        {
                            projectName += $"{siteDetail.invln_Name}";
                            isFirst = false;
                        }
                        else
                        {
                            projectName += $", {siteDetail.invln_Name}";
                        }
                        numberOfHomes += siteDetail.invln_Numberofhomes ?? 0;
                    }
                    if (projectName.Length > 100)
                    {
                        projectName = projectName.Substring(0, 100);
                    }
                    target.invln_ProjectName = projectName;
                    target.invln_numberofhomes = numberOfHomes.ToString();
                }
            }
        }
        public void SendInternalNotificationOnStatusChange(invln_Loanapplication target, invln_Loanapplication preImage)
        {
            if (target.StatusCode != null && preImage.StatusCode != null && target.StatusCode.Value != preImage.StatusCode.Value)
            {
                switch (target.StatusCode.Value)
                {
                    case (int)invln_Loanapplication_StatusCode.Draft:
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.Draft);
                        break;
                    case (int)invln_Loanapplication_StatusCode.ApplicationSubmitted:
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.ApplicationSubmitted);
                        break;
                    case (int)invln_Loanapplication_StatusCode.Inactive:
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.NA);
                        break;
                    case (int)invln_Loanapplication_StatusCode.ApplicationUnderReview:
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.ApplicationUnderReview);
                        break;
                    case (int)invln_Loanapplication_StatusCode.HoldRequested:
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.HoldRequested);
                        break;
                    case (int)invln_Loanapplication_StatusCode.Withdrawn:
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.Withdrawn);
                        break;
                    case (int)invln_Loanapplication_StatusCode.CashflowRequested:
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.CashflowRequested);
                        break;
                    case (int)invln_Loanapplication_StatusCode.CashflowUnderReview:
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.CashflowUnderReview);
                        break;
                    case (int)invln_Loanapplication_StatusCode.OnHold:
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.OnHold);
                        break;
                    case (int)invln_Loanapplication_StatusCode.ReferredBacktoApplicant:
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.ReferredBacktoApplicant);
                        break;
                    case (int)invln_Loanapplication_StatusCode.UnderReview:
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.UnderReview);
                        break;
                    case (int)invln_Loanapplication_StatusCode.SentforApproval:
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.SentforApproval);
                        break;
                    case (int)invln_Loanapplication_StatusCode.NotApproved:
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.SentforApproval);
                        break;
                    case (int)invln_Loanapplication_StatusCode.ApprovedSubjecttoDueDiligence:
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.ApprovedSubjecttoDueDiligence);
                        break;
                    case (int)invln_Loanapplication_StatusCode.ApplicationDeclined:
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.ApplicationDeclined);
                        break;
                    case (int)invln_Loanapplication_StatusCode.InDueDiligence:
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.InDueDiligence);
                        break;
                    case (int)invln_Loanapplication_StatusCode.SentforPreCompleteApproval:
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.InDueDiligence);
                        break;
                    case (int)invln_Loanapplication_StatusCode.ApprovedSubjectToContract:
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.ApprovedSubjecttoContract);
                        break;
                    case (int)invln_Loanapplication_StatusCode.CPsSatisfied:
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.CPsSatisfied);
                        break;
                    case (int)invln_Loanapplication_StatusCode.AwaitingCPSatisfaction:
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.ContractSignedSubjecttoCP);
                        break;
                    case (int)invln_Loanapplication_StatusCode.LoanAvailable:
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.LoanAvailable);
                        break;
                    default:
                        break;
                }
            }
        }

        private invln_Loanapplication MapExternalStatusToInternal(OptionSetValue externalStatus)
        {
            var loanApplication = new invln_Loanapplication();
            switch (externalStatus.Value)
            {
                case (int)invln_ExternalStatus.Draft:
                    loanApplication.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.Draft);
                    loanApplication.StateCode = new OptionSetValue((int)invln_loanapplicationState.Active);
                    break;
                case (int)invln_ExternalStatus.ApplicationSubmitted:
                    loanApplication.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.ApplicationSubmitted);
                    loanApplication.StateCode = new OptionSetValue((int)invln_loanapplicationState.Active);
                    break;
                case (int)invln_ExternalStatus.NA:
                    loanApplication.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.Inactive);
                    loanApplication.StateCode = new OptionSetValue((int)invln_loanapplicationState.Inactive);
                    break;
                case (int)invln_ExternalStatus.ApplicationUnderReview:
                    loanApplication.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.ApplicationUnderReview);
                    loanApplication.StateCode = new OptionSetValue((int)invln_loanapplicationState.Active);
                    break;
                case (int)invln_ExternalStatus.HoldRequested:
                    loanApplication.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.HoldRequested);
                    loanApplication.StateCode = new OptionSetValue((int)invln_loanapplicationState.Active);
                    break;
                case (int)invln_ExternalStatus.Withdrawn:
                    loanApplication.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.Withdrawn);
                    loanApplication.StateCode = new OptionSetValue((int)invln_loanapplicationState.Inactive);
                    break;
                case (int)invln_ExternalStatus.CashflowRequested:
                    loanApplication.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.CashflowRequested);
                    loanApplication.StateCode = new OptionSetValue((int)invln_loanapplicationState.Active);
                    break;
                case (int)invln_ExternalStatus.CashflowUnderReview:
                    loanApplication.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.CashflowUnderReview);
                    loanApplication.StateCode = new OptionSetValue((int)invln_loanapplicationState.Active);
                    break;
                case (int)invln_ExternalStatus.OnHold:
                    loanApplication.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.OnHold);
                    loanApplication.StateCode = new OptionSetValue((int)invln_loanapplicationState.Active);
                    break;
                case (int)invln_ExternalStatus.ReferredBacktoApplicant:
                    loanApplication.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.ReferredBacktoApplicant);
                    loanApplication.StateCode = new OptionSetValue((int)invln_loanapplicationState.Active);
                    break;
                case (int)invln_ExternalStatus.UnderReview:
                    loanApplication.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.UnderReview);
                    loanApplication.StateCode = new OptionSetValue((int)invln_loanapplicationState.Active);
                    break;
                case (int)invln_ExternalStatus.SentforApproval:
                    loanApplication.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.SentforApproval);
                    loanApplication.StateCode = new OptionSetValue((int)invln_loanapplicationState.Active);
                    break;
                case (int)invln_ExternalStatus.ApprovedSubjecttoDueDiligence:
                    loanApplication.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.ApprovedSubjecttoDueDiligence);
                    loanApplication.StateCode = new OptionSetValue((int)invln_loanapplicationState.Active);
                    break;
                case (int)invln_ExternalStatus.ApplicationDeclined:
                    loanApplication.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.ApplicationDeclined);
                    loanApplication.StateCode = new OptionSetValue((int)invln_loanapplicationState.Active);
                    break;
                case (int)invln_ExternalStatus.InDueDiligence:
                    loanApplication.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.InDueDiligence);
                    loanApplication.StateCode = new OptionSetValue((int)invln_loanapplicationState.Active);
                    break;
                case (int)invln_ExternalStatus.ApprovedSubjecttoContract:
                    loanApplication.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.ApprovedSubjectToContract);
                    loanApplication.StateCode = new OptionSetValue((int)invln_loanapplicationState.Active);
                    break;
                case (int)invln_ExternalStatus.ContractSignedSubjecttoCP:
                    loanApplication.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.AwaitingCPSatisfaction);
                    loanApplication.StateCode = new OptionSetValue((int)invln_loanapplicationState.Active);
                    break;
                case (int)invln_ExternalStatus.LoanAvailable:
                    loanApplication.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.LoanAvailable);
                    loanApplication.StateCode = new OptionSetValue((int)invln_loanapplicationState.Active);
                    break;
                case (int)invln_ExternalStatus.CPsSatisfied:
                    loanApplication.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.CPsSatisfied);
                    loanApplication.StateCode = new OptionSetValue((int)invln_loanapplicationState.Active);
                    break;
                default:
                    break;
            }
            return loanApplication;
        }

        public void SendEmailToNewOwner(invln_Loanapplication target, invln_Loanapplication preImage)
        {
            if (target.OwnerId.Id != preImage.OwnerId.Id && target.OwnerId.LogicalName != Team.EntityLogicalName)
            {
                var subject = $"Application ref no {target.invln_Name ?? preImage.invln_Name} - Assigned to you";
                _govNotifyEmailService.SendNotifications_INTERNAL_LOAN_APP_OWNER_CHANGE(target, subject, target.invln_Name ?? preImage.invln_Name);
                var req1 = new invln_sendinternalcrmnotificationRequest()
                {
                    invln_notificationbody = "",
                    invln_notificationowner = target.OwnerId.Id.ToString(),
                    invln_notificationtitle = $"[Application ref no {target.invln_Name ?? preImage.invln_Name} - Assigned to you](?pagetype=entityrecord&etn=invln_loanapplication&id={target.Id})'",
                };
                _ = _loanApplicationRepositoryAdmin.ExecuteNotificatioRequest(req1);
            }
        }

        public string GetFileLocationForApplicationLoan(string loanApplicationId)
        {
            if (Guid.TryParse(loanApplicationId, out Guid loanGuid))
            {
                var relatedDocumentLocation = _sharepointDocumentLocationRepository.GetDocumentLocationRelatedToLoanApplication(loanGuid);
                return relatedDocumentLocation.RelativeUrl;
            }
            return string.Empty;
        }

        public void CreateDocumentLocation(invln_Loanapplication target)
        {
            var documentLocation = _sharepointDocumentLocationRepository.GetByAttribute(nameof(SharePointDocumentLocation.Name).ToLower(), "Loan Application documents").FirstOrDefault();
            var documentToCreate = new SharePointDocumentLocation()
            {
                RegardingObjectId = target.ToEntityReference(),
                Name = $"Documents on Loans",
                RelativeUrl = $"{target.invln_Name}",
                ParentSiteOrLocation = documentLocation.ToEntityReference(),
            };
            _ = _sharepointDocumentLocationRepository.Create(documentToCreate);
        }

        public void AssignLoanToTmTeam(invln_Loanapplication target)
        {
            var tmTeam = _teamRepositoryAdmin.GetTeamByName("TM Team");
            if (tmTeam != null)
            {
                this._loanApplicationRepositoryAdmin.Update(new invln_Loanapplication()
                {
                    Id = target.Id,
                    OwnerId = tmTeam.ToEntityReference()
                });
            }
        }

        public void SetLastModificationDate(invln_Loanapplication target)
        {
            target.invln_lastmmodificationdate = DateTime.UtcNow;
        }

        public bool CheckIfLoanApplicationWithGivenNameExists(string loanName, string organisationId)
        {
            if (Guid.TryParse(organisationId, out Guid organisationGuid))
            {
                return _loanApplicationRepository.LoanWithGivenNameExists(loanName, organisationGuid);
            }
            else
            {
                throw new InvalidPluginExecutionException("Organisation Guid is not valid");
            }
        }

        private bool CheckIfExternalStatusCanBeChanged(int oldStatus, int newStatus)
        {
            if (oldStatus != (int)invln_ExternalStatus.Draft)
            {
                if (newStatus == (int)invln_ExternalStatus.ApplicationSubmitted || newStatus == (int)invln_ExternalStatus.NA)
                {
                    throw new ArgumentException("You can change status to Submitted or Inactive only when previous status was Draft");
                }
            }
            if (oldStatus != (int)invln_ExternalStatus.ApplicationSubmitted)
            {
                if (newStatus == (int)invln_ExternalStatus.Withdrawn)
                {
                    throw new ArgumentException("You can change status to Withdrawn only when previous status was Submitted");
                }
            }

            return true;
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

        #endregion
    }
}
