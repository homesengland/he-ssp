using System;
using System.Collections.Generic;
using System.Text;

namespace HE.CRM.Model.CrmSerialiedParameters
{
    public class EXTERNAL_APPLICATION_STATUS_CONFIRMATION
    {
        public string templateId { get; set; }
        public parameters_EXTERNAL_APPLICATION_STATUS_CONFIRMATION personalisation { get; set; }

    }

    public class parameters_EXTERNAL_APPLICATION_STATUS_CONFIRMATION
    {
        // Required always
        public string subject { get; set; }
        public string recipientEmail { get; set; }
        // Additional
        public string username { get; set; }
        public string actionCompleted { get; set; }
    }
}
