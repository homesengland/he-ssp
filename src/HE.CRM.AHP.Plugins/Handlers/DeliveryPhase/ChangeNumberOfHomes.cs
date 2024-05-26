using System;
using System.Collections.Generic;
using System.Linq;
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
            return ValueChanged(invln_DeliveryPhase.Fields.invln_NoofHomes);
        }

        public override void DoWork()
        {
            var application = _applicationRepository.GetById(CurrentState.invln_Application.Id);
            var milestoneframeworks = _milestoneFrameworkRepository.GetMilestoneFrameworkItemByProgrammeId(application.invln_programmelookup.Id.ToString());
            CrmServicesFactory.Get<IDeliveryPhaseService>().CalculateFunding(application, CurrentState, milestoneframeworks, ExecutionData.Target);
        }
    }
}
