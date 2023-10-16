using System;
using System.Collections.Generic;
using System.Text;

namespace HE.CRM.Model.CrmSerialiedParameters
{
    public class EXTERNAL_KYC_STATUS_CHANGE
    {
        public string templateId { get; set; }
        public parameters_EXTERNAL_KYC_STATUS_CHANGE personalisation { get; set; }

    }

    public class parameters_EXTERNAL_KYC_STATUS_CHANGE
    {
        // Required always
        public string subject { get; set; }
        public string recipientEmail { get; set; }
        // Additional
        public string contactDetails { get; set; }
    }
}
