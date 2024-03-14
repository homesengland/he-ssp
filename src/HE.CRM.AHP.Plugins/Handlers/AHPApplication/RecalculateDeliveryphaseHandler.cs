using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;
using static System.Net.Mime.MediaTypeNames;

namespace HE.CRM.AHP.Plugins.Handlers.AHPApplication
{
    public class RecalculateDeliveryphaseHandler : CrmEntityHandlerBase<invln_scheme, DataverseContext>
    {
        private readonly IMilestoneFrameworkItemRepository _milestonesRepository;
        private readonly IDeliveryPhaseRepository _deliveryPhaseRepository;
        private readonly IContactRepository _contactRepository;
        public RecalculateDeliveryphaseHandler(IMilestoneFrameworkItemRepository milestonesRepository, IDeliveryPhaseRepository deliveryPhaseRepository, IContactRepository contactRepository)
        {
            this._milestonesRepository = milestonesRepository;
            this._deliveryPhaseRepository = deliveryPhaseRepository;
            this._contactRepository = contactRepository;
        }
        public override bool CanWork()
        {
            return ValueChanged(invln_scheme.Fields.invln_noofhomes) || ValueChanged(invln_scheme.Fields.invln_fundingrequired);
        }

        public override void DoWork()
        {
            var contact = _contactRepository.GetById(CurrentState.invln_contactid);
            var milestones = _milestonesRepository.
                    GetByAttribute(invln_milestoneframeworkitem.Fields.invln_programmeId, CurrentState.invln_programmelookup).ToList();

            var deliveryPhases = _deliveryPhaseRepository.GetByAttribute(invln_DeliveryPhase.Fields.invln_Application, CurrentState.Id).ToList();

            foreach (var deliveryphase in deliveryPhases)
            {
                var numberOfHouseApplication = CurrentState.invln_noofhomes.Value;
                var numberOfHousePhase = deliveryphase.invln_NoofHomes.Value;
                var fundingRequired = CurrentState.invln_fundingrequired.Value;
                var acquisitionPercentageValue = milestones.FirstOrDefault(x => x.invln_milestone.Value == (int)invln_Milestone.Acquisition).invln_percentagepaidonmilestone.Value;
                var startOnSitePercentageValue = milestones.FirstOrDefault(x => x.invln_milestone.Value == (int)invln_Milestone.SoS).invln_percentagepaidonmilestone.Value;
                var completionPercentageValue = milestones.FirstOrDefault(x => x.invln_milestone.Value == (int)invln_Milestone.PC).invln_percentagepaidonmilestone.Value;
                deliveryphase.invln_AcquisitionValue = new Money((fundingRequired / numberOfHouseApplication) * numberOfHousePhase * acquisitionPercentageValue / 100);
                deliveryphase.invln_AcquisitionPercentageValue = acquisitionPercentageValue;
                deliveryphase.invln_StartOnSiteValue = new Money((fundingRequired / numberOfHouseApplication) * numberOfHousePhase * startOnSitePercentageValue / 100);
                deliveryphase.invln_StartOnSitePercentageValue = startOnSitePercentageValue;
                deliveryphase.invln_CompletionValue = new Money((fundingRequired / numberOfHouseApplication) * numberOfHousePhase * completionPercentageValue / 100);
                deliveryphase.invln_CompletionPercentageValue = completionPercentageValue;
            }
            CurrentState.invln_lastexternalmodificationon = DateTime.UtcNow;
            CurrentState.invln_lastexternalmodificationby = new EntityReference(Contact.EntityLogicalName, contact.Id);
        }
    }
}
