using System;

namespace HE.CRM.Common.Api.FrontDoor.Contract.Requests
{
    public sealed class DeactivateProjectRequest
    {
        public Guid ProjectRecordId { get; set; }
    }
}
