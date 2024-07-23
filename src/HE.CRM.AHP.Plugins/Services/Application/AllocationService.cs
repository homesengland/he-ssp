using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
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

            // Pobieranie listy Delivery Phase dla Allocation
            TracingService.Trace("Get list of DeliveryPhase");
            var listOfDeliveryPhase = _deliveryPhaseRepository.GetDeliveryPhasesForAllocation(allocation.Id);

            foreach ( var deliveryPhase in listOfDeliveryPhase)
            {
                //Get Claims for DeliveryPhase
                TracingService.Trace("Get claimAcquisition");
                var claimAcquisition = _claimRepository.GetClaimForAllocationDeliveryPhase(deliveryPhase.Id, (int)invln_Milestone.Acquisition);
                TracingService.Trace("Get claimSoS");
                var claimSoS = _claimRepository.GetClaimForAllocationDeliveryPhase(deliveryPhase.Id, (int)invln_Milestone.SoS);
                TracingService.Trace("Get claimPC");
                var claimPC = _claimRepository.GetClaimForAllocationDeliveryPhase(deliveryPhase.Id, (int)invln_Milestone.PC);

                if (claimAcquisition == null)
                {
                    TracingService.Trace("claimAcquisition is Null");
                }
                if (claimSoS == null)
                {
                    TracingService.Trace("claimSoS is Null");
                }
                if (claimPC == null)
                {
                    TracingService.Trace("claimPC is Null");
                }


                //Mapp Claim
                TracingService.Trace("Mapp to claimAcquisitionDto");
                var claimAcquisitionDto = ClaimMapper.MapToMilestoneClaimDto(deliveryPhase, (int)invln_Milestone.Acquisition, claimAcquisition);
                TracingService.Trace("Mapp to claimSoSDto");
                var claimSoSDto = ClaimMapper.MapToMilestoneClaimDto(deliveryPhase, (int)invln_Milestone.SoS, claimSoS);
                TracingService.Trace("Mapp to claimPCDto");
                var claimPCDto = ClaimMapper.MapToMilestoneClaimDto(deliveryPhase, (int)invln_Milestone.PC, claimPC);

                //Mapp delivery phase
                TracingService.Trace("Mapp to PhaseClaimsDto");
                listOfPhaseClaims.Add(DeliveryPhaseMapper.MapToPhaseClaimsDto(deliveryPhase, claimAcquisitionDto, claimSoSDto, claimPCDto));
            }

            // Mapp Allocation
            TracingService.Trace("Mapp to AllocationClaimsDto");
            result = AhpApplicationMapper.MapToAllocationClaimsDto(allocation, listOfPhaseClaims, _heLocalAuthorityRepository.GetById(allocation.invln_HELocalAuthorityID.Id));

            TracingService.Trace("Return Result");
            return result;
        }
    }
}
