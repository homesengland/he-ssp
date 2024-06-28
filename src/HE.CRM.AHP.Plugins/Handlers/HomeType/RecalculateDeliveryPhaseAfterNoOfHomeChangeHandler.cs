using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.DeliveryPhase;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Handlers.HomeType
{
    public class RecalculateDeliveryPhaseAfterNoOfHomeChangeHandler : CrmEntityHandlerBase<invln_HomeType, DataverseContext>
    {
        private readonly IAhpApplicationRepository _applicationRepository;
        private readonly IMilestoneFrameworkItemRepository _milestoneFrameworkItemRepository;
        private readonly IDeliveryPhaseRepository _deliveryPhaseRepository;

        public RecalculateDeliveryPhaseAfterNoOfHomeChangeHandler(
            IAhpApplicationRepository applicationRepository, IMilestoneFrameworkItemRepository milestoneFrameworkItemRepository,
            IDeliveryPhaseRepository deliveryPhaseRepository)
        {
            _applicationRepository = applicationRepository;
            _milestoneFrameworkItemRepository = milestoneFrameworkItemRepository;
            _deliveryPhaseRepository = deliveryPhaseRepository;
        }

        public override bool CanWork()
        {
            return ValueChanged(invln_HomeType.Fields.invln_numberofhomeshometype);
        }

        public override void DoWork()
        {
            var application = _applicationRepository.GetById(CurrentState.invln_application.Id);

            TracingService.Trace("Do Work");
            var milestones = _milestoneFrameworkItemRepository.
                    GetByAttribute(invln_milestoneframeworkitem.Fields.invln_programmeId, application.invln_programmelookup.Id).ToList();

            var deliveryPhases = _deliveryPhaseRepository.GetByAttribute(invln_DeliveryPhase.Fields.invln_Application, application.Id).ToList();

            foreach (var deliveryphase in deliveryPhases)
            {
                CrmServicesFactory.Get<IDeliveryPhaseService>().CalculateFunding(application, deliveryphase, milestones, false, deliveryphase);
                _deliveryPhaseRepository.Update(deliveryphase);
            }
        }
    }
}
