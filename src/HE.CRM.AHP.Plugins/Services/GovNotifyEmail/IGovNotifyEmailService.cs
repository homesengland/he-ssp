
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
        //void SendNotifications_EXTERNAL_APPLICATION_ACTION_REQUIRED(invln_AHPStatusChange statusChange, invln_scheme ahpApplication, string actionRequired);
        void SendNotifications_COMMON_CHANGE_EXTERNAL_USER_PERMISSIONS(EntityReference contactWebrole);
        void SendNotifications_COMMON_GRANT_ORGANISATION_ADMIN_PERMISSIONS(EntityReference contactWebroleId);
        void SendNotifications_COMMON_INVITE_CONTACT_TO_JOIN_ORGANIZATION(EntityReference invitedContactId, EntityReference _inviterContactId, EntityReference organisationId);
        void SendNotifications_AHP_EXTERNAL_REMINDER_TO_FINALIZE_APPLICATION_REFERRED_BACK(EntityReference ahpApplicationId, EntityReference contactId);
        void SendNotifications_AHP_EXTERNAL_REMINDER_TO_FINALIZE_DRAFT_APPLICATION(EntityReference ahpApplicationId, EntityReference contactId);
        void SendNotifications_AHP_INTERNAL_EXTERNAL_WANTS_ADDITIONAL_PAYMENTS_FOR_PHASE(EntityReference deliveryPhaseId);
        void SendNotifications_AHP_INTERNAL_REQUEST_TO_WITHDRAW(invln_AHPStatusChange ahpStatusChange, invln_scheme ahpApplication);

    }
}
