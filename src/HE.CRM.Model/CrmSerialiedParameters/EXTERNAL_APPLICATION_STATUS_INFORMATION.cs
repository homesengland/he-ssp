using System;
using System.Collections.Generic;
using System.Text;

namespace HE.CRM.Model.CrmSerialiedParameters
{
    public class EXTERNAL_APPLICATION_STATUS_INFORMATION
    {
        public string templateId { get; set; }
        public parameters_EXTERNAL_APPLICATION_STATUS_INFORMATION personalisation { get; set; }

    }

    public class parameters_EXTERNAL_APPLICATION_STATUS_INFORMATION
    {
        // Required always
        public string subject { get; set; }
        public string recipientEmail { get; set; }
        // Additional
        public string username { get; set; }
        public string applicationId { get; set; }
        public string previousStatus { get; set; }
        public string newStatus { get; set; }
    }
}
