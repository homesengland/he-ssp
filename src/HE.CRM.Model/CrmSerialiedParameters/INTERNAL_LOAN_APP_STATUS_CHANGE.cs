using System;
using System.Collections.Generic;
using System.Text;

namespace HE.CRM.Model.CrmSerializedParameters
{
    public class INTERNAL_LOAN_APP_STATUS_CHANGE
    {
        public string templateId { get; set; }
        public parameters_INTERNAL_LOAN_APP_STATUS_CHANGE personalisation { get; set; }
        
    }

    public class parameters_INTERNAL_LOAN_APP_STATUS_CHANGE
    {
        // Required always
        public string subject { get; set; }
        public string recipientEmail { get; set; }
        // Additional
        public string username { get; set; }
        public string applicationId { get; set; }
        public string applicationUrl { get; set; }
        public string statusAtBody { get; set; }
    }
}
