using System;

namespace HE.CRM.Plugins.Models.FrontDoor.Contract.Requests
{
    internal sealed class DeactivateProjectRequest
    {
        public Guid ProjectRecordId { get; set; }
    }
}
