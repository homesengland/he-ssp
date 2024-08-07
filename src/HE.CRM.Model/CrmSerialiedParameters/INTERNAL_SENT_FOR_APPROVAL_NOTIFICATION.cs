using System;
using System.Collections.Generic;
using System.Text;

namespace HE.CRM.Model.CrmSerialiedParameters
{
    public class INTERNAL_SENT_FOR_APPROVAL_NOTIFICATION
    {
        public string templateId { get; set; }
        public parameters_INTERNAL_SENT_FOR_APPROVAL_NOTIFICATION personalisation { get; set; }

    }

    public class parameters_INTERNAL_SENT_FOR_APPROVAL_NOTIFICATION
    {
        // Required always
        public string subject { get; set; }
        public string recipientEmail { get; set; }
        // Additional
        public string username { get; set; }
        public string applicationId { get; set; }
        public string applicationUrl { get; set; }
    }
}
