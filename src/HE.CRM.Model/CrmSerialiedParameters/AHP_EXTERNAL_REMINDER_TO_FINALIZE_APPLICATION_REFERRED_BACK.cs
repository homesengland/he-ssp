using System;
using System.Collections.Generic;
using System.Text;

namespace HE.CRM.Model.CrmSerialiedParameters
{
    public class AHP_EXTERNAL_REMINDER_TO_FINALIZE_APPLICATION_REFERRED_BACK
    {
        public string templateId { get; set; }
        public parameters_AHP_EXTERNAL_REMINDER_TO_FINALIZE_APPLICATION_REFERRED_BACK personalisation { get; set; }

    }

    public class parameters_AHP_EXTERNAL_REMINDER_TO_FINALIZE_APPLICATION_REFERRED_BACK
    {
        // Required always
        public string subject { get; set; }
        public string recipientEmail { get; set; }
        // Additional
        public string username { get; set; }
        public string programmename { get; set; }
    }
}
