using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.GovNotifyEmail;
using HE.CRM.AHP.Plugins.Services.HomeType;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi
{
    internal class SentInviteContactToJoinOrganisationPowerAppsHandler : CrmActionHandlerBase<invln_sendinvitationtocontacttojoinorganizationbypowerappsRequest, DataverseContext>
    {
        private string userId => ExecutionData.GetInputParameter<string>(invln_sendinvitationtocontacttojoinorganizationbypowerappsRequest.Fields.invln_userid);
        private string contactId => ExecutionData.GetInputParameter<string>(invln_sendinvitationtocontacttojoinorganizationbypowerappsRequest.Fields.invln_contactid);

        public override bool CanWork()
        {
            TracingService.Trace("CanWork - SentInviteContactToJoinOrganisationPowerAppsHandler");
            TracingService.Trace($"userId : {userId}");
            TracingService.Trace($"contactId : {contactId}");

            return userId != null && contactId != null;
        }

        public override void DoWork()
        {
            TracingService.Trace("DoWork - SentInviteContactToJoinOrganisationPowerAppsHandler");
            if (Guid.TryParse(userId, out Guid userGuid) && Guid.TryParse(contactId, out Guid contactGuid))
            {

                CrmServicesFactory.Get<IGovNotifyEmailService>().SendNotifications_COMMON_INVITE_CONTACT_TO_JOIN_ORGANIZATION_BY_POWER_APPS(
                    new EntityReference(Contact.EntityLogicalName, contactGuid),
                    new EntityReference(SystemUser.EntityLogicalName, userGuid)
                    );
            }
        }
    }
}
