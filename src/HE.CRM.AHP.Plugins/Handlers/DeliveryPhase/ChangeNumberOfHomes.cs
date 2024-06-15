using System;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.DeliveryPhase;
using HE.CRM.Common.Repositories.Interfaces;

namespace HE.CRM.AHP.Plugins.Handlers.DeliveryPhase
{
    public class ChangeNumberOfHomes : CrmEntityHandlerBase<invln_DeliveryPhase, DataverseContext>
    {
        private readonly IAhpApplicationRepository _applicationRepository;
        private readonly IMilestoneFrameworkItemRepository _milestoneFrameworkRepository;

        public ChangeNumberOfHomes(IAhpApplicationRepository ahpApplicationRepository, IMilestoneFrameworkItemRepository milestoneFrameworkItemRepository)
        {
            _applicationRepository = ahpApplicationRepository;
            _milestoneFrameworkRepository = milestoneFrameworkItemRepository;
        }

        public override bool CanWork()
        {
            return ValueChanged(invln_DeliveryPhase.Fields.invln_NoofHomes) || ValueChanged(invln_DeliveryPhase.Fields.invln_buildactivitytype)
                || ValueChanged(invln_DeliveryPhase.Fields.invln_rehabactivitytype) || ValueChanged(invln_DeliveryPhase.Fields.invln_nbrh);
        }

        public override void DoWork()
        {
            Logger.Trace($"{DateTime.Now} - Start executing {GetType().Name}. UserId: {ExecutionData.Context.UserId}");
            Logger.Trace($"invln_NoofHomes: {CurrentState.invln_NoofHomes}, hasChanged: {ValueChanged(invln_DeliveryPhase.Fields.invln_NoofHomes)}");
            Logger.Trace($"invln_buildactivitytype: {CurrentState.invln_buildactivitytype?.Value}, hasChanged: {ValueChanged(invln_DeliveryPhase.Fields.invln_buildactivitytype)}");
            Logger.Trace($"invln_rehabactivitytype: {CurrentState.invln_rehabactivitytype?.Value}, hasChanged: {ValueChanged(invln_DeliveryPhase.Fields.invln_rehabactivitytype)}");
            Logger.Trace($"invln_nbrh: {CurrentState.invln_nbrh}, hasChanged: {ValueChanged(invln_DeliveryPhase.Fields.invln_nbrh)}");

            var application = _applicationRepository.GetById(CurrentState.invln_Application.Id);
            var milestoneframeworks = _milestoneFrameworkRepository.GetMilestoneFrameworkItemByProgrammeId(application.invln_programmelookup.Id.ToString());
            CrmServicesFactory.Get<IDeliveryPhaseService>().CalculateFunding(application, CurrentState, milestoneframeworks, ExecutionData.Target);
        }
    }
}
