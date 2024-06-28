using System;

namespace HE.CRM.Plugins.Models.FrontDoor.Contract
{
    public sealed class FrontDoorProjectContact
    {
        public Guid AccountId { get; set; }

        public Guid ContactId { get; set; }

        public string ContactFirstName { get; set; }

        public string ContactLastName { get; set; }

        public string ContactEmail { get; set; }

        public string ContactTelephoneNumber { get; set; }
    }
}
