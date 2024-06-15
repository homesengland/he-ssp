using System;
using System.Linq;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.DeliveryPhase;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;

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
            TracingService.Trace("Can Work");
            return ValueChanged(invln_scheme.Fields.invln_noofhomes) || ValueChanged(invln_scheme.Fields.invln_fundingrequired);
        }

        public override void DoWork()
        {
            Logger.Trace($"{DateTime.Now} - Start executing {GetType().Name}. UserId: {ExecutionData.Context.UserId}");

            var contact = _contactRepository.GetById(CurrentState.invln_contactid);
            var milestones = _milestonesRepository.
                    GetByAttribute(invln_milestoneframeworkitem.Fields.invln_programmeId, CurrentState.invln_programmelookup.Id).ToList();

            var deliveryPhases = _deliveryPhaseRepository.GetByAttribute(invln_DeliveryPhase.Fields.invln_Application, CurrentState.Id).ToList();

            foreach (var deliveryphase in deliveryPhases)
            {
                CrmServicesFactory.Get<IDeliveryPhaseService>().CalculateFunding(CurrentState, deliveryphase, milestones, deliveryphase);
                _deliveryPhaseRepository.Update(deliveryphase);
            }
            ExecutionData.Target.invln_lastexternalmodificationon = DateTime.UtcNow;
            ExecutionData.Target.invln_lastexternalmodificationby = new EntityReference(Contact.EntityLogicalName, contact.Id);
        }
    }
}
