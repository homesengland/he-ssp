using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Common.Repositories.Interfaces;
using HE.CRM.AHP.Plugins.Common;
using HE.CRM.Model.CrmSerialiedParameters;
using HE.CRM.AHP.Plugins.Services.GovNotifyEmail;

namespace HE.CRM.AHP.Plugins.Handlers.DeliveryPhase
{
    public class AcceptOrRejectPaymentProportion : CrmEntityHandlerBase<invln_DeliveryPhase, DataverseContext>
    {
        private readonly IAhpApplicationRepository _applicationRepository;
        private readonly IMilestoneFrameworkItemRepository _milestoneFrameworkItemRepository;
        private readonly IGovNotifyEmailService _govNotifyEmailService;

        public AcceptOrRejectPaymentProportion(IAhpApplicationRepository applicationRepository,
                IMilestoneFrameworkItemRepository milestoneFrameworkItemRepository,
                IGovNotifyEmailService govNotifyEmailService)
        {
            _applicationRepository = applicationRepository;
            _milestoneFrameworkItemRepository = milestoneFrameworkItemRepository;
            _govNotifyEmailService = govNotifyEmailService;
        }

        public override bool CanWork()
        {
            return ValueChanged(invln_DeliveryPhase.Fields.StatusCode);
        }

        public override void DoWork()
        {
            if (CurrentState.StatusCode.Value == (int)invln_DeliveryPhase_StatusCode.AdjustmentAccepted)
            {
                var application = _applicationRepository
                    .GetById(CurrentState.invln_Application,
                    new string[] { invln_scheme.Fields.invln_programmelookup,
                    invln_scheme.Fields.invln_GrowthManager,
                    invln_scheme.Fields.invln_organisationid,
                    invln_scheme.Fields.invln_contactid,
                    invln_scheme.Fields.OwnerId});
                ExecutionData.Target.invln_Dateofapproval = DateTime.Now;
                ExecutionData.Target.invln_Approvedby = CurrentState.ModifiedBy;

                _govNotifyEmailService.SendNotification_AHP_DELIVERY_PHASE_NOTIFICATION_OF_ADJUSTMENT_ACCEPTED(application);
            }
        }
    }
}
