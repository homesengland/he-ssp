using System;
using System.Collections.Generic;
using System.Text;

namespace HE.CRM.Model.CrmSerialiedParameters
{
    public class AHP_EXTERNAL_APPLICATION_ON_HOLD
    {
        public string templateId { get; set; }
        public parameters_AHP_EXTERNAL_APPLICATION_ON_HOLD personalisation { get; set; }
    }
    public class parameters_AHP_EXTERNAL_APPLICATION_ON_HOLD
    {
        // Required always
        public string subject { get; set; }
        public string recipientEmail { get; set; }
        // Additional
        public string name { get; set; }
        public string programmename { get; set; }
        public string applicationname { get; set; }
    }
}


