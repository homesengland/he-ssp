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


        public AllocationService(CrmServiceArgs args) : base(args)
        {
            _ahpApplicationRepository = CrmRepositoriesFactory.Get<IAhpApplicationRepository>();
            _contactRepository = CrmRepositoriesFactory.Get<IContactRepository>();
            _accountRepository = CrmRepositoriesFactory.Get<IAccountRepository>();
            _contactWebroleRepository = CrmRepositoriesFactory.Get<IContactWebroleRepository>();
            _deliveryPhaseRepository = CrmRepositoriesFactory.Get<IDeliveryPhaseRepository>();
            _claimRepository = CrmRepositoriesFactory.Get<IClaimRepository>();
        }


        public AllocationClaimsDto GetAllocationWithClaims(string externalContactId, Guid accountId, Guid allocationId)
        {
            TracingService.Trace("GetAllocationWithClaims");
            AllocationClaimsDto result = null;
            Contact contact = null;
            contact = _contactRepository.GetContactViaExternalId(externalContactId);


            var webroleList = _contactWebroleRepository.GetListOfUsersWithoutLimitedRole(accountId.ToString());
            var isOtherThanLimitedUser = webroleList.Exists(x => x.invln_Contactid.Id == contact.Id);
            var isLimitedUser = _contactWebroleRepository.IsContactHaveSelectedWebRoleForOrganisation(contact.Id, accountId, invln_Permission.Limiteduser);
            TracingService.Trace("WebRole checked");

            invln_scheme allocation = null;
            if (isLimitedUser == true && isOtherThanLimitedUser == false)
            {
                allocation = _ahpApplicationRepository.GetAllocation(allocationId, accountId, contact);
            }
            if (isOtherThanLimitedUser == true)
            {
                allocation = _ahpApplicationRepository.GetAllocation(allocationId, accountId);
            }

            if (allocation == null)
            {
                return result;
            }




            var listOfPhaseClaims = new List<PhaseClaimsDto>();

            // Pobieranie listy Delivery Phase dla Allocation
            var listOfDeliveryPhase = _deliveryPhaseRepository.GetDeliveryPhasesForAllocation(allocation.Id);

            foreach ( var deliveryPhase in listOfDeliveryPhase)
            {
                //Pobieranie Claimów dla DPhase
                var claimAcquisition = _claimRepository.GetClaimForAllocationDeliveryPhase(deliveryPhase.Id, (int)invln_Milestone.Acquisition);
                var claimSoS = _claimRepository.GetClaimForAllocationDeliveryPhase(deliveryPhase.Id, (int)invln_Milestone.SoS);
                var claimPC = _claimRepository.GetClaimForAllocationDeliveryPhase(deliveryPhase.Id, (int)invln_Milestone.PC);

                //Mapowanie Claim
                var claimAcquisitionDto = ClaimMapper.MapToMilestoneClaimDto(claimAcquisition);
                var claimSoSDto = ClaimMapper.MapToMilestoneClaimDto(claimSoS);
                var claimPCDto = ClaimMapper.MapToMilestoneClaimDto(claimPC);

                //Mapowanie delivery phase
                listOfPhaseClaims.Add(DeliveryPhaseMapper.MapToPhaseClaimsDto(deliveryPhase, claimAcquisitionDto, claimSoSDto, claimPCDto));

            }

            // Mapowanie Alocacji na DTO




            return result;
        }
    }
}
