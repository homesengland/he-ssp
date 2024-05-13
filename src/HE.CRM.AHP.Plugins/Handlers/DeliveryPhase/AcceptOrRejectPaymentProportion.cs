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

namespace HE.CRM.AHP.Plugins.Handlers.DeliveryPhase
{
    public class AcceptOrRejectPaymentProportion : CrmEntityHandlerBase<invln_DeliveryPhase, DataverseContext>
    {
        private readonly IAhpApplicationRepository _applicationRepository;
        private readonly IMilestoneFrameworkItemRepository _milestoneFrameworkItemRepository;
        private readonly INotificationSettingRepository _notificationSettingRepository;
        private readonly IAccountRepository _accountRepository;

        public AcceptOrRejectPaymentProportion(IAhpApplicationRepository applicationRepository
            , IMilestoneFrameworkItemRepository milestoneFrameworkItemRepository,
            INotificationSettingRepository notificationSettingRepository,
            IAccountRepository accountRepository)
        {
            _applicationRepository = applicationRepository;
            _milestoneFrameworkItemRepository = milestoneFrameworkItemRepository;
            _notificationSettingRepository = notificationSettingRepository;
            _accountRepository = accountRepository;
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
                    invln_scheme.Fields.invln_GrowthManager, invln_scheme.Fields.invln_organisationid});
                ExecutionData.Target.invln_Dateofapproval = DateTime.Now;
                ExecutionData.Target.invln_Approvedby = CurrentState.ModifiedBy;

                //  AHPConst.adjustmentAccepted

                //if (ahpApplication.OwnerId.LogicalName == SystemUser.EntityLogicalName)
                //{
                this.TracingService.Trace(AHPConst.AdjustmentAccepted);
                var emailTemplate = _notificationSettingRepository.GetTemplateViaTypeName(AHPConst.AdjustmentAccepted);
                //    var ownerData = _systemUserRepositoryAdmin.GetById(ahpApplication.OwnerId.Id, nameof(SystemUser.InternalEMailAddress).ToLower(), nameof(SystemUser.FullName).ToLower());
                var account = _accountRepository.GetById(application.invln_organisationid.Id, nameof(Account.Name).ToLower());
                var subject = (account.Name ?? AHPConst.NoName) + " " + emailTemplate.invln_subject;
                var govNotParams = new AHP_DELIVERY_PHASE_NOTIFICATION_OF_ADDITIONAL_PAYMENTS_FOR_PHASE()
                {
                    templateId = emailTemplate?.invln_templateid,
                    personalisation = new parameters_AHP_DELIVERY_PHASE_NOTIFICATION_OF_ADDITIONAL_PAYMENTS_FOR_PHASE()
                    {
                        recipientEmail = ownerData.InternalEMailAddress,
                        subject = subject,
                        username = ownerData.FullName ?? "NO NAME",
                        organisationname = account.Name ?? "NO NAME",
                    }
                };

                //    var options = new JsonSerializerOptions
                //    {
                //        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                //        WriteIndented = true
                //    };

                //    var parameters = JsonSerializer.Serialize(govNotParams, options);
                //    this.SendGovNotifyEmail(ahpApplication.OwnerId, ahpApplication.ToEntityReference(), subject, parameters, emailTemplate);
                //}

            }

            if (CurrentState.StatusCode.Value == (int)invln_DeliveryPhase_StatusCode.RejectedAdjustment)
            {
                var application = _applicationRepository.GetById(CurrentState.invln_Application, new string[] { invln_scheme.Fields.invln_programmelookup });
                var milestonesFramework = _milestoneFrameworkItemRepository.GetMilestoneFrameworkItemByProgrammeId(application.invln_programmelookup.Id.ToString());
                ExecutionData.Target.invln_AcquisitionPercentageValue = milestonesFramework
                                                .FirstOrDefault(x => x.invln_milestone.Value == (int)invln_Milestone.Acquisition).invln_percentagepaidonmilestone.Value;
                ExecutionData.Target.invln_CompletionPercentageValue = milestonesFramework
                                .FirstOrDefault(x => x.invln_milestone.Value == (int)invln_Milestone.PC).invln_percentagepaidonmilestone.Value;
                ExecutionData.Target.invln_StartOnSitePercentageValue = milestonesFramework
                                .FirstOrDefault(x => x.invln_milestone.Value == (int)invln_Milestone.SoS).invln_percentagepaidonmilestone.Value;
            }
        }
    }
}
