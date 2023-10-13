using System;
using System.Collections.Generic;
using System.Text;

namespace HE.CRM.Model.CrmSerialiedParameters
{
    public class EXTERNAL_APPLICATION_ACTION_REQUIRED
    {
        public string templateId { get; set; }
        public parameters_EXTERNAL_APPLICATION_ACTION_REQUIRED personalisation { get; set; }

    }

    public class parameters_EXTERNAL_APPLICATION_ACTION_REQUIRED
    {
        // Required always
        public string subject { get; set; }
        public string recipientEmail { get; set; }
        // Additional
        public string username { get; set; }
        public string applicationId { get; set; }
    }
}
