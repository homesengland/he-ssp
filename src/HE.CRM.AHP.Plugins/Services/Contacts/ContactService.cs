using System;
using System.Linq;
using HE.Base.Services;
using HE.CRM.AHP.Plugins.Services.GovNotifyEmail;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;
using DataverseModel;

namespace HE.CRM.AHP.Plugins.Services.Contacts
{
    public class ContactService : CrmService, IContactService
    {
        private readonly IContactRepository _contactRepository;

        private readonly IGovNotifyEmailService _govNotifyEmailService;
        public ContactService(CrmServiceArgs args) : base(args)
        {
            _contactRepository = CrmRepositoriesFactory.Get<IContactRepository>();

            _govNotifyEmailService = CrmServicesFactory.Get<IGovNotifyEmailService>();
        }

        public void InviteContactToJoinExistingOrganisation(Guid inviterId, Guid invitedContactId, Guid organisationId)
        {
            _govNotifyEmailService.SendNotifications_COMMON_INVITE_CONTACT_TO_JOIN_ORGANIZATION(new EntityReference(Contact.EntityLogicalName, invitedContactId),
                new EntityReference(Contact.EntityLogicalName, inviterId), new EntityReference(Account.EntityLogicalName, organisationId));
        }
    }
}
