using System;

namespace HE.CRM.Common.Api.FrontDoor.Contract.Responses
{
    public sealed class GetSiteResponse
    {
        public DateTimeOffset CreatedOn { get; set; }

        public string LocalAuthority { get; set; }

        public string LocalAuthorityName { get; set; }

        public Guid SiteId { get; set; }

        public string SiteName { get; set; }

        public int? NumberOfHomesEnabledBuilt { get; set; }

        public int? PlanningStatus { get; set; }

        public string LocalAuthorityCode { get; set; }

        public Guid ProjectRecordId { get; set; }
    }
}
