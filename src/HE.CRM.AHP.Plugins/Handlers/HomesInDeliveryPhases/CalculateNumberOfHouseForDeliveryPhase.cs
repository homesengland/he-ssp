using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Common.Repositories.Interfaces;

namespace HE.CRM.AHP.Plugins.Handlers.HomesInDeliveryPhases
{
    public class CalculateNumberOfHouseForDeliveryPhase : CrmEntityHandlerBase<invln_homesindeliveryphase, DataverseContext>
    {
        private readonly IHomesInDeliveryPhaseRepository _homesInDeliveryPhaseRepository;
        private readonly IDeliveryPhaseRepository _deliveryPhaseRepository;

        public CalculateNumberOfHouseForDeliveryPhase(IHomesInDeliveryPhaseRepository homesInDeliveryPhaseRepository,
                                                    IDeliveryPhaseRepository deliveryPhaseRepository)
        {
            _homesInDeliveryPhaseRepository = homesInDeliveryPhaseRepository;
            _deliveryPhaseRepository = deliveryPhaseRepository;
        }

        public override bool CanWork()
        {
            if (ExecutionData.Context.SharedVariables.Contains("tag"))
            {
                ExecutionData.Context.SharedVariables.TryGetValue("tag", out string tag);
                if (tag == "Cloning")
                {
                    Logger.Info($"SharedVariables: tag={tag}");
                    return false;
                }
            }
            return CurrentState.invln_numberofhomes != null;
        }

        public override void DoWork()
        {
            TracingService.Trace("Do Work: CalculateNumberOfHouseForDeliveryPhase");
            TracingService.Trace("Get Homes In Delivery Phase");
            var homesInDeliveryPhases = _homesInDeliveryPhaseRepository.GetByAttribute(invln_homesindeliveryphase.Fields.invln_deliveryphaselookup, CurrentState.invln_deliveryphaselookup.Id,
                                                                        new string[] { invln_homesindeliveryphase.Fields.invln_numberofhomes });
            TracingService.Trace("Calculate Number of homes");
            var numberOfHomes = homesInDeliveryPhases.Sum(x => x.invln_numberofhomes).Value;
            TracingService.Trace("Update Delivery Phase");
            var deliveryPhaseToUpdate = new invln_DeliveryPhase()
            {
                Id = CurrentState.invln_deliveryphaselookup.Id,
                invln_NoofHomes = numberOfHomes,
            };

            _deliveryPhaseRepository.Update(deliveryPhaseToUpdate);
            TracingService.Trace("Finish");
        }
    }
}
