using System;
using System.Collections.Generic;
using System.Text;

namespace HE.CRM.Model.CrmSerialiedParameters
{
    public class AHP_EXTERNAL_REJECT_ON_DELIVERY_PHASE
    {
        public string templateId { get; set; }
        public parameters_AHP_EXTERNAL_REJECT_ON_DELIVERY_PHASE personalisation { get; set; }
    }
    public class parameters_AHP_EXTERNAL_REJECT_ON_DELIVERY_PHASE
    {
        // Required always
        public string subject { get; set; }
        public string recipientEmail { get; set; }
        // Additional
        public string name { get; set; }
    }
}

