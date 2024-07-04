using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
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
                || ValueChanged(invln_DeliveryPhase.Fields.invln_rehabactivitytype) || ValueChanged(invln_DeliveryPhase.Fields.invln_nbrh)
                 || ValueChanged(invln_DeliveryPhase.Fields.StatusCode);
        }

        public override void DoWork()
        {
            var resetMilestone = CurrentState.StatusCode.Value == (int)invln_DeliveryPhase_StatusCode.RejectedAdjustment;
            var application = _applicationRepository.GetById(CurrentState.invln_Application.Id);
            var milestoneframeworks = _milestoneFrameworkRepository.GetMilestoneFrameworkItemByProgrammeId(application.invln_programmelookup.Id.ToString());
            var df = CrmServicesFactory.Get<IDeliveryPhaseService>().CalculateFunding(application, CurrentState, milestoneframeworks, resetMilestone, CurrentState);
            if (df == null)
            {
                TracingService.Trace("df = null");
                return;
            }
            if (!resetMilestone)
            {
                ExecutionData.Target.invln_AcquisitionPercentageValue = df.invln_AcquisitionPercentageValue;
                ExecutionData.Target.invln_AcquisitionValue = df.invln_AcquisitionValue;
                ExecutionData.Target.invln_StartOnSitePercentageValue = df.invln_StartOnSitePercentageValue;
                ExecutionData.Target.invln_StartOnSiteValue = df.invln_StartOnSiteValue;
                ExecutionData.Target.invln_CompletionPercentageValue = df.invln_CompletionPercentageValue;
                ExecutionData.Target.invln_CompletionValue = df.invln_CompletionValue;
            }

        }
    }
}
