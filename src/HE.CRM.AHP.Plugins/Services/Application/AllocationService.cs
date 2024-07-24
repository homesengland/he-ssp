using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.IdentityModel.Claims;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Common.Extensions;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.DtoMapping;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Services.Application
{
    public class AllocationService : CrmService, IAllocationService
    {
        private readonly IAhpApplicationRepository _ahpApplicationRepository;
        private readonly IContactRepository _contactRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IContactWebroleRepository _contactWebroleRepository;
        private readonly IDeliveryPhaseRepository _deliveryPhaseRepository;
        private readonly IClaimRepository _claimRepository;
        private readonly IHeLocalAuthorityRepository _heLocalAuthorityRepository;


        public AllocationService(CrmServiceArgs args) : base(args)
        {
            _ahpApplicationRepository = CrmRepositoriesFactory.Get<IAhpApplicationRepository>();
            _contactRepository = CrmRepositoriesFactory.Get<IContactRepository>();
            _accountRepository = CrmRepositoriesFactory.Get<IAccountRepository>();
            _contactWebroleRepository = CrmRepositoriesFactory.Get<IContactWebroleRepository>();
            _deliveryPhaseRepository = CrmRepositoriesFactory.Get<IDeliveryPhaseRepository>();
            _claimRepository = CrmRepositoriesFactory.Get<IClaimRepository>();
            _heLocalAuthorityRepository = CrmRepositoriesFactory.Get<IHeLocalAuthorityRepository>();
        }


        public AllocationClaimsDto GetAllocationWithClaims(string externalContactId, Guid accountId, Guid allocationId)
        {
            TracingService.Trace("GetAllocationWithClaims");
            AllocationClaimsDto result = null;
            Contact contact = null;
            contact = _contactRepository.GetContactViaExternalId(externalContactId);

            TracingService.Trace("Starting WebRole check");
            var webroleList = _contactWebroleRepository.GetListOfUsersWithoutLimitedRole(accountId.ToString());
            var isOtherThanLimitedUser = webroleList.Exists(x => x.invln_Contactid.Id == contact.Id);
            var isLimitedUser = _contactWebroleRepository.IsContactHaveSelectedWebRoleForOrganisation(contact.Id, accountId, invln_Permission.Limiteduser);

            TracingService.Trace($"isOtherThanLimitedUser : {isOtherThanLimitedUser}  isLimitedUser : {isLimitedUser}  ");
            TracingService.Trace("WebRole checked");

            invln_scheme allocation = null;
            if (isLimitedUser == true && isOtherThanLimitedUser == false)
            {
                TracingService.Trace("Get allocation for isLimitedUser");
                allocation = _ahpApplicationRepository.GetAllocation(allocationId, accountId, contact);
            }
            if (isOtherThanLimitedUser == true)
            {
                TracingService.Trace("Get allocation for isOtherThanLimitedUser");
                allocation = _ahpApplicationRepository.GetAllocation(allocationId, accountId);
            }

            if (allocation == null)
            {
                TracingService.Trace("allocation == null");
                return result;
            }

            TracingService.Trace("Prepare listOfPhaseClaims");
            var listOfPhaseClaims = new List<PhaseClaimsDto>();

            //Get List of DeliveryPhase with Claims for Allocation
            var dataFromCrm = _ahpApplicationRepository.GetAllocationWithDeliveryPhaseAndClaims(externalContactId, accountId, allocationId, Guid.Empty).Entities;

            foreach (var recordData in dataFromCrm)
            {
                var deliveryPhaseId = recordData.GetAliasedAttributeValue<Guid>("DeliveryPhase", invln_DeliveryPhase.Fields.invln_DeliveryPhaseId);
                TracingService.Trace($"Delivery Phase ID : {deliveryPhaseId}");

                var claimAcquisitionId = recordData.GetAliasedAttributeValue<Guid>("ClaimAcquisition", invln_Claim.Fields.invln_ClaimId);
                var claimSoSId = recordData.GetAliasedAttributeValue<Guid>("ClaimSoS", invln_Claim.Fields.invln_ClaimId);
                var claimPCId = recordData.GetAliasedAttributeValue<Guid>("ClaimPC", invln_Claim.Fields.invln_ClaimId);

                if (claimAcquisitionId == Guid.Empty)
                {
                    TracingService.Trace("claimAcquisition is Null");
                }
                else
                {
                    TracingService.Trace($"claimAcquisition is {claimAcquisitionId}");
                }
                if (claimSoSId == Guid.Empty)
                {
                    TracingService.Trace("claimSoS is Null");
                }
                else
                {
                    TracingService.Trace($"claimSoSId is {claimSoSId}");
                }
                if (claimPCId == Guid.Empty)
                {
                    TracingService.Trace("claimPC is Null");
                }
                else
                {
                    TracingService.Trace($"claimPCId is {claimPCId}");
                }

                //Mapp Claim
                TracingService.Trace("Mapp to claimAcquisitionDto");
                var claimAcquisitionDto = ClaimMapper.MapToMilestoneClaimDto(recordData, (int)invln_Milestone.Acquisition, claimAcquisitionId);
                TracingService.Trace("Mapp to claimSoSDto");
                var claimSoSDto = ClaimMapper.MapToMilestoneClaimDto(recordData, (int)invln_Milestone.SoS, claimSoSId);
                TracingService.Trace("Mapp to claimPCDto");
                var claimPCDto = ClaimMapper.MapToMilestoneClaimDto(recordData, (int)invln_Milestone.PC, claimPCId);

                //Mapp delivery phase
                TracingService.Trace($"Mapp to PhaseClaimsDto : {deliveryPhaseId}");
                listOfPhaseClaims.Add(DeliveryPhaseMapper.MapToPhaseClaimsDto(recordData, claimAcquisitionDto, claimSoSDto, claimPCDto));
            }

            // Mapp Allocation
            TracingService.Trace("Mapp to AllocationClaimsDto");
            result = AhpApplicationMapper.MapToAllocationClaimsDto(allocation, listOfPhaseClaims, _heLocalAuthorityRepository.GetById(allocation.invln_HELocalAuthorityID.Id));

            TracingService.Trace("Return Result");
            return result;
        }

        public void SetAllocationPhase(string externalContactId, Guid accountId, Guid allocationId, Guid deliveryPhaseId, string phaseClaimsDtoData)
        {
            TracingService.Trace("SetAllocationPhase");

            TracingService.Trace("Deserialize phaseClaimsDto");
            var phaseClaimsDto = JsonSerializer.Deserialize<PhaseClaimsDto>(phaseClaimsDtoData);

            TracingService.Trace("Get data from Crm");
            var dataFromCrm = _ahpApplicationRepository.GetAllocationWithDeliveryPhaseAndClaims(externalContactId, accountId, allocationId, deliveryPhaseId).Entities.First();
            var deliveryPhaseName = dataFromCrm.GetAliasedAttributeValue<string>("DeliveryPhase", invln_DeliveryPhase.Fields.invln_phasename);


            // ClaimAcquisition
            var claimAcquisition_invln_ClaimId = dataFromCrm.GetAliasedAttributeValue<Guid>("ClaimAcquisition", invln_Claim.Fields.invln_ClaimId);
            var acquisitionMilestone = phaseClaimsDto.AcquisitionMilestone;
            if (acquisitionMilestone == null && claimAcquisition_invln_ClaimId != Guid.Empty)
            {
                _claimRepository.Delete(new invln_Claim()
                {
                    Id = claimAcquisition_invln_ClaimId
                });
            }
            else if (acquisitionMilestone != null)
            {
                CreateOrUpdateClaimForAllocationPhase(allocationId, deliveryPhaseId, acquisitionMilestone, "Acquisition", claimAcquisition_invln_ClaimId, deliveryPhaseName);
            }

            // ClaimStartOnSite
            var claimStartOnSite_invln_ClaimId = dataFromCrm.GetAliasedAttributeValue<Guid>("ClaimSoS", invln_Claim.Fields.invln_ClaimId);
            var startOnSiteMilestone = phaseClaimsDto.StartOnSiteMilestone;
            if (startOnSiteMilestone == null && claimStartOnSite_invln_ClaimId != Guid.Empty)
            {
                _claimRepository.Delete(new invln_Claim()
                {
                    Id = claimStartOnSite_invln_ClaimId
                });
            }
            else if (startOnSiteMilestone != null)
            {
                CreateOrUpdateClaimForAllocationPhase(allocationId, deliveryPhaseId, startOnSiteMilestone, "StartOnSite", claimStartOnSite_invln_ClaimId, deliveryPhaseName);
            }

            // ClaimCompletion
            var claimCompletion_invln_ClaimId = dataFromCrm.GetAliasedAttributeValue<Guid>("ClaimPC", invln_Claim.Fields.invln_ClaimId);
            var completionMilestone = phaseClaimsDto.CompletionMilestone;
            if (completionMilestone == null && claimCompletion_invln_ClaimId != Guid.Empty)
            {
                _claimRepository.Delete(new invln_Claim()
                {
                    Id = claimCompletion_invln_ClaimId
                });
            }
            else if (completionMilestone != null)
            {
                CreateOrUpdateClaimForAllocationPhase(allocationId, deliveryPhaseId, completionMilestone, "Completion", claimCompletion_invln_ClaimId, deliveryPhaseName);
            }
        }


        private void CreateOrUpdateClaimForAllocationPhase(Guid allocationId, Guid deliveryPhaseId, MilestoneClaimDto milestone, string typeOfMilestone, Guid claimIdInCrm, string deliveryPhaseName)
        {
            TracingService.Trace($"Create new object invln_Claim for {typeOfMilestone} Milestone");
            var newClaim = new invln_Claim()
            {
                invln_Name = deliveryPhaseName + " " + typeOfMilestone + " Claim",
                invln_Milestone = new OptionSetValue(milestone.Type),
                invln_AmountApportionedtoMilestone = new Money(milestone.AmountOfGrantApportioned),
                invln_PercentageofGrantApportionedtoThisMilestone = (double)milestone.PercentageOfGrantApportioned,
                invln_MilestoneDate = milestone.AchievementDate ?? null,

                invln_ClaimSubmissionDate = milestone.SubmissionDate ?? null,
                invln_IncurredCosts = milestone.CostIncurred ?? null,
                invln_RequirementsConfirmation = milestone.IsConfirmed ?? null,

                invln_Allocation = new EntityReference(invln_scheme.EntityLogicalName, allocationId),
                invln_DeliveryPhase = new EntityReference(invln_DeliveryPhase.EntityLogicalName, deliveryPhaseId)
            };

            TracingService.Trace($"Create or Update in CRM");
            if (claimIdInCrm == Guid.Empty)
            {
                // Create
                TracingService.Trace($"Create in CRM");
                newClaim.StatusCode = new OptionSetValue((int)invln_Claim_StatusCode.Draft);
                newClaim.invln_ExternalStatus = new OptionSetValue((int)invln_ClaimExternalStatus.Draft);
                _claimRepository.Create(newClaim);
            }
            else
            {
                // Update
                if (milestone.Status == (int)invln_ClaimExternalStatus.Submitted)
                {
                    newClaim.StatusCode = new OptionSetValue((int)invln_Claim_StatusCode.Submitted);
                    newClaim.invln_ExternalStatus = new OptionSetValue((int)invln_ClaimExternalStatus.Submitted);
                }
                TracingService.Trace($"Update in CRM");
                newClaim.Id = claimIdInCrm;
                _claimRepository.Update(newClaim);
            }
            TracingService.Trace($"Created or Updated in CRM");
        }
    }
}
