using System;
using System.Collections.Generic;
using System.Text;

namespace HE.CRM.Model.CrmSerialiedParameters
{
    public class COMMON_REQUEST_TO_ASSIGN_ISP_TO_USER
    {
        public string templateId { get; set; }
        public parameters_COMMON_REQUEST_TO_ASSIGN_ISP_TO_USER personalisation { get; set; }

    }

    public class parameters_COMMON_REQUEST_TO_ASSIGN_ISP_TO_USER
    {
        // Required always
        public string subject { get; set; }
        public string recipientEmail { get; set; }
        // Additional
        public string username { get; set; }
        public string applicationId { get; set; }
        public string applictionUrl { get; set; }
    }
}
