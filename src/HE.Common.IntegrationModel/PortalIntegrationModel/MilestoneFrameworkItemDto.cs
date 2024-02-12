using System;
using System.Collections.Generic;
using System.Text;

namespace HE.Common.IntegrationModel.PortalIntegrationModel
{
    public class MilestoneFrameworkItemDto
    {
        public string name { get; set; }
        public string programme { get; set; }
        public int? milestone { get; set; }
        public bool? isMilestonePayable { get; set; }
        public decimal? percentPaid { get; set; }
        public decimal? minimumValue { get; set; }
    }
}
