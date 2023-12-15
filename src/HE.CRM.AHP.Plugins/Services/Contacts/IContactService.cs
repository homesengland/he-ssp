using System;
using HE.Base.Services;

namespace HE.CRM.AHP.Plugins.Services.Contacts
{
    public interface IContactService : ICrmService
    {
        void SendRequestToAssignContactToExistingOrganisation(Guid organisationId, Guid contactId);
        void InviteContactToJoinExistingOrganisation(Guid inviterId, Guid invitedContactId, Guid organisationId);
    }
}
