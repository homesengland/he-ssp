using System;

namespace HE.CRM.Common.Api.FrontDoor.Contract.Requests
{
    public sealed class CheckProjectExistsRequest
    {
        public Guid PartnerRecordId { get; set; }

        public string ProjectName { get; set; }
    }
}
