using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.DtoMapping;
using HE.CRM.Common.Repositories.Interfaces;
using HE.CRM.Model.CrmSerializedParameters;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace HE.CRM.Plugins.Services.LoanApplication
{
    public class LoanApplicationService : CrmService, ILoanApplicationService
    {
        #region Fields

        private readonly ILoanApplicationRepository _loanApplicationRepository;
        private readonly ISiteDetailsRepository _siteDetailsRepository;
        private readonly IContactRepository _contactRepository;
        private readonly IWebRoleRepository _webroleRepository;

        private readonly ILoanApplicationRepository _loanApplicationRepositoryAdmin;
        private readonly INotificationSettingRepository _notificationSettingRepositoryAdmin;
        private readonly IGovNotifyEmailRepository _govNotifyEmailRepositoryAdmin;
        private readonly IEnvironmentVariableRepository _environmentVariableRepositoryAdmin;
        private readonly ISystemUserRepository _systemUserRepositoryAdmin;

        #endregion

        #region Constructors

        public LoanApplicationService(CrmServiceArgs args) : base(args)
        {
            _loanApplicationRepository = CrmRepositoriesFactory.Get<ILoanApplicationRepository>();
            _siteDetailsRepository = CrmRepositoriesFactory.Get<ISiteDetailsRepository>();
            _contactRepository = CrmRepositoriesFactory.Get<IContactRepository>();
            _webroleRepository = CrmRepositoriesFactory.Get<IWebRoleRepository>();

            _loanApplicationRepositoryAdmin = CrmRepositoriesFactory.GetSystem<ILoanApplicationRepository>();
            _notificationSettingRepositoryAdmin = CrmRepositoriesFactory.GetSystem<INotificationSettingRepository>();
            _govNotifyEmailRepositoryAdmin = CrmRepositoriesFactory.GetSystem<IGovNotifyEmailRepository>();
            _environmentVariableRepositoryAdmin = CrmRepositoriesFactory.GetSystem<IEnvironmentVariableRepository>();
            _systemUserRepositoryAdmin = CrmRepositoriesFactory.GetSystem<ISystemUserRepository>();
        }

        #endregion

        #region Public Methods

        public string GetLoanApplicationsForAccountAndContact(string externalContactId, string accountId, string loanApplicationId = null)
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
                    loanApplicationsForAccountAndContact = _loanApplicationRepository.GetLoanApplicationsForGivenAccountAndContact(accountGuid, externalContactId, loanApplicationId);
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
                    invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.UnderReview),
                    StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.UnderReview)
                });
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

                var retrievedLoanApplicationStatus = _loanApplicationRepository.GetById(loanId, new string[] { nameof(invln_Loanapplication.invln_ExternalStatus).ToLower() }).invln_ExternalStatus;

                int oldStatus = retrievedLoanApplicationStatus != null ? retrievedLoanApplicationStatus.Value : 0;
                CheckIfExternalStatusCanBeChanged(oldStatus, externalStatus);
                var loanToUpdate = new invln_Loanapplication()
                {
                    Id = loanId,
                    invln_ExternalStatus = new OptionSetValue(externalStatus),
                };
                if (!string.IsNullOrEmpty(changeReason))
                {
                    loanToUpdate.invln_statuschangereason = changeReason;
                }
                TracingService.Trace("update loan application");
                _loanApplicationRepository.Update(loanToUpdate);
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
                    StateCode = new OptionSetValue(1)
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
            if (((preImage.StatusCode.Value == (int)invln_Loanapplication_StatusCode.Draft && target.StatusCode == null) || (target.StatusCode != null && target.StatusCode.Value == (int)invln_Loanapplication_StatusCode.Draft)) && target.OwnerId.Id != preImage.OwnerId.Id)
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
                    foreach (var siteDetail in relatedSiteDetails)
                    {
                        projectName += $"{siteDetail.invln_Name}, ";
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
                var statusLabel = string.Empty;
                var pastFormStatus = string.Empty;
                switch (target.StatusCode.Value)
                {
                    case (int)invln_Loanapplication_StatusCode.Draft:
                        statusLabel = "Draft";
                        pastFormStatus = "changed to " + statusLabel; //TODO: to update
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.Draft);
                        break;
                    case (int)invln_Loanapplication_StatusCode.ApplicationSubmitted:
                        statusLabel = "Application submitted";
                        pastFormStatus = "changed to " + statusLabel; //TODO: to update
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.ApplicationSubmitted);
                        break;
                    case (int)invln_Loanapplication_StatusCode.Inactive:
                        statusLabel = "Inactive";
                        pastFormStatus = "changed to " + statusLabel; //TODO: to update
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.NA);
                        break;
                    case (int)invln_Loanapplication_StatusCode.ApplicationUnderReview:
                        statusLabel = "Application under review";
                        pastFormStatus = "changed to " + statusLabel; //TODO: to update
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.ApplicationUnderReview);
                        break;
                    case (int)invln_Loanapplication_StatusCode.HoldRequested:
                        statusLabel = "Hold requested";
                        pastFormStatus = "requested to be put on hold.";
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.HoldRequested);
                        break;
                    case (int)invln_Loanapplication_StatusCode.Withdrawn:
                        statusLabel = "Withdrawn";
                        pastFormStatus = "withdrawn";
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.Withdrawn);
                        break;
                    case (int)invln_Loanapplication_StatusCode.CashflowRequested:
                        statusLabel = "Cashflow requested";
                        pastFormStatus = "changed to " + statusLabel; //TODO: to update
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.CashflowRequested);
                        break;
                    case (int)invln_Loanapplication_StatusCode.CashflowUnderReview:
                        statusLabel = "Cashflow under review";
                        pastFormStatus = "changed to " + statusLabel; //TODO: to update
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.CashflowUnderReview);
                        break;
                    case (int)invln_Loanapplication_StatusCode.OnHold:
                        statusLabel = "On hold";
                        pastFormStatus = "changed to " + statusLabel; //TODO: to update
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.OnHold);
                        break;
                    case (int)invln_Loanapplication_StatusCode.ReferredBacktoApplicant:
                        statusLabel = "Reffered back to applicant";
                        pastFormStatus = "changed to " + statusLabel; //TODO: to update
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.ReferredBacktoApplicant);
                        break;
                    case (int)invln_Loanapplication_StatusCode.UnderReview:
                        statusLabel = "Under review";
                        pastFormStatus = "changed to " + statusLabel; //TODO: to update
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.UnderReview);
                        break;
                    case (int)invln_Loanapplication_StatusCode.SentforApproval:
                        statusLabel = "Sent for approval";
                        pastFormStatus = "changed to " + statusLabel; //TODO: to update
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.SentforApproval);
                        break;
                    case (int)invln_Loanapplication_StatusCode.NotApproved:
                        statusLabel = "Not Approved";
                        pastFormStatus = "been not approved and has been returned.";
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.SentforApproval);
                        break;
                    case (int)invln_Loanapplication_StatusCode.ApprovedSubjecttoDueDiligence:
                        statusLabel = "Approved subject to due diligence";
                        pastFormStatus = "changed to " + statusLabel; //TODO: to update
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.ApprovedSubjecttoDueDiligence);
                        break;
                    case (int)invln_Loanapplication_StatusCode.ApplicationDeclined:
                        statusLabel = "Application declined";
                        pastFormStatus = "changed to " + statusLabel; //TODO: to update
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.ApplicationDeclined);
                        break;
                    case (int)invln_Loanapplication_StatusCode.InDueDiligence:
                        statusLabel = "In due diligence";
                        pastFormStatus = "changed to " + statusLabel; //TODO: to update
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.InDueDiligence);
                        break;
                    case (int)invln_Loanapplication_StatusCode.SentforPreCompleteApproval:
                        statusLabel = "Sent for pre complete approval";
                        pastFormStatus = "changed to " + statusLabel; //TODO: to update
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.InDueDiligence);
                        break;
                    case (int)invln_Loanapplication_StatusCode.ApprovedSubjectToContract:
                        statusLabel = "Approved Subject to Contract";
                        pastFormStatus = "approved subject to contract.";
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.ApprovedSubjecttoContract);
                        break;
                    case (int)invln_Loanapplication_StatusCode.CPsSatisfied:
                        statusLabel = "CPs Satisfied";
                        pastFormStatus = "changed to " + statusLabel; //TODO: to update
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.CPsSatisfied);
                        break;
                    case (int)invln_Loanapplication_StatusCode.AwaitingCPSatisfaction:
                        statusLabel = "Awaiting CP satisfaction";
                        pastFormStatus = "changed to " + statusLabel; //TODO: to update
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.ContractSignedSubjecttoCP);
                        break;
                    case (int)invln_Loanapplication_StatusCode.LoanAvailable:
                        statusLabel = "Loan available";
                        pastFormStatus = "changed to " + statusLabel; //TODO: to update
                        target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.LoanAvailable);
                        break;
                    default:
                        break;
                }

                var req1 = new invln_sendinternalcrmnotificationRequest()
                {
                    invln_notificationbody = $"[Application ref no {target.invln_Name ?? preImage.invln_Name} - Status change to '{statusLabel}](?pagetype=entityrecord&etn=invln_loanapplication&id={target.Id})'",
                    invln_notificationowner = target.OwnerId == null ? preImage.OwnerId.Id.ToString() : target.OwnerId.Id.ToString(),
                    invln_notificationtitle = "Information",
                    invln_emailid = "email.todelete@wp.pl",// TODO: delete this parameter
                };
                _ = _loanApplicationRepositoryAdmin.ExecuteNotificatioRequest(req1);

                var emailTemplate = _notificationSettingRepositoryAdmin.GetTemplateViaTypeName("INTERNAL_LOAN_APP_STATUS_CHANGE");
                var emailToCreate = new invln_govnotifyemail()
                {
                    OwnerId = target.OwnerId ?? preImage.OwnerId,
                    RegardingObjectId = target.ToEntityReference(),
                    StatusCode = new OptionSetValue((int)invln_govnotifyemail_StatusCode.Draft),
                    invln_notificationsettingid = emailTemplate?.ToEntityReference(),
                };
                var emailId = _govNotifyEmailRepositoryAdmin.Create(emailToCreate);

                if (emailTemplate != null)
                {
                    var orgUrl = _environmentVariableRepositoryAdmin.GetEnvironmentVariableValue("invln_environmenturl") ?? "";
                    var ownerData = _systemUserRepositoryAdmin.GetById(emailToCreate.OwnerId.Id, nameof(SystemUser.InternalEMailAddress).ToLower(), nameof(SystemUser.FullName).ToLower());
                    var subject = $"Application ref no {target.invln_Name ?? preImage.invln_Name} - Status change to '{statusLabel}";
                    var govNotParams = new INTERNAL_LOAN_APP_STATUS_CHANGE()
                    {
                        templateId = emailTemplate?.invln_templateid,
                        personalisation = new parameters()
                        {
                            recipientEmail = ownerData.InternalEMailAddress,
                            username = ownerData.FullName,
                            applicationId = preImage.invln_Name,
                            applicationUrl = orgUrl + "/main.aspx?appid=2576a100-db47-ee11-be6f-002248c653e1&pagetype=entityrecord&etn=invln_loanapplication&id=" + target.Id,
                            subject = subject,
                            statusAtBody = pastFormStatus
                        }
                    };

                    var options = new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                        WriteIndented = true
                    };
                    // TODO: delete after MVP when update possible from gov notify returned data
                    _govNotifyEmailRepositoryAdmin.Update(new invln_govnotifyemail()
                    {
                        Id = emailId,
                        Subject = subject,
                        invln_body = JsonSerializer.Serialize(govNotParams, options)
                    });
                    //
                    var govNotReq = new invln_sendgovnotifyemailRequest()
                    {
                        invln_emailid = emailId.ToString(),
                        invln_govnotifyparameters = JsonSerializer.Serialize(govNotParams, options),
                    };
                    _ = _loanApplicationRepositoryAdmin.ExecuteGovNotifyNotificationRequest(govNotReq);
                }
            }
        }

        public void ChangeInternalStatusOnExternalStatusChange(invln_Loanapplication target, invln_Loanapplication preImage)
        {
            if (target.invln_ExternalStatus.Value != preImage.invln_ExternalStatus.Value)
            {
                TracingService.Trace($"new status {target.invln_ExternalStatus.Value}");
                switch (target.invln_ExternalStatus.Value)
                {
                    case (int)invln_ExternalStatus.Draft:
                        target.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.Draft);
                        target.StateCode = new OptionSetValue((int)invln_loanapplicationState.Active);
                        break;
                    case (int)invln_ExternalStatus.ApplicationSubmitted:
                        target.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.ApplicationSubmitted);
                        target.StateCode = new OptionSetValue((int)invln_loanapplicationState.Active);
                        break;
                    case (int)invln_ExternalStatus.NA:
                        target.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.Inactive);
                        target.StateCode = new OptionSetValue((int)invln_loanapplicationState.Inactive);
                        break;
                    case (int)invln_ExternalStatus.ApplicationUnderReview:
                        target.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.ApplicationUnderReview);
                        target.StateCode = new OptionSetValue((int)invln_loanapplicationState.Active);
                        break;
                    case (int)invln_ExternalStatus.HoldRequested:
                        target.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.HoldRequested);
                        target.StateCode = new OptionSetValue((int)invln_loanapplicationState.Active);
                        break;
                    case (int)invln_ExternalStatus.Withdrawn:
                        target.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.Withdrawn);
                        target.StateCode = new OptionSetValue((int)invln_loanapplicationState.Inactive);
                        break;
                    case (int)invln_ExternalStatus.CashflowRequested:
                        target.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.CashflowRequested);
                        target.StateCode = new OptionSetValue((int)invln_loanapplicationState.Active);
                        break;
                    case (int)invln_ExternalStatus.CashflowUnderReview:
                        target.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.CashflowUnderReview);
                        target.StateCode = new OptionSetValue((int)invln_loanapplicationState.Active);
                        break;
                    case (int)invln_ExternalStatus.OnHold:
                        target.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.OnHold);
                        target.StateCode = new OptionSetValue((int)invln_loanapplicationState.Active);
                        break;
                    case (int)invln_ExternalStatus.ReferredBacktoApplicant:
                        target.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.ReferredBacktoApplicant);
                        target.StateCode = new OptionSetValue((int)invln_loanapplicationState.Active);
                        break;
                    case (int)invln_ExternalStatus.UnderReview:
                        target.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.UnderReview);
                        target.StateCode = new OptionSetValue((int)invln_loanapplicationState.Active);
                        break;
                    case (int)invln_ExternalStatus.SentforApproval:
                        target.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.SentforApproval);
                        target.StateCode = new OptionSetValue((int)invln_loanapplicationState.Active);
                        break;
                    case (int)invln_ExternalStatus.ApprovedSubjecttoDueDiligence:
                        target.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.ApprovedSubjecttoDueDiligence);
                        target.StateCode = new OptionSetValue((int)invln_loanapplicationState.Active);
                        break;
                    case (int)invln_ExternalStatus.ApplicationDeclined:
                        target.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.ApplicationDeclined);
                        target.StateCode = new OptionSetValue((int)invln_loanapplicationState.Active);
                        break;
                    case (int)invln_ExternalStatus.InDueDiligence:
                        target.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.InDueDiligence);
                        target.StateCode = new OptionSetValue((int)invln_loanapplicationState.Active);
                        break;
                    case (int)invln_ExternalStatus.ApprovedSubjecttoContract:
                        target.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.ApprovedSubjectToContract);
                        target.StateCode = new OptionSetValue((int)invln_loanapplicationState.Active);
                        break;
                    case (int)invln_ExternalStatus.ContractSignedSubjecttoCP:
                        target.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.AwaitingCPSatisfaction);
                        target.StateCode = new OptionSetValue((int)invln_loanapplicationState.Active);
                        break;
                    case (int)invln_ExternalStatus.LoanAvailable:
                        target.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.LoanAvailable);
                        target.StateCode = new OptionSetValue((int)invln_loanapplicationState.Active);
                        break;
                    case (int)invln_ExternalStatus.CPsSatisfied:
                        target.StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.CPsSatisfied);
                        target.StateCode = new OptionSetValue((int)invln_loanapplicationState.Active);
                        break;
                    default:
                        break;
                }
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

        #endregion
    }
}
