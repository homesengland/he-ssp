using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.DtoMapping;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text.Json;

namespace HE.CRM.Plugins.Services.LoanApplication
{
    public class LoanApplicationService : CrmService, ILoanApplicationService
    {
        #region Fields

        private readonly ILoanApplicationRepository loanApplicationRepository;
        private readonly ISiteDetailsRepository siteDetailsRepository;
        private readonly IAccountRepository accountRepository;
        private readonly IContactRepository contactRepository;
        private readonly IWebRoleRepository webroleRepository;

        #endregion

        #region Constructors

        public LoanApplicationService(CrmServiceArgs args) : base(args)
        {
            loanApplicationRepository = CrmRepositoriesFactory.Get<ILoanApplicationRepository>();
            siteDetailsRepository = CrmRepositoriesFactory.Get<ISiteDetailsRepository>();
            accountRepository = CrmRepositoriesFactory.Get<IAccountRepository>();
            contactRepository = CrmRepositoriesFactory.Get<IContactRepository>();
            webroleRepository = CrmRepositoriesFactory.Get<IWebRoleRepository>();
        }

        #endregion

        #region Public Methods

        public string GetLoanApplicationsForAccountAndContact(string externalContactId, string accountId, string loanApplicationId = null)
        {
            List<LoanApplicationDto> entityCollection = new List<LoanApplicationDto>();
            if (Guid.TryParse(accountId, out Guid accountGuid))
            {
                var contact = contactRepository.GetContactViaExternalId(externalContactId);
                var role = webroleRepository.GetContactWebRole(contact.Id, ((int)invln_Portal1.Loans).ToString());
                List<invln_Loanapplication> loanApplicationsForAccountAndContact;
                if (role.Any(x => x.Contains("pl.invln_permission") && ((OptionSetValue)((AliasedValue)x["pl.invln_permission"]).Value).Value == (int)invln_Permission.Accountadministrator) && loanApplicationId == null)
                {
                    TracingService.Trace("admin");
                    loanApplicationsForAccountAndContact = loanApplicationRepository.GetAccountLoans(accountGuid);
                }
                else
                {
                    TracingService.Trace("regular user, not admin");
                    loanApplicationsForAccountAndContact = loanApplicationRepository.GetLoanApplicationsForGivenAccountAndContact(accountGuid, externalContactId, loanApplicationId);
                }
                this.TracingService.Trace("GetLoanApplicationsForGivenAccountAndContact");
                this.TracingService.Trace($"{loanApplicationsForAccountAndContact.Count}");
                foreach (var element in loanApplicationsForAccountAndContact)
                {
                    List<SiteDetailsDto> siteDetailsDtoList = new List<SiteDetailsDto>();
                    this.TracingService.Trace("GetSiteDetailRelatedToLoanApplication");
                    var siteDetailsList = siteDetailsRepository.GetSiteDetailRelatedToLoanApplication(element.ToEntityReference());
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
                        loanApplicationContact = this.contactRepository.GetById(element.invln_Contact.Id, new string[]
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
                loanApplicationRepository.Update(new invln_Loanapplication()
                {
                    Id = target.Id,
                    invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.Underreview),
                    StatusCode = new OptionSetValue((int)invln_Loanapplication_StatusCode.Underreview)
                });
            }
        }

        public string CreateRecordFromPortal(string contactExternalId, string accountId, string loanApplicationId, string loanApplicationPayload)
        {
            this.TracingService.Trace("PAYLOAD:" + loanApplicationPayload);

            LoanApplicationDto loanApplicationFromPortal = JsonSerializer.Deserialize<LoanApplicationDto>(loanApplicationPayload);
            //THIS IS CONTACT WHO IS SENDING MESSAGE
            var requestContact = contactRepository.GetContactViaExternalId(contactExternalId);

            //Update Contact on Loan Application
            Contact loanApplicationContact = null;
            if (loanApplicationFromPortal?.LoanApplicationContact?.ContactExternalId != null)
            {
                //THIS IS CONTACT FOR WHICH LOAN IS CREATED
                var contactExternalid = loanApplicationFromPortal?.LoanApplicationContact?.ContactExternalId ?? contactExternalId;
                loanApplicationContact = contactRepository.GetContactViaExternalId(contactExternalid);
                contactRepository.Update(new Contact()
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
                loanApplicationRepository.Update(loanApplicationToCreate);
            }
            else
            {
                if (loanApplicationToCreate.invln_Contact == null)
                {
                    loanApplicationToCreate.invln_Contact = requestContact.ToEntityReference();
                }

                this.TracingService.Trace("Create invln_Loanapplication");
                loanApplicationToCreate.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.Draft);
                loanApplicationGuid = loanApplicationRepository.Create(loanApplicationToCreate);
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
                        siteDetailsRepository.Update(siteDetailToCreate);
                    }
                    else
                    {
                        siteDetailsRepository.Create(siteDetailToCreate);
                    }
                    this.TracingService.Trace("after create record");
                }
            }

            return loanApplicationGuid.ToString();
        }

        public void ChangeLoanApplicationExternalStatus(int externalStatus, string loanApplicationId)
        {
            TracingService.Trace($"loan id {loanApplicationId}");
            TracingService.Trace($"new external status {externalStatus}");
            if (Guid.TryParse(loanApplicationId, out Guid loanId) && externalStatus != null)
            {

                var retrievedLoanApplicationStatus = loanApplicationRepository.GetById(loanId, new string[] { nameof(invln_Loanapplication.invln_ExternalStatus).ToLower() }).invln_ExternalStatus;

                int oldStatus = retrievedLoanApplicationStatus != null ? retrievedLoanApplicationStatus.Value : 0;
                CheckIfExternalStatusCanBeChanged(oldStatus, externalStatus);
                invln_Loanapplication loanToUpdate = new invln_Loanapplication()
                {
                    Id = loanId,
                    invln_ExternalStatus = new OptionSetValue(externalStatus),
                };
                TracingService.Trace("update loan application");
                loanApplicationRepository.Update(loanToUpdate);
            }
        }


        public void UpdateLoanApplication(string loanApplicationId, string loanApplication, string fieldsToUpdate, string accountId, string contactExternalId)
        {
            if (Guid.TryParse(loanApplicationId, out Guid applicationId))
            {
                var deserializedLoanApplication = JsonSerializer.Deserialize<LoanApplicationDto>(loanApplication);
                var contactExternalid = deserializedLoanApplication?.LoanApplicationContact?.ContactExternalId ?? contactExternalId;
                Contact contact = contactRepository.GetContactViaExternalId(contactExternalid);
                var loanApplicationMapped = LoanApplicationDtoMapper.MapLoanApplicationDtoToRegularEntity(deserializedLoanApplication, contact, accountId);
                invln_Loanapplication loanApplicationToUpdate = new invln_Loanapplication();
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
                                var retrievedLoanApplicationStatus = loanApplicationRepository.GetById(applicationId, new string[] { nameof(invln_Loanapplication.invln_ExternalStatus).ToLower() }).invln_ExternalStatus;
                                int oldStatus = retrievedLoanApplicationStatus != null ? retrievedLoanApplicationStatus.Value : 0;
                                CheckIfExternalStatusCanBeChanged(oldStatus, loanApplicationMapped.invln_ExternalStatus.Value);
                            }
                            loanApplicationToUpdate[field] = loanApplicationMapped[field];
                        }
                    }
                }
                loanApplicationToUpdate.Id = applicationId;
                loanApplicationRepository.Update(loanApplicationToUpdate);
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
                loanApplicationRepository.Update(loanApplicationToUpdate);

                var relatedSiteDetails = siteDetailsRepository.GetSiteDetailRelatedToLoanApplication(loanApplicationToUpdate.ToEntityReference());
                if (relatedSiteDetails != null && relatedSiteDetails.Count > 0)
                {
                    foreach (var siteDetail in relatedSiteDetails)
                    {
                        var siteDetailToUpdate = new invln_SiteDetails()
                        {
                            Id = siteDetail.Id,
                            StateCode = new OptionSetValue(1),
                        };
                        siteDetailsRepository.Update(siteDetailToUpdate);
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
                var relatedSiteDetails = siteDetailsRepository.GetSiteDetailRelatedToLoanApplication(target.ToEntityReference());
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
                    target.invln_Noofhomes = numberOfHomes.ToString();
                }
            }
        }
        public void SendInternalNotificationOnStatusChange(invln_Loanapplication target, invln_Loanapplication preImage)
        {
            var statusLabel = string.Empty;
            switch (target.StatusCode.Value)
            {
                case (int)invln_Loanapplication_StatusCode.Draft:
                    statusLabel = "Draft";
                    target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.Draft);
                    break;
                case (int)invln_Loanapplication_StatusCode.ApplicationSubmitted:
                    statusLabel = "Application submitted";
                    target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.ApplicationSubmitted);
                    break;
                case (int)invln_Loanapplication_StatusCode.Inactive_Active:
                    statusLabel = "Inactive";
                    target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.NA);
                    break;
                case (int)invln_Loanapplication_StatusCode.ApplicationunderReview:
                    statusLabel = "Application under review";
                    target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.ApplicationunderReview);
                    break;
                case (int)invln_Loanapplication_StatusCode.HoldRequested:
                    statusLabel = "Hold requested";
                    target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.HoldRequested);
                    break;
                case (int)invln_Loanapplication_StatusCode.Withdrawn:
                    statusLabel = "Withdrawn";
                    target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.Withdrawn);
                    break;
                case (int)invln_Loanapplication_StatusCode.Cashflowrequested:
                    statusLabel = "Cashflow requested";
                    target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.Cashflowrequested);
                    break;
                case (int)invln_Loanapplication_StatusCode.Cashflowunderreview:
                    statusLabel = "Cashflow under review";
                    target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.Cashflowunderreview);
                    break;
                case (int)invln_Loanapplication_StatusCode.OnHold:
                    statusLabel = "On hold";
                    target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.OnHold);
                    break;
                case (int)invln_Loanapplication_StatusCode.ReferredBacktoApplicant:
                    statusLabel = "Reffered back to applicant";
                    target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.ReferredBacktoApplicant);
                    break;
                case (int)invln_Loanapplication_StatusCode.Underreview:
                    statusLabel = "Under review";
                    target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.Underreview);
                    break;
                case (int)invln_Loanapplication_StatusCode.SentforApproval:
                    statusLabel = "Sent for approval";
                    target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.Sentforapproval);
                    break;
                case (int)invln_Loanapplication_StatusCode.NotApproved:
                    statusLabel = "Not approved";
                    target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.Sentforapproval);
                    break;
                case (int)invln_Loanapplication_StatusCode.Approvedsubjecttoduediligence:
                    statusLabel = "Approved subject to due diligence";
                    target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.Approvedsubjecttoduediligence);
                    break;
                case (int)invln_Loanapplication_StatusCode.ApplicationDeclined:
                    statusLabel = "Application declined";
                    target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.ApplicationDeclined);
                    break;
                case (int)invln_Loanapplication_StatusCode.Induediligence:
                    statusLabel = "In due diligence";
                    target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.InDueDiligence);
                    break;
                case (int)invln_Loanapplication_StatusCode.SentforPreCompleteApproval:
                    statusLabel = "Sent for pre complete approval";
                    target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.InDueDiligence);
                    break;
                case (int)invln_Loanapplication_StatusCode.ApprovedSubjectToContract:
                    statusLabel = "Approved subject to contract";
                    target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.Approvedsubjecttocontract);
                    break;
                case (int)invln_Loanapplication_StatusCode.AwaitingCPSatisfaction:
                    statusLabel = "Awaiting CP satisfaction";
                    target.invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.ContractSignedSubjecttoCP);
                    break;
                case (int)invln_Loanapplication_StatusCode.LoanAvailable:
                    statusLabel = "Loan available";
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
                invln_loanapplicationid = target.Id.ToString(),
            };
            _ = loanApplicationRepository.ExecuteNotificatioRequest(req1);
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
