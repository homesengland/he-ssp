using System;

namespace HE.CRM.Plugins.Models.FrontDoor.Contract.Requests
{
    internal sealed class CheckProjectExistsRequest
    {
        public Guid PartnerRecordId { get; set; }

        public string ProjectName { get; set; }
    }
}
