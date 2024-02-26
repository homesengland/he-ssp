using System;
using System.Collections.Generic;
using System.Text;

namespace HE.CRM.Model.CrmSerialiedParameters
{
    public class EXTERNAL_APPLICATION_CASHFLOW_REQUESTED
    {
        public string templateId { get; set; }
        public parameters_EXTERNAL_APPLICATION_CASHFLOW_REQUESTED personalisation { get; set; }

    }

    public class parameters_EXTERNAL_APPLICATION_CASHFLOW_REQUESTED
    {
        // Required always
        public string subject { get; set; }
        public string recipientEmail { get; set; }
        // Additional
        public string username { get; set; }
        public string applicationId { get; set; }
    }
}
