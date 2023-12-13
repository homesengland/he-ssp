using System;
using System.Collections.Generic;
using System.Text;

namespace HE.CRM.Model.CrmSerialiedParameters
{
    public class COMMON_CHANGE_EXTERNAL_USER_PERMISSIONS
    {
        public string templateId { get; set; }
        public parameters_COMMON_CHANGE_EXTERNAL_USER_PERMISSIONS personalisation { get; set; }

    }

    public class parameters_COMMON_CHANGE_EXTERNAL_USER_PERMISSIONS
    {
        // Required always
        public string subject { get; set; }
        public string recipientEmail { get; set; }
        // Additional
        public string username { get; set; }
    }
}
