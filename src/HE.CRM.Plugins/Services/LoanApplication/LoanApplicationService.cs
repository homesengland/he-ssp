using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.DtoMapping;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
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

        #endregion

        #region Constructors

        public LoanApplicationService(CrmServiceArgs args) : base(args)
        {
            loanApplicationRepository = CrmRepositoriesFactory.Get<ILoanApplicationRepository>();
            siteDetailsRepository = CrmRepositoriesFactory.Get<ISiteDetailsRepository>();
            accountRepository = CrmRepositoriesFactory.Get<IAccountRepository>();
            contactRepository = CrmRepositoriesFactory.Get<IContactRepository>();
        }

        #endregion

        #region Public Methods

        public string GetLoanApplicationsForAccountAndContact(string externalContactId, string accountId, string loanApplicationId = null)
        {
            List<LoanApplicationDto> entityCollection = new List<LoanApplicationDto>();
            if (Guid.TryParse(accountId, out Guid accountGuid))
            {
                var loanApplicationsForAccountAndContact = loanApplicationRepository.GetLoanApplicationsForGivenAccountAndContact(accountGuid, externalContactId, loanApplicationId);
                foreach (var element in loanApplicationsForAccountAndContact)
                {
                    List<SiteDetailsDto> siteDetailsDtoList = new List<SiteDetailsDto>();
                    var siteDetailsList = siteDetailsRepository.GetSiteDetailRelatedToLoanApplication(element.ToEntityReference());
                    if (siteDetailsList != null)
                    {
                        foreach (var siteDetail in siteDetailsList)
                        {
                            siteDetailsDtoList.Add(SiteDetailsDtoMapper.MapSiteDetailsToDto(siteDetail));
                        }
                    }
                    entityCollection.Add(LoanApplicationDtoMapper.MapLoanApplicationToDto(element, siteDetailsDtoList, externalContactId));
                }
            }
            return JsonSerializer.Serialize(entityCollection);
        }

        public string CreateRecordFromPortal(string contactExternalId, string accountId, string loanApplicationId, string loanApplicationPayload)
        {
            this.TracingService.Trace("PAYLOAD:" + loanApplicationPayload);
            LoanApplicationDto loanApplicationFromPortal = JsonSerializer.Deserialize<LoanApplicationDto>(loanApplicationPayload);
            Contact contact = contactRepository.GetContactViaExternalId(contactExternalId);

            int numberOfSites = 0;
            if (loanApplicationFromPortal.siteDetailsList != null && loanApplicationFromPortal.siteDetailsList.Count > 0)
            {
                numberOfSites = loanApplicationFromPortal.siteDetailsList.Count;
            }

            this.TracingService.Trace("Setting up invln_Loanapplication");
            loanApplicationFromPortal.numberOfSites = numberOfSites.ToString();
            var loanApplicationToCreate = LoanApplicationDtoMapper.MapLoanApplicationDtoToRegularEntity(loanApplicationFromPortal, contact, accountId);

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
                this.TracingService.Trace("Create invln_Loanapplication");
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
                Contact contact = contactRepository.GetContactViaExternalId(contactExternalId);
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
                            loanApplicationToUpdate[field] = loanApplicationMapped[field];
                        }
                    }
                }
                loanApplicationToUpdate.Id = applicationId;
                loanApplicationRepository.Update(loanApplicationToUpdate);
            }
        }
        #endregion
    }
}
