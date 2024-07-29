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
using HE.CRM.Common.Extensions.Entities;
using HE.CRM.Common.Repositories.Interfaces;
using HE.CRM.Model.CrmSerialiedParameters;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;

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
        private readonly ISharepointDocumentLocationRepository _sharepointDocumentLocationRepository;
        private readonly IHomeTypeRepository _homeTypeRepository;
        private readonly IHomesInDeliveryPhaseRepository _homesInDeliveryPhaseRepository;

        public AllocationService(CrmServiceArgs args) : base(args)
        {
            _ahpApplicationRepository = CrmRepositoriesFactory.Get<IAhpApplicationRepository>();
            _contactRepository = CrmRepositoriesFactory.Get<IContactRepository>();
            _accountRepository = CrmRepositoriesFactory.Get<IAccountRepository>();
            _contactWebroleRepository = CrmRepositoriesFactory.Get<IContactWebroleRepository>();
            _deliveryPhaseRepository = CrmRepositoriesFactory.Get<IDeliveryPhaseRepository>();
            _claimRepository = CrmRepositoriesFactory.Get<IClaimRepository>();
            _heLocalAuthorityRepository = CrmRepositoriesFactory.Get<IHeLocalAuthorityRepository>();
            _sharepointDocumentLocationRepository = CrmRepositoriesFactory.Get<ISharepointDocumentLocationRepository>();
            _homeTypeRepository = CrmRepositoriesFactory.Get<IHomeTypeRepository>();
            _homesInDeliveryPhaseRepository = CrmRepositoriesFactory.Get<IHomesInDeliveryPhaseRepository>();
        }

        public Guid CreateAllocation(Guid schemeId, bool isVariation = false)
        {
            var application = _ahpApplicationRepository.GetById(schemeId);

            if (application.StatusCode.Value != (int)invln_AHPInternalStatus.ApplicationSubmitted)
            {
                var internalStatus = (invln_AHPInternalStatus)application.StatusCode.Value;
                Logger.Warn($"Cannot create allocation from status {internalStatus}");
                return Guid.Empty;
            }

            var allocation = application.CloneWithoutSystemFields();
            allocation.Id = Guid.NewGuid();
            allocation.invln_isallocation = true;
            allocation.invln_BaseApplication = new EntityReference(invln_scheme.EntityLogicalName, schemeId);
            allocation.StatusCode = application.StatusCode;
            allocation.StateCode = application.StateCode;
            allocation.invln_schemename = $"{application.invln_schemename} 1";
            allocation.invln_AllocationID = "123";

            var allocationId = _ahpApplicationRepository.Create(allocation);

            MoveDocumentsToAllocation(schemeId, allocationId);

            //foreach (var deliveryPhase in deliveryPhaseList)
            //{
            //    var deliveryPhaseAllocation = deliveryPhase.CloneWithoutSystemFields();
            //    deliveryPhaseAllocation.Id = Guid.NewGuid();
            //    deliveryPhaseAllocation.invln_Application = new EntityReference(invln_scheme.EntityLogicalName, allocationId);
            //    _deliveryPhaseRepository.Create(deliveryPhaseAllocation);
            //}

            var homeTypeList = _homeTypeRepository.GetByAttribute(invln_HomeType.Fields.invln_application, application.Id);
            var homeTypeListForAllocation = homeTypeList
                .Select(ht =>
                {
                    ht.Id = Guid.NewGuid();
                    ht.invln_application = new EntityReference(invln_scheme.EntityLogicalName, allocationId);
                    return (Entity)ht;
                })
                .ToList();

            _homeTypeRepository.CreateMultiple(homeTypeListForAllocation);

            var deliveryPhaseList = _deliveryPhaseRepository.GetByAttribute(invln_DeliveryPhase.Fields.invln_Application, application.Id);
            var deliveryPhaseForAllocation = deliveryPhaseList
                .Select(df =>
                {
                    df.Id = Guid.NewGuid();
                    df.invln_Application = new EntityReference(invln_scheme.EntityLogicalName, allocationId);
                    return (Entity)df;
                }
                )
                .ToList();
            _ahpApplicationRepository.CreateMultiple(deliveryPhaseForAllocation);

            //_homesInDeliveryPhaseRepository.Create()

            return allocationId;
        }

        private void MoveDocumentsToAllocation(Guid fromApplicationId, Guid toApplicationId)
        {
            var query = new QueryExpression(SharePointDocumentLocation.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(true),
                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = SharePointDocumentLocation.EntityLogicalName,
                        LinkFromAttributeName = SharePointDocumentLocation.Fields.RegardingObjectId,
                        LinkToEntityName = invln_scheme.EntityLogicalName,
                        LinkToAttributeName = invln_scheme.PrimaryIdAttribute,
                        JoinOperator = JoinOperator.Inner,
                        LinkCriteria = new FilterExpression()
                        {
                            Conditions =
                            {
                                new ConditionExpression(invln_scheme.PrimaryIdAttribute, ConditionOperator.Equal, fromApplicationId)
                            }
                        }
                    }
                }
            };

            var documentLocations = _sharepointDocumentLocationRepository
                .RetrieveAll(query).Entities
                .Select(e => e.ToEntity<SharePointDocumentLocation>());

            foreach (var documentLocation in documentLocations)
            {
                var updateDocumentLocation = new SharePointDocumentLocation()
                {
                    Id = documentLocation.Id,
                    RegardingObjectId = new EntityReference(invln_scheme.EntityLogicalName, toApplicationId)
                };
                _sharepointDocumentLocationRepository.Update(updateDocumentLocation);
            }
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

        //private IEnumerable<invln_DeliveryPhase> GetCopyDeliveryPhases(Guid allocationId)
        //{
        //    _deliveryPhaseRepository.GetDeliveryPhasesForNullableUserAndOrganisationRelatedToApplication
        //}
    }
}
