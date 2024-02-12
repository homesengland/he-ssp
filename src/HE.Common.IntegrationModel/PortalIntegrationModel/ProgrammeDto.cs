using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Microsoft.Crm.Sdk.Messages;

namespace HE.Common.IntegrationModel.PortalIntegrationModel
{
    public class ProgrammeDto
    {
        public string name { get; set; }
        public System.DateTime? startOn { get; set; }
        public System.DateTime? endOn { get; set; }
        public System.DateTime? startOnSiteStartDate { get; set; }
        public System.DateTime? startOnSiteEndDate { get; set; }
        public System.DateTime? completionStartDate { get; set; }
        public System.DateTime? completionEndDate { get; set; }
        public System.DateTime? assignFundingStartDate { get; set; }
        public System.DateTime? assignFundingEndDate { get; set; }
        public List<MilestoneFrameworkItemDto> milestoneFrameworkItem { get; set; }
    }
}



