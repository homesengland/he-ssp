using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Services;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Services.GovNotifyEmail
{
    public interface IGovNotifyEmailService : ICrmService
    {
        void SendNotifications_COMMON_CHANGE_EXTERNAL_USER_PERMISSIONS(EntityReference contactWebrole);

        void SendNotifications_COMMON_GRANT_ORGANISATION_ADMIN_PERMISSIONS(EntityReference contactWebroleId);

        void SendNotifications_COMMON_INVITE_CONTACT_TO_JOIN_ORGANIZATION(EntityReference invitedContactId, EntityReference _inviterContactId, EntityReference organisationId);
        void SendNotifications_COMMON_INVITE_CONTACT_TO_JOIN_ORGANIZATION_BY_POWER_APPS(EntityReference invitedContactId, EntityReference userId);

        void SendNotifications_AHP_EXTERNAL_REMINDER_TO_FINALIZE_APPLICATION_REFERRED_BACK(EntityReference ahpApplicationId, EntityReference contactId);

        void SendNotifications_AHP_EXTERNAL_REMINDER_TO_FINALIZE_DRAFT_APPLICATION(EntityReference ahpApplicationId, EntityReference contactId);

        void SendNotification_AHP_DELIVERY_PHASE_NOTIFICATION_OF_ADJUSTMENT_ACCEPTED(invln_scheme ahpApplication);

        void SendNotifications_AHP_INTERNAL_EXTERNAL_WANTS_ADDITIONAL_PAYMENTS_FOR_PHASE(invln_AHPStatusChange ahpStatusChange, invln_scheme ahpApplication);

        void SendNotifications_AHP_EXTERNAL_REJECT_ON_DELIVERY_PHASE(EntityReference deliveryPhaseId);

        void SendNotifications_AHP_INTERNAL_REQUEST_TO_WITHDRAW(invln_AHPStatusChange ahpStatusChange, invln_scheme ahpApplication);

        void SendNotifications_AHP_EXTERNAL_APPLICATION_SUBMITTED(invln_AHPStatusChange ahpStatusChange, invln_scheme ahpApplication);

        void SendNotifications_AHP_EXTERNAL_APPLICATION_REFERRED_BACK_TO_APPLICANT(invln_AHPStatusChange ahpStatusChange, invln_scheme ahpApplication);

        void SendNotifications_AHP_INTERNAL_APPLICATION_APPROVED_SUBJECT_TO_CONTRACT(invln_AHPStatusChange ahpStatusChange, invln_scheme ahpApplication);

        void SendNotifications_AHP_INTERNAL_APPLICATION_ON_HOLD(invln_AHPStatusChange ahpStatusChange, invln_scheme ahpApplication);

        void SendNotifications_AHP_EXTERNAL_APPLICATION_ON_HOLD(invln_AHPStatusChange ahpStatusChange, invln_scheme ahpApplication);

        void SendNotifications_AHP_EXTERNAL_APPLICATION_REJECTED(invln_AHPStatusChange ahpStatusChange, invln_scheme ahpApplication);
        void SendNotifications_AHP_EXTERNAL_APPLICATION_APPROVED_OUTCOME(invln_AHPStatusChange ahpStatusChange, invln_scheme ahpApplication);
        
    }
}
