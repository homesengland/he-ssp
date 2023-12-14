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
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi
{
    public class SentInviteContactToJoinOrganisationHandler : CrmActionHandlerBase<invln_sendinvitecontacttojoinorganisationRequest, DataverseContext>
    {
        #region Fields

        private string invitedContactId => ExecutionData.GetInputParameter<string>(invln_sendinvitecontacttojoinorganisationRequest.Fields.invitedcontactid);
        private string inviterContactId => ExecutionData.GetInputParameter<string>(invln_sendinvitecontacttojoinorganisationRequest.Fields.invitercontactid);
        private string accountId => ExecutionData.GetInputParameter<string>(invln_sendinvitecontacttojoinorganisationRequest.Fields.organisationid);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return accountId != null && invitedContactId != null && inviterContactId != null;
        }

        public override void DoWork()
        {
            TracingService.Trace("SentInviteContactToJoinOrganisationHandler");
            if (Guid.TryParse(invitedContactId, out Guid _invitedContactId) && Guid.TryParse(inviterContactId, out Guid _inviterContactId) && Guid.TryParse(accountId, out Guid _accountId))
            {

                CrmServicesFactory.Get<IGovNotifyEmailService>().SendNotifications_COMMON_INVITE_CONTACT_TO_JOIN_ORGANIZATION(
                    new EntityReference(Contact.EntityLogicalName, _invitedContactId),
                    new EntityReference(Contact.EntityLogicalName, _inviterContactId),
                    new EntityReference(Account.EntityLogicalName, _accountId)
                    );
            }
        }

        #endregion
    }
}
