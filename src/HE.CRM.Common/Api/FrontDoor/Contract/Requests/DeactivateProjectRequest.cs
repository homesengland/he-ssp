using System;

namespace HE.CRM.Common.Api.FrontDoor.Contract.Requests
{
    internal sealed class DeactivateProjectRequest
    {
        public Guid ProjectRecordId { get; set; }
    }
}
