using System;

namespace HE.CRM.Plugins.Models.FrontDoor.Contract.Requests
{
    internal sealed class SaveSiteRequest
    {
        public Guid ProjectRecordId { get; set; }

        public Guid? SiteId { get; set; }

        public string SiteName { get; set; }

        public int? NumberOfHomesEnabledBuilt { get; set; }

        public int? PlanningStatus { get; set; }

        public string LocalAuthorityCode { get; set; }
    }
}
