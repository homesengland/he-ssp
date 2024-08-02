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

        public void CalculateGrantDetails(Guid allocationId)
        {
            Logger.Trace($"{nameof(AllocationService)}.{nameof(CalculateGrantDetails)}, allocationId: {allocationId}");
            var onlyApprovedAllocation = true;

            var claimColumns = new[]
            {
                invln_Claim.Fields.invln_Milestone,
                invln_Claim.Fields.invln_AmountApportionedtoMilestone,
                invln_Claim.Fields.invln_Milestone,
            };
            var allocation = _ahpApplicationRepository.GetById(allocationId);
            var claims = _claimRepository.GetClaimsForAllocation(allocationId, onlyApprovedAllocation, claimColumns);
            Logger.Trace($"Found {claims.Count()} claims.");
            var totalGrantAllocated = allocation.invln_fundingrequired;
            var amountPaid = default(decimal);

            foreach (var claim in claims)
            {
                amountPaid += claim.invln_AmountApportionedtoMilestone?.Value ?? 0;
            }
            var amountRemaining = totalGrantAllocated.Value - amountPaid;

            _ahpApplicationRepository.Update(
                new invln_scheme()
                {
                    Id = allocation.Id,
                    invln_TotalGrantAllocated = totalGrantAllocated,
                    invln_AmountPaid = new Money(amountPaid),
                    invln_AmountRemaining = new Money(amountRemaining)
                });
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

            TracingService.Trace("Claim Acquisition");
            CreateOrUpdateOrDeleteClaimForAllocationPhase(allocationId, deliveryPhaseId, phaseClaimsDto, "Acquisition", dataFromCrm);

            TracingService.Trace("Claim StartOnSite");
            CreateOrUpdateOrDeleteClaimForAllocationPhase(allocationId, deliveryPhaseId, phaseClaimsDto, "StartOnSite", dataFromCrm);

            TracingService.Trace("Claim Completion"); 
            CreateOrUpdateOrDeleteClaimForAllocationPhase(allocationId, deliveryPhaseId, phaseClaimsDto, "Completion", dataFromCrm);
        }

        public AllocationDto GetAllocation(string externalContactId, Guid accountId, Guid allocationId)
        {
            TracingService.Trace("GetAllocation");
            AllocationDto result = null;
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

            TracingService.Trace($"Allocation data:");
            TracingService.Trace($"baseApplicationId : {allocation.invln_BaseApplication?.Id}");
            TracingService.Trace($"allocation Id : {allocation.invln_schemeId}");
            TracingService.Trace($"allocation PartnerId : {allocation.invln_organisationid?.Id}");
            TracingService.Trace($"allocation ProgrammeId : {allocation.invln_programmelookup?.Id}");


            TracingService.Trace("Get data from Crm");
            var dataFromCrm = _ahpApplicationRepository.GetAllocationForAllocationDto(allocation.invln_BaseApplication.Id, allocation.Id, allocation.invln_organisationid.Id, allocation.invln_programmelookup.Id).Entities;

            TracingService.Trace($"Was record found in CRM?  {dataFromCrm.Count > 0}");
            if (dataFromCrm.Count == 0)
            {
                return result;
            }

            var recordDataFromCrm = dataFromCrm.FirstOrDefault();

            // Mapp to AllocationDto
            TracingService.Trace("Mapp to AllocationClaimsDto");
            result = AhpApplicationMapper.MapToAllocationDto(recordDataFromCrm);

            TracingService.Trace("Return Result");
            return result;
        }


        private void CreateOrUpdateOrDeleteClaimForAllocationPhase(Guid allocationId, Guid deliveryPhaseId, PhaseClaimsDto phaseClaimsDto, string typeOfMilestone, Entity dataFromCrm)
        {
            var claimId = Guid.Empty;
            var milestone = new MilestoneClaimDto();
            decimal milestoneDpAmountOfGrantApportioned = 0;
            decimal? milestoneDpPercentageOfGrantApportioned = 0;

            switch (typeOfMilestone)
            {
                case "Acquisition":
                    claimId = dataFromCrm.GetAliasedAttributeValue<Guid>("ClaimAcquisition", invln_Claim.Fields.invln_ClaimId);
                    milestone = phaseClaimsDto.AcquisitionMilestone;
                    milestoneDpAmountOfGrantApportioned = dataFromCrm.GetAliasedAttributeValue<Money>("DeliveryPhase", invln_DeliveryPhase.Fields.invln_AcquisitionValue).Value;
                    milestoneDpPercentageOfGrantApportioned = dataFromCrm.GetAliasedAttributeValue<decimal?>("DeliveryPhase", invln_DeliveryPhase.Fields.invln_AcquisitionPercentageValue);
                    break;
                case "StartOnSite":
                    claimId = dataFromCrm.GetAliasedAttributeValue<Guid>("ClaimSoS", invln_Claim.Fields.invln_ClaimId);
                    milestone = phaseClaimsDto.StartOnSiteMilestone;
                    milestoneDpAmountOfGrantApportioned = dataFromCrm.GetAliasedAttributeValue<Money>("DeliveryPhase", invln_DeliveryPhase.Fields.invln_StartOnSiteValue).Value;
                    milestoneDpPercentageOfGrantApportioned = dataFromCrm.GetAliasedAttributeValue<decimal?>("DeliveryPhase", invln_DeliveryPhase.Fields.invln_StartOnSitePercentageValue);
                    break;
                case "Completion":
                    claimId = dataFromCrm.GetAliasedAttributeValue<Guid>("ClaimPC", invln_Claim.Fields.invln_ClaimId);
                    milestone = phaseClaimsDto.CompletionMilestone;
                    milestoneDpAmountOfGrantApportioned = dataFromCrm.GetAliasedAttributeValue<Money>("DeliveryPhase", invln_DeliveryPhase.Fields.invln_CompletionValue).Value;
                    milestoneDpPercentageOfGrantApportioned = dataFromCrm.GetAliasedAttributeValue<decimal?>("DeliveryPhase", invln_DeliveryPhase.Fields.invln_CompletionPercentageValue);
                    break;
                default:
                    break;
            }

            if (milestone == null && claimId != Guid.Empty)
            {
                TracingService.Trace($"Delete Claim for {typeOfMilestone} Milestone");
                _claimRepository.Delete(new invln_Claim()
                {
                    Id = claimId
                });
            }
            else if (milestone != null)
            {
                TracingService.Trace($"Create new object invln_Claim for {typeOfMilestone} Milestone");
                var newClaim = new invln_Claim()
                {
                    invln_Name = dataFromCrm.GetAliasedAttributeValue<string>("DeliveryPhase", invln_DeliveryPhase.Fields.invln_phasename) + " " + typeOfMilestone + " Claim",
                    invln_Milestone = new OptionSetValue(milestone.Type),
                    invln_AmountApportionedtoMilestone = new Money(milestoneDpAmountOfGrantApportioned),
                    invln_PercentageofGrantApportionedtoThisMilestone = (double)milestoneDpPercentageOfGrantApportioned,
                    invln_MilestoneDate = milestone.AchievementDate ?? null,

                    invln_ClaimSubmissionDate = milestone.SubmissionDate ?? null,
                    invln_IncurredCosts = milestone.CostIncurred ?? null,
                    invln_RequirementsConfirmation = milestone.IsConfirmed ?? null,

                    invln_Allocation = new EntityReference(invln_scheme.EntityLogicalName, allocationId),
                    invln_DeliveryPhase = new EntityReference(invln_DeliveryPhase.EntityLogicalName, deliveryPhaseId)
                };

                TracingService.Trace($"Exception check for Acquisition Milestone");
                if (typeOfMilestone == "Acquisition")
                {
                    if (phaseClaimsDto.AcquisitionMilestone.CostIncurred == false)
                    {
                        newClaim.invln_AmountApportionedtoMilestone = new Money(0);
                        newClaim.invln_PercentageofGrantApportionedtoThisMilestone = 0;
                    }
                }

                TracingService.Trace($"Exception check for StartOnSite Milestone");
                if (typeOfMilestone == "StartOnSite" && phaseClaimsDto.AcquisitionMilestone != null)
                {
                    if (phaseClaimsDto.AcquisitionMilestone.CostIncurred.Value == false)
                    {
                        TracingService.Trace($"AcquisitionMilestone.CostIncurred is false. Amount and Percentage will be recalculated.");
                        newClaim.invln_AmountApportionedtoMilestone = new Money(milestoneDpAmountOfGrantApportioned + dataFromCrm.GetAliasedAttributeValue<Money>("DeliveryPhase", invln_DeliveryPhase.Fields.invln_AcquisitionValue).Value);

                        var sumofcalculatedfounds = (double)dataFromCrm.GetAliasedAttributeValue<Money>("DeliveryPhase", invln_DeliveryPhase.Fields.invln_sumofcalculatedfounds).Value;
                        newClaim.invln_PercentageofGrantApportionedtoThisMilestone = (double)(newClaim.invln_AmountApportionedtoMilestone.Value * 100) / sumofcalculatedfounds;
                    }
                }

                TracingService.Trace($"Create or Update in CRM");
                if (claimId == Guid.Empty)
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
                        TracingService.Trace($"Change status of claim to Submitted");
                        newClaim.StatusCode = new OptionSetValue((int)invln_Claim_StatusCode.Submitted);
                        newClaim.invln_ExternalStatus = new OptionSetValue((int)invln_ClaimExternalStatus.Submitted);
                    }
                    newClaim.Id = claimId;
                    TracingService.Trace($"Update in CRM");
                    _claimRepository.Update(newClaim);
                }
                TracingService.Trace($"End of Created or Updated in CRM");
            }
        }
    }
}
