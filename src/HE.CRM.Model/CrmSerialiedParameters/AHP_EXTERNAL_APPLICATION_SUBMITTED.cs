using System;
using System.Collections.Generic;
using System.Text;

namespace HE.CRM.Model.CrmSerialiedParameters
{
    public class AHP_EXTERNAL_APPLICATION_SUBMITTED
    {
        public string templateId { get; set; }
        public parameters_AHP_EXTERNAL_APPLICATION_SUBMITTED personalisation { get; set; }
    }
    public class parameters_AHP_EXTERNAL_APPLICATION_SUBMITTED
    {
        // Required always
        public string subject { get; set; }
        public string recipientEmail { get; set; }
        // Additional
        public string name { get; set; }
        public string referencenumber { get; set; }
    }
}
